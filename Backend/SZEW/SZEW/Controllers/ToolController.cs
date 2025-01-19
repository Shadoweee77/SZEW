using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SZEW.DTO;
using SZEW.Interfaces;
using SZEW.Models;
using SZEW.Repository;

namespace SZEW.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class ToolController : Controller
    {
        private readonly IToolRepository _toolRepository;
        private readonly IToolsOrderRepository _toolOrderRepository;
        private readonly IMapper _mapper;

        public ToolController(IToolRepository toolRepository, IMapper mapper, IToolsOrderRepository toolOrderRepository)
        {
            _toolOrderRepository = toolOrderRepository;
            _toolRepository = toolRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Tool>))]
        public IActionResult GetTools()
        {
            var tools = _mapper.Map<List<ToolDto>>(_toolRepository.GetTools());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(tools);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(Tool))]
        [ProducesResponseType(404)]
        public IActionResult GetToolById(int id)
        {
            if (!_toolRepository.ToolExists(id))
                return NotFound();

            var tool = _mapper.Map<ToolDto>(_toolRepository.GetToolById(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(tool);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateTool([FromQuery] int orderId, [FromBody] CreateToolDto toolCreate)
        {
            if (toolCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var toolMap = _mapper.Map<Tool>(toolCreate);
            toolMap.Order = _toolOrderRepository.GetOrderById(orderId);

            try
            {
                // Manually set the ID based on the current max ID from the database
                var maxId = _toolRepository.GetTools().Select(v => v.Id).DefaultIfEmpty(0).Max();
                toolMap.Id = maxId + 1;

                if (!_toolRepository.CreateTool(toolMap))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgresEx && postgresEx.SqlState == "23505")
            {
                ModelState.AddModelError("", "A tool with the same ID already exists");
                return StatusCode(409, ModelState);
            }
            return Ok("Successfully created");
        }

        [HttpPut("{toolId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateTool(int toolId, [FromBody] CreateToolDto updatedTool)
        {
            if (updatedTool == null)
                return BadRequest(ModelState);

            if (!_toolRepository.ToolExists(toolId))
                return NotFound();

            var tool = _toolRepository.GetToolById(toolId);

            if (tool == null)
                return NotFound();

            var existingTool = _toolRepository.GetToolById(toolId);
            _mapper.Map(updatedTool, existingTool);

            if (toolId != existingTool.Id)
                return BadRequest(ModelState);

            if (!_toolRepository.UpdateTool(existingTool))
            {
                ModelState.AddModelError("", "Something went wrong updating the tool");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{toolId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTool(int toolId)
        {
            if (!_toolRepository.ToolExists(toolId))
                return NotFound();

            var tool = _toolRepository.GetToolById(toolId);

            if (!_toolRepository.DeleteTool(tool))
            {
                ModelState.AddModelError("", "Something went wrong deleting the tool");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
