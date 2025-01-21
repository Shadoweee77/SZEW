using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Security.Claims;
using SZEW.DTO;
using SZEW.Interfaces;
using SZEW.Models;
using SZEW.Repository;

namespace SZEW.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class ToolsOrderController : Controller
    {
        private readonly IToolsOrderRepository _toolsOrderRepository;
        private readonly IToolRepository _toolRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ToolsOrderController(IToolsOrderRepository toolsOrderRepository, IUserRepository userRepository, IMapper mapper, IToolRepository toolRepository)
        {
            _toolRepository = toolRepository;
            _toolsOrderRepository = toolsOrderRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<ToolsOrderDto>))]
        public IActionResult GetAllToolsOrders()
        {
            var orders = _mapper.Map<List<ToolsOrderDto>>(_toolsOrderRepository.GetAllToolsOrders());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(orders);
        }

        [HttpGet("{toolsOrderId:int}")]
        [ProducesResponseType(200, Type = typeof(ToolsOrderDto))]
        [ProducesResponseType(400)]
        public IActionResult GetToolsOrderById(int toolsOrderId)
        {
            if (!_toolsOrderRepository.ToolsOrderExists(toolsOrderId))
            {
                return NotFound();
            }

            var order = _mapper.Map<ToolsOrderDto>(_toolsOrderRepository.GetToolsOrderById(toolsOrderId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(order);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateTooslOrder([FromBody] CreateToolsOrderDto toolOrderCreate)
        {
            if (toolOrderCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Requester ID not found in the token.");
            }

            var userId = int.Parse(userIdClaim.Value);
            var toolOrderMap = _mapper.Map<ToolsOrder>(toolOrderCreate);
            toolOrderMap.Orderer = _userRepository.GetUserById(userId);

            try
            {
                // Manually set the ID based on the current max ID from the database
                var maxId = _toolsOrderRepository.GetAllToolsOrders().Select(v => v.Id).DefaultIfEmpty(0).Max();
                toolOrderMap.Id = maxId + 1;

                if (!_toolsOrderRepository.CreateToolsOrder(toolOrderMap))
                {
                    ModelState.AddModelError("", "Something went wrong while saving the tool order.");
                    return StatusCode(500, ModelState);
                }
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgresEx && postgresEx.SqlState == "23505")
            {
                ModelState.AddModelError("", "A tool order with the same ID already exists.");
                return StatusCode(409, ModelState);
            }
            return Ok("Successfully created");
        }

        [HttpPut("{toolsOrderId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateToolsOrder(int toolsOrderId, [FromBody] UpdateToolsOrderDto updatedOrder)
        {
            if (updatedOrder == null)
                return BadRequest(ModelState);

            if (!_toolsOrderRepository.ToolsOrderExists(toolsOrderId))
                return NotFound();

            var existingOrder = _toolsOrderRepository.GetToolsOrderById(toolsOrderId);
            if (existingOrder == null)
                return NotFound();

            _mapper.Map(updatedOrder, existingOrder);

            if (!_toolsOrderRepository.UpdateToolsOrder(existingOrder))
            {
                ModelState.AddModelError("", "Something went wrong updating the order");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{toolsOrderId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteToolsOrder(int toolsOrderId)
        {
            if (!_toolsOrderRepository.ToolsOrderExists(toolsOrderId))
            {
                return NotFound();
            }

            var orderToDelete = _toolsOrderRepository.GetToolsOrderById(toolsOrderId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_toolsOrderRepository.DeleteToolsOrder(orderToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the order");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpGet("{toolsOrderId}/exists")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        public IActionResult ToolsOrderExists(int toolsOrderId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Tools order {toolsOrderId} is not valid");
            }

            return Ok(_toolsOrderRepository.ToolsOrderExists(toolsOrderId));
        }
    }
}
