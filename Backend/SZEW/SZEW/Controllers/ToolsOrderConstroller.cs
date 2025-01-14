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
    public class ToolsOrderController : ControllerBase
    {
        private readonly IToolsOrderRepository _toolsOrderRepository;
        private readonly IMapper _mapper;

        public ToolsOrderController(IToolsOrderRepository toolsOrderRepository, IMapper mapper)
        {
            _toolsOrderRepository = toolsOrderRepository;
            _mapper = mapper;
        }

        // GET: api/toolsorder
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<ToolsOrderDto>))]
        public IActionResult GetAllOrders()
        {
            var orders = _toolsOrderRepository.GetAllOrders();
            var ordersDto = _mapper.Map<List<ToolsOrderDto>>(orders);

            return Ok(ordersDto);
        }

        // GET: api/toolsorder/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ToolsOrderDto))]
        [ProducesResponseType(404)]
        public IActionResult GetOrderById(int id)
        {
            var order = _toolsOrderRepository.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }

            var orderDto = _mapper.Map<ToolsOrderDto>(order);
            return Ok(orderDto);
        }

        // POST: api/toolsorder
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ToolsOrderDto))]
        [ProducesResponseType(400)]
        public IActionResult CreateToolsOrder([FromBody] ToolsOrderDto toolsOrderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var toolsOrder = _mapper.Map<ToolsOrder>(toolsOrderDto);

            if (_toolsOrderRepository.AddToolsOrder(toolsOrder))
            {
                return CreatedAtAction("GetOrderById", new { id = toolsOrder.Id }, toolsOrderDto);
            }

            return BadRequest("Could not create the tools order.");
        }

        // GET: api/toolsorder/exists/5
        [HttpGet("exists/{id}")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        public IActionResult ToolsOrderExists(int id)
        {
            if (_toolsOrderRepository.ToolsOrderExists(id))
            {
                return Ok(true);
            }
            return Ok(false);
        }
    }
}
