using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SZEW.DTO;
using SZEW.Interfaces;
using SZEW.Models;

namespace SZEW.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : Controller
    {
        private readonly IVehicleRepository _vehicleRepository;
        //private readonly IMapper _mapper;

        public VehicleController(IVehicleRepository vehicleRepository)
        //public VehicleController(IVehicleRepository vehicleRepository, IMapper mapper)
        {
            this._vehicleRepository = vehicleRepository;
            //this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Vehicle>))]
        public IActionResult GetVehicles()
        {
            var vehicles = _vehicleRepository.GetVehicles();
            //var vehicles = _mapper.Map<List<VehicleDto>>(_vehicleRepository.GetVehicles());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(vehicles);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(Vehicle))]
        [ProducesResponseType(400)]
        public IActionResult GetVehicleByID(int id)
        {
            if (!_vehicleRepository.VehicleExists(id))
            {
                return NotFound();
            }

            var vehicle = _vehicleRepository.GetVehicle(id);
            //var vehicle = _mapper.Map<List<VehicleDto>>(_vehicleRepository.GetVehicle(id));

            if (!ModelState.IsValid)
            {
                return BadRequest($"Vehicle {id} is not valid");
            }

            return Ok(vehicle);
        }

        [HttpGet("{vin}")]
        [ProducesResponseType(200, Type = typeof(Vehicle))]
        [ProducesResponseType(400)]
        public IActionResult GetVehicleByVIN(string vin)
        {
            if (!_vehicleRepository.VehicleExists(vin))
            {
                return NotFound();
            }

            var vehicle = _vehicleRepository.GetVehicle(vin);
            //var vehicle = _mapper.Map<List<VehicleDto>>(_vehicleRepository.GetVehicle(id));

            if (!ModelState.IsValid)
            {
                return BadRequest($"Vehicle {vin} is not valid");
            }

            return Ok(vehicle);
        }

        [HttpGet("{id}/exists")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        public IActionResult VehicleExists(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Vehicle {id} is not valid");
            }

            if (_vehicleRepository.VehicleExists(id))
            {
                return Ok(true);
            }
            else return Ok(false);
        }
    }
}
