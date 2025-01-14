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
    public class SparePartController : Controller
    {
        private readonly ISparePartRepository _sparePartRepository;
        private readonly IMapper _mapper;

        public SparePartController(ISparePartRepository sparePartRepository, IMapper mapper)
        {
            this._sparePartRepository = sparePartRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<SparePart>))]
        public IActionResult GetAllSpareParts()
        {
            var spareParts = _mapper.Map<List<SparePartDto>>(_sparePartRepository.GetAllSpareParts());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(spareParts);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(SparePart))]
        public IActionResult GetSparePartById(int id)
        {
            var sparePart = _mapper.Map<SparePartDto>(_sparePartRepository.GetSparePartById(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(sparePart);
        }

        [HttpGet("{id}/exists")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        public IActionResult SparePartExists(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Spare part {id} is not valid");
            }

            if (_sparePartRepository.SparePartExists(id))
            {
                return Ok(true);
            }
            else return Ok(false);
        }
    }
}
