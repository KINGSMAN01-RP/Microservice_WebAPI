using EnquiryMaster_WebAPI.Data;
using EnquiryMaster_WebAPI.DTOs.EnquiryDTO;
using EnquiryMaster_WebAPI.Enums;
using EnquiryMaster_WebAPI.ExtensionMethods.EnquiryQueryableExtensions;
using EnquiryMaster_WebAPI.Models.EnquiryModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EnquiryMaster_WebAPI.Repositories.EnquiryMaster
{
    public class EnquiryRepository : IEnquiryRepository
    {
        private readonly EnquiryDbContext _dbcontext;

        public EnquiryRepository(EnquiryDbContext enquiryDbContext)
        {
            _dbcontext = enquiryDbContext;
        }

        /// <summary>
        /// Filtered + paged list suitable for master screen grid.
        /// Projects to EnquiryListItemDto to reduce payload.
        /// </summary>
        
        public async Task<PagedResult<EnquiryListItemDto>> ListAsync(EnquiryFilterDto filter, CancellationToken ct = default)
        {
            filter ??= new EnquiryFilterDto();

            var baseQuery = _dbcontext.Enquiries.AsQueryable()
                .ApplyFilter(filter)
                .ApplySorting(filter);

            // total count for pagination
            var total = await baseQuery.CountAsync(ct);

            // projection (only fields needed in the grid)
            var projected = baseQuery.Select(e => new EnquiryListItemDto
            {
                EnquiryId = e.EnquiryId,
                EnquiryName = e.EnquiryName,
                Status = e.Status,
                Priority = e.Priority,
                Category = e.Category,
                SubCategory = e.SubCategory,
                CustomerName = e.CustomerName,
                Phone = e.Phone,
                Email = e.Email,
                AssignedToUserId = e.AssignedToUserId,
                CreatedOn = e.CreatedOn,
                DueDate = e.DueDate,
                NextFollowUpOn = e.NextFollowUpOn
            });

            var items = await projected
                .ApplyPaging(filter.Page, filter.PageSize)
                .ToListAsync(ct);

            return new PagedResult<EnquiryListItemDto>
            {
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalCount = total,
                Items = items
            };
        }



        /// <summary>
        /// Backward compatibility: returns all entities (avoid for big sets).
        /// </summary>
        public async Task<IEnumerable<Enquiry>> GetAllAsync(CancellationToken ct = default)
        {
            return await _dbcontext.Enquiries
                .Where(e => !e.IsDeleted)
                .ToListAsync(ct);
        }

        public async Task<Enquiry?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _dbcontext.Enquiries
                .FirstOrDefaultAsync(e => e.EnquiryId == id, ct);
        }

        public async Task<Enquiry> CreateAsync(Enquiry enquiry, CancellationToken ct = default)
        {
            // Server-side defaults
            enquiry.Status = EnquiryStatus.Open; 
            enquiry.CreatedOn = DateTime.UtcNow;
            enquiry.IsActive = true;
            enquiry.IsDeleted = false;

            _dbcontext.Enquiries.Add(enquiry);
            await _dbcontext.SaveChangesAsync(ct);
            return enquiry;
        }

        public async Task<Enquiry?> UpdateAsync(Enquiry enquiry, CancellationToken ct = default)
        {
            var old = await _dbcontext.Enquiries.FirstOrDefaultAsync(e => e.EnquiryId == enquiry.EnquiryId, ct);
            if (old == null) return null;

            // Map fields (manual mapping; replace with AutoMapper if you prefer)
            old.EnquiryName = enquiry.EnquiryName;
            old.Description = enquiry.Description;
            old.Status = enquiry.Status;
            old.Priority = enquiry.Priority;
            old.Category = enquiry.Category;
            old.SubCategory = enquiry.SubCategory;
            old.Source = enquiry.Source;
            old.Channel = enquiry.Channel;
            old.CampaignCode = enquiry.CampaignCode;
            old.ReferenceNumber = enquiry.ReferenceNumber;
            old.CustomerName = enquiry.CustomerName;
            old.Email = enquiry.Email;
            old.Phone = enquiry.Phone;
            old.Address = enquiry.Address;
            old.AssignedToUserId = enquiry.AssignedToUserId;
            old.AssignedToTeamId = enquiry.AssignedToTeamId;
            old.DueDate = enquiry.DueDate;
            old.NextFollowUpOn = enquiry.NextFollowUpOn;
            old.ResolutionNotes = enquiry.ResolutionNotes;
            old.Tags = enquiry.Tags;
            old.MetadataJson = enquiry.MetadataJson;
            old.IsActive = enquiry.IsActive;
            old.BranchId = enquiry.BranchId;
            old.OrgUnit = enquiry.OrgUnit;

            // Audit
            old.UpdatedOn = DateTime.UtcNow;

            // Auto-close timestamp
            if (old.Status == EnquiryStatus.Resolved || old.Status == EnquiryStatus.Closed)
                old.ClosedOn ??= DateTime.UtcNow;

            await _dbcontext.SaveChangesAsync(ct);
            return old;
        }

        /// <summary>
        /// Soft delete: mark as deleted, keep the record.
        /// </summary>
        public async Task<bool> SoftDeleteAsync(int id, CancellationToken ct = default)
        {
            var enquiry = await _dbcontext.Enquiries.FirstOrDefaultAsync(e => e.EnquiryId == id, ct);
            if (enquiry == null) return false;

            enquiry.IsDeleted = true;
            enquiry.IsActive = false;
            enquiry.UpdatedOn = DateTime.UtcNow;

            await _dbcontext.SaveChangesAsync(ct);
            return true;
        }

        /// <summary>
        /// Hard delete (backward compatibility). Prefer SoftDeleteAsync.
        /// </summary>
        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var enquiry = await _dbcontext.Enquiries.FindAsync(new object[] { id }, ct);
            if (enquiry == null) return false;

            _dbcontext.Enquiries.Remove(enquiry);
            await _dbcontext.SaveChangesAsync(ct);
            return true;
        }
    }
}
