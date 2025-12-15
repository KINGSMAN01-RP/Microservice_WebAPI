
using EnquiryMaster_WebAPI.DTOs.EnquiryDTO;
using EnquiryMaster_WebAPI.Enums;
using EnquiryMaster_WebAPI.Models.EnquiryModel;
using EnquiryMaster_WebAPI.Repositories.EnquiryMaster;
using Microsoft.AspNetCore.Mvc;

namespace EnquiryMaster_WebAPI.Controllers
{
    [ApiController]
    [Route("api/enquiry")]
    public class EnquiryController : ControllerBase
    {
        private readonly IEnquiryRepository _repository;


        public EnquiryController(IEnquiryRepository repository)
        {
            _repository = repository;
        }

        // GET: api/enquiry (grid list with filters/paging/sorting)
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<EnquiryListItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List([FromQuery] EnquiryFilterDto filter, CancellationToken ct)
        {
            var result = await _repository.ListAsync(filter, ct);
            return Ok(result);
        }

        // GET: api/enquiry/{id}
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<EnquiryDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct);
            if (entity == null)
                return NotFound(new ApiResponse<string> { Success = false, Message = "Enquiry not found" });

            var dto = MapToDetailDto(entity);
            return Ok(new ApiResponse<EnquiryDetailDto> { Data = dto });
        }

        // POST: api/enquiry
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<EnquiryDetailDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] EnquiryCreateDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = MapFromCreateDto(dto);

            // Server-side defaults (also set in repo)
            entity.Status = EnquiryStatus.Open;
            entity.CreatedOn = DateTime.UtcNow;
            entity.IsActive = true;
            entity.IsDeleted = false;

            var created = await _repository.CreateAsync(entity, ct);
            var resultDto = MapToDetailDto(created);

            return CreatedAtAction(nameof(Get), new { id = created.EnquiryId }, new ApiResponse<EnquiryDetailDto> { Data = resultDto });
        }

        // PUT: api/enquiry/{id}
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<EnquiryDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] EnquiryUpdateDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _repository.GetByIdAsync(id, ct);
            if (existing == null)
                return NotFound(new ApiResponse<string> { Success = false, Message = "Enquiry not found" });

            // Map DTO onto existing entity
            ApplyUpdateDto(existing, dto);

            // Repo will set UpdatedOn and ClosedOn conditionally
            var updated = await _repository.UpdateAsync(existing, ct);

            var resultDto = MapToDetailDto(updated!);
            return Ok(new ApiResponse<EnquiryDetailDto> { Data = resultDto });
        }

        // DELETE (soft): api/enquiry/{id}
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var success = await _repository.SoftDeleteAsync(id, ct);
            if (!success)
                return NotFound(new ApiResponse<string> { Success = false, Message = "Enquiry not found or already deleted" });

            return Ok(new ApiResponse<bool> { Data = true, Message = "Deleted successfully" });
        }

        // DELETE (hard): api/enquiry/{id}/hard
        // Optional: use only for admin or cleanup operations.
        [HttpDelete("{id:int}/hard")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> HardDelete(int id, CancellationToken ct)
        {
            var success = await _repository.DeleteAsync(id, ct);
            if (!success)
                return NotFound(new ApiResponse<string> { Success = false, Message = "Enquiry not found" });

            return Ok(new ApiResponse<bool> { Data = true, Message = "Hard deleted successfully" });
        }

        #region Mapping helpers (manual).

        private Enquiry MapFromCreateDto(EnquiryCreateDto dto)
        {
            
            return new Enquiry
            {
                EnquiryName = dto.EnquiryName,
                Description = dto.Description,
                Priority = dto.Priority,
                Category = dto.Category,
                SubCategory = dto.SubCategory,
                Source = dto.Source,
                Channel = dto.Channel,
                CampaignCode = dto.CampaignCode,
                ReferenceNumber = dto.ReferenceNumber,
                CustomerName = dto.CustomerName,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                AssignedToUserId = dto.AssignedToUserId,
                AssignedToTeamId = dto.AssignedToTeamId,
                DueDate = dto.DueDate,
                NextFollowUpOn = dto.NextFollowUpOn,
                Tags = dto.Tags,
                MetadataJson = dto.MetadataJson,
                BranchId = dto.BranchId,
                OrgUnit = dto.OrgUnit
            };
        }

        private void ApplyUpdateDto(Enquiry existing, EnquiryUpdateDto dto)
        {
           
            existing.EnquiryName = dto.EnquiryName;
            existing.Description = dto.Description;
            existing.Status = dto.Status;
            existing.Priority = dto.Priority;
            existing.Category = dto.Category;
            existing.SubCategory = dto.SubCategory;
            existing.Source = dto.Source;
            existing.Channel = dto.Channel;
            existing.CampaignCode = dto.CampaignCode;
            existing.ReferenceNumber = dto.ReferenceNumber;
            existing.CustomerName = dto.CustomerName;
            existing.Email = dto.Email;
            existing.Phone = dto.Phone;
            existing.Address = dto.Address;
            existing.AssignedToUserId = dto.AssignedToUserId;
            existing.AssignedToTeamId = dto.AssignedToTeamId;
            existing.DueDate = dto.DueDate;
            existing.NextFollowUpOn = dto.NextFollowUpOn;
            existing.ResolutionNotes = dto.ResolutionNotes;
            existing.Tags = dto.Tags;
            existing.MetadataJson = dto.MetadataJson;
            if (dto.IsActive.HasValue) existing.IsActive = dto.IsActive.Value;
           
        }

        private EnquiryDetailDto MapToDetailDto(Enquiry e)
        {
            
            return new EnquiryDetailDto
            {
                EnquiryId = e.EnquiryId,
                EnquiryName = e.EnquiryName,
                Description = e.Description,
                Status = e.Status,
                Priority = e.Priority,
                Category = e.Category,
                SubCategory = e.SubCategory,
                Source = e.Source,
                Channel = e.Channel,
                CampaignCode = e.CampaignCode,
                ReferenceNumber = e.ReferenceNumber,
                CustomerName = e.CustomerName,
                Email = e.Email,
                Phone = e.Phone,
                Address = e.Address,
                AssignedToUserId = e.AssignedToUserId,
                AssignedToTeamId = e.AssignedToTeamId,
                CreatedOn = e.CreatedOn,
                UpdatedOn = e.UpdatedOn,
                DueDate = e.DueDate,
                ClosedOn = e.ClosedOn,
                NextFollowUpOn = e.NextFollowUpOn,
                FollowUpCount = e.FollowUpCount,
                ResolutionNotes = e.ResolutionNotes,
                IsActive = e.IsActive,
                IsDeleted = e.IsDeleted,
                Tags = e.Tags,
                MetadataJson = e.MetadataJson,
                AttachmentCount = e.AttachmentCount,
                BranchId = e.BranchId,
                OrgUnit = e.OrgUnit
            };
        }

        #endregion
    }
}

