using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SZEW.DTO;
using SZEW.Interfaces;
using SZEW.Models;
using SZEW.Repository;

namespace SZEW.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SparePartsOrderController : Controller
    {
        private readonly ISparePartsOrderRepository _sparePartsOrderRepository;
        private readonly IMapper _mapper;

        public SparePartsOrderController(ISparePartsOrderRepository sparePartsOrderRepository, IMapper mapper)
        {
            _sparePartsOrderRepository = sparePartsOrderRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<SparePartsOrder>))]
        public IActionResult GetAllOrders()
        {
            var orders = _mapper.Map<List<SparePartsOrderDto>>(_sparePartsOrderRepository.GetAllOrders());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(orders);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(SparePartsOrder))]
        public IActionResult GetOrderById(int id)
        {
            var order = _mapper.Map<SparePartsOrderDto>(_sparePartsOrderRepository.GetOrderById(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(order);
        }

        [HttpGet("{id}/exists")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        public IActionResult SparePartsOrderExists(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Spare Parts Order {id} is not valid");
            }

            if (_sparePartsOrderRepository.SparePartsOrderExists(id))
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(SparePartsOrder))]
        [ProducesResponseType(400)]
        public IActionResult CreateSparePartsOrder([FromBody] SparePartsOrderDto sparePartsOrderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sparePartsOrder = _mapper.Map<SparePartsOrder>(sparePartsOrderDto);
            if (_sparePartsOrderRepository.AddSparePartsOrder(sparePartsOrder))
            {
                return CreatedAtAction("GetOrderById", new { id = sparePartsOrder.Id }, sparePartsOrder);
            }

            return BadRequest("Could not create the spare parts order.");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateSparePartsOrder(int id, [FromBody] SparePartsOrderDto sparePartsOrderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sparePartsOrder = _mapper.Map<SparePartsOrder>(sparePartsOrderDto);
            if (_sparePartsOrderRepository.UpdateSparePartsOrder(sparePartsOrder))
            {
                return NoContent();
            }

            return BadRequest("Could not update the spare parts order.");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeleteSparePartsOrder(int id)
        {
            if (_sparePartsOrderRepository.DeleteSparePartsOrder(id))
            {
                return NoContent();
            }

            return BadRequest("Could not delete the spare parts order.");
        }
    }
}
