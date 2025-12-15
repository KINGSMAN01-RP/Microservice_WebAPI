using EnquiryMaster_WebAPI.Enums;

namespace EnquiryMaster_WebAPI.DTOs.EnquiryDTO
{

    public class EnquiryCreateDto
    {
        public string EnquiryName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public EnquiryStatus Status { get; set; } = EnquiryStatus.Open;
        public EnquiryPriority Priority { get; set; } = EnquiryPriority.Medium;
        public string? Category { get; set; }
        public string? SubCategory { get; set; }

        public EnquirySource Source { get; set; } = EnquirySource.Unknown;
        public EnquiryChannel Channel { get; set; } = EnquiryChannel.NotSpecified;
        public string? CampaignCode { get; set; }
        public string? ReferenceNumber { get; set; }

        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public int? AssignedToUserId { get; set; }
        public int? AssignedToTeamId { get; set; }

        public DateTime? DueDate { get; set; }
        public DateTime? NextFollowUpOn { get; set; }

        public string? Tags { get; set; }
        public string? MetadataJson { get; set; }

        public int? BranchId { get; set; }
        public string? OrgUnit { get; set; }
    }

    public class EnquiryUpdateDto
    {
        public string EnquiryName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public EnquiryStatus Status { get; set; } = EnquiryStatus.Open;
        public EnquiryPriority Priority { get; set; } = EnquiryPriority.Medium;

        public string? Category { get; set; }
        public string? SubCategory { get; set; }

        public EnquirySource Source { get; set; } = EnquirySource.Unknown;
        public EnquiryChannel Channel { get; set; } = EnquiryChannel.NotSpecified;
        public string? CampaignCode { get; set; }
        public string? ReferenceNumber { get; set; }

        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public int? AssignedToUserId { get; set; }
        public int? AssignedToTeamId { get; set; }

        public DateTime? DueDate { get; set; }
        public DateTime? NextFollowUpOn { get; set; }

        public string? ResolutionNotes { get; set; }

        public string? Tags { get; set; }
        public string? MetadataJson { get; set; }

        public bool? IsActive { get; set; }

    }


    public class EnquiryListItemDto
    {
        public int EnquiryId { get; set; }
        public string EnquiryName { get; set; } = string.Empty;
        public EnquiryStatus Status { get; set; }
        public EnquiryPriority Priority { get; set; }

        public string? Category { get; set; }
        public string? SubCategory { get; set; }

        public string? CustomerName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public int? AssignedToUserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? NextFollowUpOn { get; set; }

    }


    public class EnquiryDetailDto
    {
        public int EnquiryId { get; set; }
        public string EnquiryName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public EnquiryStatus Status { get; set; }
        public EnquiryPriority Priority { get; set; }

        public string? Category { get; set; }
        public string? SubCategory { get; set; }

        public EnquirySource Source { get; set; }
        public EnquiryChannel Channel { get; set; }
        public string? CampaignCode { get; set; }
        public string? ReferenceNumber { get; set; }

        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public int? AssignedToUserId { get; set; }
        public int? AssignedToTeamId { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ClosedOn { get; set; }
        public DateTime? NextFollowUpOn { get; set; }
        public int FollowUpCount { get; set; }
        public string? ResolutionNotes { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public string? Tags { get; set; }
        public string? MetadataJson { get; set; }

        public int AttachmentCount { get; set; }

        public int? BranchId { get; set; }
        public string? OrgUnit { get; set; }
    }



    public class EnquiryFilterDto
    {
        public string? Text { get; set; }             // searches name/desc/customer/ref
        public EnquiryStatus? Status { get; set; }
        public EnquiryPriority? Priority { get; set; }
        public string? Category { get; set; }
        public string? SubCategory { get; set; }
        public EnquirySource? Source { get; set; }
        public EnquiryChannel? Channel { get; set; }

        public int? AssignedToUserId { get; set; }
        public int? BranchId { get; set; }

        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public DateTime? DueFrom { get; set; }
        public DateTime? DueTo { get; set; }

        public bool IncludeDeleted { get; set; } = false;

        // Paging / Sorting
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 25;
        public string? SortBy { get; set; } = "CreatedOn"; // EnquiryName, Priority, DueDate, etc.
        public bool SortDesc { get; set; } = true;


    }



    public class PagedResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string? Message { get; set; }
        public T? Data { get; set; }
    }



}
