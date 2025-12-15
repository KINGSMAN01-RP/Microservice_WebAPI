using EnquiryMaster_WebAPI.DTOs.EnquiryDTO;
using EnquiryMaster_WebAPI.Models.EnquiryModel;

namespace EnquiryMaster_WebAPI.Repositories.EnquiryMaster
{
    public interface IEnquiryRepository
    {
        // New: Filtered + paged list for master screen grid
        Task<PagedResult<EnquiryListItemDto>> ListAsync(EnquiryFilterDto filter, CancellationToken ct = default);

        // Detail
        Task<Enquiry?> GetByIdAsync(int id, CancellationToken ct = default);

        // Create/Update
        Task<Enquiry> CreateAsync(Enquiry enquiry, CancellationToken ct = default);
        Task<Enquiry?> UpdateAsync(Enquiry enquiry, CancellationToken ct = default);

        // Soft delete (recommended)
        Task<bool> SoftDeleteAsync(int id, CancellationToken ct = default);

        // Backward compatibility: hard delete (use cautiously)
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);

        // Optional: keep your old method signature for compatibility (returns all)
        Task<IEnumerable<Enquiry>> GetAllAsync(CancellationToken ct = default);
    }
}

