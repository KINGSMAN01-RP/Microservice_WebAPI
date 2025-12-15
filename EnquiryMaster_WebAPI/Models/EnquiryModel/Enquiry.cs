using EnquiryMaster_WebAPI.Enums;

namespace EnquiryMaster_WebAPI.Models.EnquiryModel
{

    public class Enquiry
    {
        public int EnquiryId { get; set; }

        public string EnquiryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public EnquiryStatus Status { get; set; } = EnquiryStatus.Open;
        public EnquiryPriority Priority { get; set; } = EnquiryPriority.Medium;

        public string? Category { get; set; }        // e.g., "Loan", "Account", "Support"
        public string? SubCategory { get; set; }     // e.g., "Home Loan", "KYC", ...

        // Source & Channel
        public EnquirySource Source { get; set; } = EnquirySource.Unknown;
        public EnquiryChannel Channel { get; set; } = EnquiryChannel.NotSpecified;
        public string? CampaignCode { get; set; }    // if coming from a marketing campaign
        public string? ReferenceNumber { get; set; } // external ref / ticket / case #

        // Contact info
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        // Assignment
        public int? AssignedToUserId { get; set; }
        public int? AssignedToTeamId { get; set; }

        // Dates & SLA
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedOn { get; set; }
        public DateTime? DueDate { get; set; }       // SLA target date
        public DateTime? ClosedOn { get; set; }

        // Follow-up & progress
        public DateTime? NextFollowUpOn { get; set; }
        public int FollowUpCount { get; set; } = 0;
        public string? ResolutionNotes { get; set; }

        // Flags
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        // Audit
        public int? CreatedByUserId { get; set; }
        public int? UpdatedByUserId { get; set; }

        // Tags / Metadata
        public string? Tags { get; set; }            // comma-separated or JSON
        public string? MetadataJson { get; set; }    // flexible payload

        // Attachments (if you’ll store file refs)
        public int AttachmentCount { get; set; } = 0;

        // Branch / Org context (useful in CBS)
        public int? BranchId { get; set; }
        public string? OrgUnit { get; set; }

    }
}
