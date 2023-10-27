
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReasonWebApi.Dto;
using ReasonWebApi.Interface;
using ReasonWebApi.Models;
using Serilog;

namespace ReasonWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReasonController : ControllerBase
    {
        private readonly IReasonRepository _reasonRepository;
       

        public ReasonController(IReasonRepository reasonRepository)
        {
            _reasonRepository = reasonRepository;
          
        }




        [HttpGet]
        public async Task<IActionResult> GetAllReasonsAsync(string reasonName = null, int pageNumber = 1, int pageSize = 10 ,string sortBy = "ReasonId", string sortOrder = "asc")
        {
            try
            {
                IEnumerable<Reason> reasons;

                if (!string.IsNullOrEmpty(reasonName))
                {
                    reasons = await _reasonRepository.GetAllReasonsAsync(reasonName, pageNumber, pageSize, sortBy, sortOrder);
                }
                else
                {
                    reasons = await _reasonRepository.GetAllReasonsAsync(pageNumber: pageNumber, pageSize: pageSize, sortBy: sortBy, sortOrder: sortOrder);
                }

                if (!reasons.Any())
                {
                    Log.Warning("No reasons found.");
                    return NotFound("No reasons found.");
                }

                IEnumerable<object> reasonDtos;

                if (!string.IsNullOrEmpty(reasonName))
                {
                    reasonDtos = reasons.Select(r => new GetByNameDto
                    {
                        ReasonId = r.ReasonId,
                        IsPublished = r.IsPublished,
                        PrimaryReason = r.PrimaryReason,
                        ReasonName = r.ReasonName,
                        ReasonCode = r.ReasonCode,
                        ReasonType = r.ReasonType,
                        ThirdPartyNumber = r.ThirdPartyNumber,
                        Description = r.Description,
                        PublishedBy = r.PublishedBy,
                        UpdatedBy = r.UpdatedBy
                    });
                }
                else
                {
                    reasonDtos = reasons.Select(r => new ReasonDto
                    {
                        ReasonId = r.ReasonId,
                        IsPublished = r.IsPublished,
                        PrimaryReason = r.PrimaryReason,
                        ReasonName = r.ReasonName,
                        ReasonCode = r.ReasonCode,
                        ReasonType = r.ReasonType,
                        ThirdPartyNumber = r.ThirdPartyNumber,
                        Description = r.Description,
                        PublishedBy = r.PublishedBy,
                        DatePublished = r.DatePublished,
                        DisplayOnWeb = r.DisplayOnWeb,
                        SortOrder = r.SortOrder,
                        Tag = r.Tag,
                        Comments = r.Comments,
                        IPAddress = r.IPAddress,
                        CreatedBy = r.CreatedBy,
                        DateCreated = r.DateCreated,
                        UpdatedBy = r.UpdatedBy,
                        LastUpdated = r.LastUpdated,
                        IsDeleted = r.IsDeleted,
                        DeletedBy = r.DeletedBy,
                        DateDeleted = r.DateDeleted
                    });
                }

                Log.Information("Retrieved reasons successfully.");
                return Ok(reasonDtos);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Internal server error: {ErrorMessage}", ex.Message);
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }




        [HttpPost]
        public async Task<ActionResult<AddReasonDto>> AddReason([FromBody] AddReasonDto reasonDTO)
        {
            try
            {
                var reason = new Reason

                {
                    IsPublished = reasonDTO.IsPublished,
                    PrimaryReason = reasonDTO.PrimaryReason,
                    ReasonName = reasonDTO.ReasonName,
                    ReasonCode = reasonDTO.ReasonCode,
                    ReasonType = reasonDTO.ReasonType,
                    ThirdPartyNumber = reasonDTO.ThirdPartyNumber,
                    Description = reasonDTO.Description,
                    PublishedBy = reasonDTO.PublishedBy,
                    CreatedBy = reasonDTO.CreatedBy
                    
                };

                await _reasonRepository.AddReasonAsync(reason);

                return Ok(reasonDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }



        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReason(int id, [FromBody] UpdateReasonDto reasonDTO)
        {
            try
            {
                var existingReason = await _reasonRepository.GetReasonByIdAsync(id);
                if (existingReason == null)
                {
                    return NotFound();
                }

                var reason = new Reason
                {
                    ReasonId = id,
                    IsPublished = reasonDTO.IsPublished,
                    PrimaryReason = reasonDTO.PrimaryReason,
                    ReasonName = reasonDTO.ReasonName,
                    ReasonCode = reasonDTO.ReasonCode,
                    ReasonType = reasonDTO.ReasonType,
                    ThirdPartyNumber = reasonDTO.ThirdPartyNumber,
                    Description = reasonDTO.Description,
                    PublishedBy = reasonDTO.PublishedBy,
                    UpdatedBy = reasonDTO.UpdatedBy
                };

                await _reasonRepository.UpdateReasonAsync(id,reason);

                return Ok(reasonDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> GetReasonById(int id)
        {
            try
            {
                var reason = await _reasonRepository.GetReasonByIdAsync(id);
                if (reason == null || reason.ReasonId == 0)
                {
                    Log.Information($"No reason found with ID: {id}");
                    return NotFound("Reason not found.");
                }
                else if (reason.ReasonName == "Record has been deleted.")
                {
                    Log.Information($"Record with ID: {id} has been deleted.");
                    return StatusCode(410, "Record has been deleted.");
                }

                var reasonDto = new GetByIdDto
                {
                    ReasonId = reason.ReasonId,
                    IsPublished = reason.IsPublished,
                    PrimaryReason = reason.PrimaryReason,
                    ReasonName = reason.ReasonName,
                    ReasonCode = reason.ReasonCode,
                    ReasonType = reason.ReasonType,
                    ThirdPartyNumber = reason.ThirdPartyNumber,
                    Description = reason.Description,
                    PublishedBy = reason.PublishedBy,
                    UpdatedBy = reason.UpdatedBy
                };

                Log.Information($"Retrieved reason with ID: {id}");
                return Ok(reasonDto);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error occurred in GetReasonById for ID: {id}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReason(int id, [FromQuery] string deletedBy)
        {
            try
            {
                var isDeleted = await _reasonRepository.DeleteReasonAsync(id, deletedBy);
                if (!isDeleted)
                {
                    return NotFound("Reason not found.");
                }
                return Ok("Reason deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

    }
}
