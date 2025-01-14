using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SZEW.DTO;
using SZEW.Interfaces;
using SZEW.Models;
using System.Collections.Generic;

namespace SZEW.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ToolsRequestController : ControllerBase
    {
        private readonly IToolsRequestRepository _toolsRequestRepository;
        private readonly IMapper _mapper;

        public ToolsRequestController(IToolsRequestRepository toolsRequestRepository, IMapper mapper)
        {
            _toolsRequestRepository = toolsRequestRepository;
            _mapper = mapper;
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

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ToolsRequestDto))]
        [ProducesResponseType(400)]
        public IActionResult CreateToolsRequest([FromBody] ToolsRequestDto toolsRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var toolsRequest = _mapper.Map<ToolsRequest>(toolsRequestDto);

            if (!_toolsRequestRepository.AddToolsRequest(toolsRequest))
                return BadRequest("Could not create tools request.");

            return CreatedAtAction("GetRequestById", new { id = toolsRequest.Id }, toolsRequestDto);
        }

        [HttpGet("exists/{id}")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult ToolsRequestExists(int id)
        {
            return Ok(_toolsRequestRepository.ToolsRequestExists(id));
        }
    }
}
