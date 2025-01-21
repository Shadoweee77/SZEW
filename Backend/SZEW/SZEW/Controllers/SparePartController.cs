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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SparePartController : Controller
    {
        private readonly ISparePartRepository _sparePartRepository;
        private readonly ISparePartsOrderRepository _sparePartsOrderRepository;
        private readonly IMapper _mapper;

        public SparePartController(ISparePartRepository sparePartRepository, IMapper mapper, ISparePartsOrderRepository sparePartsOrderRepository)
        {
            _sparePartsOrderRepository = sparePartsOrderRepository;
            _sparePartRepository = sparePartRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<SparePart>))]
        public IActionResult GetAllSpareParts()
        {
            var spareParts = _mapper.Map<List<SparePartDto>>(_sparePartRepository.GetAllSpareParts());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(spareParts);
        }

        [HttpGet("{sparePartId:int}")]
        [ProducesResponseType(200, Type = typeof(SparePart))]
        [ProducesResponseType(404)]
        public IActionResult GetSparePartById(int sparePartId)
        {
            if (!_sparePartRepository.SparePartExists(sparePartId))
                return NotFound();

            var sparePart = _mapper.Map<SparePartDto>(_sparePartRepository.GetSparePartById(sparePartId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(sparePart);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateSparePart([FromQuery] int orderId, [FromBody] CreateSparePartDto sparePartCreate)
        {
            if (sparePartCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_sparePartsOrderRepository.SparePartsOrderExists(orderId))
            {
                return BadRequest($"Spare parts order {orderId} is not valid");
            }
            var sparePartMap = _mapper.Map<SparePart>(sparePartCreate);
            sparePartMap.Order = _sparePartsOrderRepository.GetOrderById(orderId);

            try
            {
                var maxId = _sparePartRepository.GetAllSpareParts().Select(v => v.Id).DefaultIfEmpty(0).Max();
                sparePartMap.Id = maxId + 1;

                if (!_sparePartRepository.CreateSparePart(sparePartMap))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgresEx && postgresEx.SqlState == "23505")
            {
                ModelState.AddModelError("", "A spare part with the same ID already exists");
                return StatusCode(409, ModelState);
            }
            return Ok("Successfully created");
        }

        [HttpPut("{sparePartId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateSparePart(int sparePartId, [FromBody] CreateSparePartDto updatedSparePart)
        {
            if (updatedSparePart == null)
                return BadRequest(ModelState);

            if (!_sparePartRepository.SparePartExists(sparePartId))
                return NotFound();

            var sparePart = _sparePartRepository.GetSparePartById(sparePartId);

            if (sparePart == null)
                return NotFound();

            var existingSparePart = _sparePartRepository.GetSparePartById(sparePartId);
            _mapper.Map(updatedSparePart, existingSparePart);

            if (sparePartId != existingSparePart.Id)
                return BadRequest(ModelState);

            if (!_sparePartRepository.UpdateSparePart(existingSparePart))
            {
                ModelState.AddModelError("", "Something went wrong updating the spare part");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{sparePartId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteSparePart(int sparePartId)
        {
            if (!_sparePartRepository.SparePartExists(sparePartId))
                return NotFound();

            var sparePart = _sparePartRepository.GetSparePartById(sparePartId);

            if (!_sparePartRepository.DeleteSparePart(sparePart))
            {
                ModelState.AddModelError("", "Something went wrong deleting the spare part");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        [HttpGet("{sparePartId}/exists")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        public IActionResult SparePartExists(int sparePartId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Spare Part {sparePartId} is not valid");
            }

            return Ok(_sparePartRepository.SparePartExists(sparePartId));
        }
    }
}