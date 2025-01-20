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
        public IActionResult GetAllRequests()
        {
            var requests = _toolsRequestRepository.GetAllRequests();
            var requestsDto = _mapper.Map<List<ToolsRequestDto>>(requests);
            return Ok(requestsDto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ToolsRequestDto))]
        [ProducesResponseType(404)]
        public IActionResult GetRequestById(int id)
        {
            var request = _toolsRequestRepository.GetRequestById(id);
            if (request == null)
                return NotFound();

            var requestDto = _mapper.Map<ToolsRequestDto>(request);
            return Ok(requestDto);
        }
           
        [HttpGet("{id}/exists")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult ToolsRequestExists(int id)
        {
            return Ok(_toolsRequestRepository.ToolsRequestExists(id));
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
                var maxId = _toolsRequestRepository.GetAllRequests().Select(v => v.Id).DefaultIfEmpty(0).Max();
                requestMap.Id = maxId + 1;

                if (!_toolsRequestRepository.CreateRequest(requestMap))
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

        [HttpPut("{requestId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateRequest(int requestId, [FromBody] UpdateToolsRequestDto updatedRequest)
        {
            if (updatedRequest == null)
                return BadRequest(ModelState);
            if (!_toolsRequestRepository.ToolsRequestExists(requestId))
                return NotFound();
            var existingRequest = _toolsRequestRepository.GetRequestById(requestId);
            if (existingRequest == null)
            {
                return NotFound();
            }
            var verifierIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);


            var verifierId = int.Parse(verifierIdClaim.Value);
            existingRequest.VerifierId = verifierId;
            _mapper.Map(updatedRequest, existingRequest);

            if(requestId != existingRequest.Id)
                return BadRequest(ModelState);
            if (!_toolsRequestRepository.UpdateRequest(existingRequest))
            {
                ModelState.AddModelError("", "Something went wrong verifying the request");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{requestId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRequest(int requestId)
        {
            if (!_toolsRequestRepository.ToolsRequestExists(requestId))
            {
                return NotFound();
            }

            var requestToDelete = _toolsRequestRepository.GetRequestById(requestId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_toolsRequestRepository.DeleteRequest(requestToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the request");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
