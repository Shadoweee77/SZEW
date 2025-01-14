using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SZEW.Interfaces;
using SZEW.Models;
using SZEW.DTO;
using AutoMapper;

namespace SZEW.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ToolController : Controller
    {
        private readonly IToolRepository _toolRepository;
        private readonly IMapper _mapper;

        public ToolController(IToolRepository toolRepository, IMapper mapper)
        {
            _toolRepository = toolRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Tool>))]
        public IActionResult GetTools()
        {
            var tools = _mapper.Map<List<ToolDto>>(_toolRepository.GetTools());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(tools);
        }

        [HttpGet("{id}/exists")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        public IActionResult ToolExists(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Tool {id} is not valid");
            }

            if (_toolRepository.ToolExists(id))
            {
                return Ok(true);
            }
            else return Ok(false);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Tool))]
        [ProducesResponseType(400)]
        public IActionResult GetToolById(int id)
        {
            if (!ModelState.IsValid || !_toolRepository.ToolExists(id))
            {
                return BadRequest($"Tool {id} is not valid");
            }

            var tool = _toolRepository.GetToolById(id);
            return Ok(_mapper.Map<ToolDto>(tool));
        }
    }
}
