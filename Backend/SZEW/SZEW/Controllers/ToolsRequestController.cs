using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SZEW.DTO;
using SZEW.Interfaces;
using SZEW.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SZEW.Repository;

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
        private readonly IUserRepository _verifierRepository;

        public ToolsRequestController(IToolsRequestRepository toolsRequestRepository, IUserRepository reqesterRepository, IUserRepository verifierRepository, IMapper mapper)
        {
            this._toolsRequestRepository = toolsRequestRepository;
            this._requesterRepository = reqesterRepository;
            this._verifierRepository = verifierRepository;
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
           
        [HttpGet("exists/{id}")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult ToolsRequestExists(int id)
        {
            return Ok(_toolsRequestRepository.ToolsRequestExists(id));
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateToolsRequest([FromQuery] int RequesterId, [FromBody] CreateToolsRequestDto requestCreate)
        {
            if (requestCreate == null)
            {
                return BadRequest(ModelState);
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var requestMap = _mapper.Map<ToolsRequest>(requestCreate);
            requestMap.Requester = _requesterRepository.GetUserById(RequesterId);
            requestMap.Accepted = false;

            try
            {
                var maxId = _toolsRequestRepository.GetAllRequests().Max(v => v.Id);
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

        [HttpPut("verify/{requestId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult VerifyRequest(int requestId, [FromQuery] int verifierId)
        {
            var existingRequest = _toolsRequestRepository.GetRequestById(requestId);

            if (existingRequest == null)
            {
                return NotFound();
            }

            existingRequest.VerifierId = verifierId;
            existingRequest.Accepted = true;

            if (!_toolsRequestRepository.VerifyRequest(existingRequest))
            {
                ModelState.AddModelError("", "Something went wrong verifying the request");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{vehicleId}")]
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
