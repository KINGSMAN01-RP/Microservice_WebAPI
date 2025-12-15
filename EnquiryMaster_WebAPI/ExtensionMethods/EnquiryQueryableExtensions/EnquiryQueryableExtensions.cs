using EnquiryMaster_WebAPI.DTOs.EnquiryDTO;
using EnquiryMaster_WebAPI.Models.EnquiryModel;

namespace EnquiryMaster_WebAPI.ExtensionMethods.EnquiryQueryableExtensions
{
    public static class EnquiryQueryableExtensions
    {
        /// <summary>
        /// Applies filtering based on EnquiryFilterDto.
        /// Keeps IQueryable so EF can translate to SQL.
        /// </summary>
        public static IQueryable<Enquiry> ApplyFilter(this IQueryable<Enquiry> q, EnquiryFilterDto f)
        {
            if (f == null) return q;

            if (!f.IncludeDeleted)
                q = q.Where(e => !e.IsDeleted);

            if (!string.IsNullOrWhiteSpace(f.Text))
            {
                var text = f.Text.Trim();
                q = q.Where(e =>
                    e.EnquiryName.Contains(text) ||
                    (e.Description != null && e.Description.Contains(text)) ||
                    (e.CustomerName != null && e.CustomerName.Contains(text)) ||
                    (e.ReferenceNumber != null && e.ReferenceNumber.Contains(text)));
            }

            if (f.Status.HasValue) q = q.Where(e => e.Status == f.Status);
            if (f.Priority.HasValue) q = q.Where(e => e.Priority == f.Priority);
            if (!string.IsNullOrEmpty(f.Category)) q = q.Where(e => e.Category == f.Category);
            if (!string.IsNullOrEmpty(f.SubCategory)) q = q.Where(e => e.SubCategory == f.SubCategory);
            if (f.Source.HasValue) q = q.Where(e => e.Source == f.Source);
            if (f.Channel.HasValue) q = q.Where(e => e.Channel == f.Channel);
            if (f.AssignedToUserId.HasValue) q = q.Where(e => e.AssignedToUserId == f.AssignedToUserId);
            if (f.BranchId.HasValue) q = q.Where(e => e.BranchId == f.BranchId);

            if (f.CreatedFrom.HasValue) q = q.Where(e => e.CreatedOn >= f.CreatedFrom.Value);
            if (f.CreatedTo.HasValue) q = q.Where(e => e.CreatedOn <= f.CreatedTo.Value);
            if (f.DueFrom.HasValue) q = q.Where(e => e.DueDate >= f.DueFrom.Value);
            if (f.DueTo.HasValue) q = q.Where(e => e.DueDate <= f.DueTo.Value);

            return q;
        }

        /// <summary>
        /// Applies sorting based on SortBy and SortDesc.
        /// Default sort is CreatedOn desc.
        /// </summary>
        public static IQueryable<Enquiry> ApplySorting(this IQueryable<Enquiry> q, EnquiryFilterDto f)
        {
            var sortBy = f?.SortBy?.ToLowerInvariant();

            return sortBy switch
            {
                "enquiryname" => f!.SortDesc ? q.OrderByDescending(e => e.EnquiryName) : q.OrderBy(e => e.EnquiryName),
                "priority" => f!.SortDesc ? q.OrderByDescending(e => e.Priority) : q.OrderBy(e => e.Priority),
                "status" => f!.SortDesc ? q.OrderByDescending(e => e.Status) : q.OrderBy(e => e.Status),
                "duedate" => f!.SortDesc ? q.OrderByDescending(e => e.DueDate) : q.OrderBy(e => e.DueDate),
                "nextfollowupon" => f!.SortDesc ? q.OrderByDescending(e => e.NextFollowUpOn) : q.OrderBy(e => e.NextFollowUpOn),
                "createdon" => f!.SortDesc ? q.OrderByDescending(e => e.CreatedOn) : q.OrderBy(e => e.CreatedOn),
                _ => f!.SortDesc ? q.OrderByDescending(e => e.CreatedOn) : q.OrderBy(e => e.CreatedOn)
            };
        }

        /// <summary>
        /// Applies paging using Page and PageSize.
        /// Guards negative values.
        /// </summary>
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> q, int page, int pageSize)
        {
            var safePage = page <= 0 ? 1 : page;
            var safeSize = pageSize <= 0 ? 25 : pageSize;

            return q.Skip((safePage - 1) * safeSize).Take(safeSize);
        }
    }
}

