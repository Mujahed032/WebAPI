
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReasonWebApi.Dto;
using ReasonWebApi.Interface;
using ReasonWebApi.Models;

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
        public async Task<IActionResult> GetAllReasons()
        {
            try
            {
                var reasons = await _reasonRepository.GetAllReasonsAsync();


                return Ok(reasons);
            }
            catch (Exception ex)
            {
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
                    return NotFound("Reason not found.");
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

                return Ok(reasonDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchReasonByName(string reasonName)
        {
            try
            {
                var reasons = await _reasonRepository.SearchReasonByNameAsync(reasonName);
                if (reasons == null || !reasons.Any())
                {
                    return NotFound("No reasons found.");
                }

                var reasonDtos = reasons.Select(reason => new GetByNameDto
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
                });

                return Ok(reasonDtos);
            }
            catch (Exception ex)
            {
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
