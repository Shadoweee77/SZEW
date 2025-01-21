using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SZEW.DTO;
using SZEW.Interfaces;
using SZEW.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SZEW.Repository;
using System.Security.Claims;

namespace SZEW.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ToolsRequestController : ControllerBase
    {
        private readonly IToolsRequestRepository _toolsRequestRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _requesterRepository;

        public ToolsRequestController(IToolsRequestRepository toolsRequestRepository, IUserRepository reqesterRepository, IMapper mapper)
        {
            this._toolsRequestRepository = toolsRequestRepository;
            this._requesterRepository = reqesterRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<ToolsRequestDto>))]
        public IActionResult GetAllToolsRequests()
        {
            var requests = _toolsRequestRepository.GetAllToolsRequests();
            var requestsDto = _mapper.Map<List<ToolsRequestDto>>(requests);
            return Ok(requestsDto);
        }

        [HttpGet("{toolsRequestId}")]
        [ProducesResponseType(200, Type = typeof(ToolsRequestDto))]
        [ProducesResponseType(404)]
        public IActionResult GetToolsRequestById(int toolsRequestId)
        {
            var request = _toolsRequestRepository.GetToolsRequestById(toolsRequestId);
            if (request == null)
                return NotFound();

            var requestDto = _mapper.Map<ToolsRequestDto>(request);
            return Ok(requestDto);
        }
           
        [HttpGet("{toolsRequestId}/exists")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult ToolsRequestExists(int toolsRequestId)
        {
            return Ok(_toolsRequestRepository.ToolsRequestExists(toolsRequestId));
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateToolsRequest([FromBody] CreateToolsRequestDto requestCreate)
        {
            if (requestCreate == null)
            {
                return BadRequest(ModelState);
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var requesterIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (requesterIdClaim == null)
            {
                return Unauthorized("Requester ID not found in the token.");
            }

            var requesterId = int.Parse(requesterIdClaim.Value);

            var requestMap = _mapper.Map<ToolsRequest>(requestCreate);
            requestMap.Requester = _requesterRepository.GetUserById(requesterId);
            requestMap.Accepted = false;

            try
            {
                var maxId = _toolsRequestRepository.GetAllToolsRequests().Select(v => v.Id).DefaultIfEmpty(0).Max();
                requestMap.Id = maxId + 1;

                if (!_toolsRequestRepository.CreateToolsRequest(requestMap))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgresEx && postgresEx.SqlState == "23505")
            {
                ModelState.AddModelError("", "A Tools request with the same ID already exists");
                return StatusCode(409, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{toolsRequestId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateToolsRequest(int toolsRequestId, [FromBody] UpdateToolsRequestDto updatedRequest)
        {
            if (updatedRequest == null)
                return BadRequest(ModelState);
            if (!_toolsRequestRepository.ToolsRequestExists(toolsRequestId))
                return NotFound();
            var existingRequest = _toolsRequestRepository.GetToolsRequestById(toolsRequestId);
            if (existingRequest == null)
            {
                return NotFound();
            }
            var verifierIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);


            var verifierId = int.Parse(verifierIdClaim.Value);
            existingRequest.VerifierId = verifierId;
            _mapper.Map(updatedRequest, existingRequest);

            if(toolsRequestId != existingRequest.Id)
                return BadRequest(ModelState);
            if (!_toolsRequestRepository.UpdateToolsRequest(existingRequest))
            {
                ModelState.AddModelError("", "Something went wrong verifying the request");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{toolsRequestId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteToolsRequest(int toolsRequestId)
        {
            if (!_toolsRequestRepository.ToolsRequestExists(toolsRequestId))
            {
                return NotFound();
            }

            var requestToDelete = _toolsRequestRepository.GetToolsRequestById(toolsRequestId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_toolsRequestRepository.DeleteToolsRequest(requestToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the request");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
