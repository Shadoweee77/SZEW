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
        public IActionResult GetToolsOrders()
        {
            var orders = _mapper.Map<List<ToolsOrderDto>>(_toolsOrderRepository.GetAllOrders());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(ToolsOrderDto))]
        [ProducesResponseType(400)]
        public IActionResult GetToolsOrderById(int id)
        {
            if (!_toolsOrderRepository.ToolsOrderExists(id))
            {
                return NotFound();
            }

            var order = _mapper.Map<ToolsOrderDto>(_toolsOrderRepository.GetOrderById(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(order);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateToolOrder([FromQuery] int userId, [FromBody] CreateToolsOrderDto toolOrderCreate)
        {
            if (toolOrderCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var toolOrderMap = _mapper.Map<ToolsOrder>(toolOrderCreate);
            toolOrderMap.Orderer = _userRepository.GetUserById(userId);

            try
            {
                // Manually set the ID based on the current max ID from the database
                var maxId = _toolsOrderRepository.GetAllOrders().Select(v => v.Id).DefaultIfEmpty(0).Max();
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


        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateToolsOrder(int id, [FromBody] UpdateToolsOrderDto updatedOrder)
        {
            if (updatedOrder == null)
                return BadRequest(ModelState);

            if (!_toolsOrderRepository.ToolsOrderExists(id))
                return NotFound();

            var existingOrder = _toolsOrderRepository.GetOrderById(id);
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

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteToolsOrder(int id)
        {
            if (!_toolsOrderRepository.ToolsOrderExists(id))
            {
                return NotFound();
            }

            var orderToDelete = _toolsOrderRepository.GetOrderById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_toolsOrderRepository.DeleteToolsOrder(orderToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the order");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
