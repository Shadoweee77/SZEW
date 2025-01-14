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
        private readonly IWorkshopClientRepository _ownerRepository;
        private readonly IMapper _mapper;

        public VehicleController(IVehicleRepository vehicleRepository, IWorkshopClientRepository _ownerRepository, IMapper mapper)
        {
            this._vehicleRepository = vehicleRepository;
            this._ownerRepository = _ownerRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Vehicle>))]
        public IActionResult GetVehicles()
        {
            var vehicles = _mapper.Map<List<VehicleDto>>(_vehicleRepository.GetVehicles());

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

            var vehicle = _mapper.Map<VehicleDto>(_vehicleRepository.GetVehicle(id));

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

            var vehicle = _mapper.Map<VehicleDto>(_vehicleRepository.GetVehicle(vin));

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
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        
        public IActionResult CreateVehicle([FromQuery] int OwnerId,[FromBody] CreateVehicleDto vehicleCreate)
        {
            if (vehicleCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!string.IsNullOrWhiteSpace(vehicleCreate.VIN))
            {
                var existingVehicleByVIN = _vehicleRepository.GetVehicles()
                    .FirstOrDefault(v =>
                    !string.IsNullOrWhiteSpace(vehicleCreate.VIN) &&
                    v.VIN != null &&
                    v.VIN.Trim().ToUpper() == vehicleCreate.VIN.Trim().ToUpper());


                if (existingVehicleByVIN != null)
                {
                    ModelState.AddModelError("", "Vehicle with this VIN already exists");
                    return StatusCode(422, ModelState);
                }
            }

            // Check for duplicate vehicle based on Registration Number (if provided)
            if (!string.IsNullOrWhiteSpace(vehicleCreate.RegistrationNumber))
            {
                var existingVehicleByReg = _vehicleRepository.GetVehicles()
                    .FirstOrDefault(v =>
                    !string.IsNullOrWhiteSpace(vehicleCreate.RegistrationNumber) &&
                    v.RegistrationNumber != null &&
                    v.RegistrationNumber.Trim().ToUpper() == vehicleCreate.RegistrationNumber.Trim().ToUpper());

                if (existingVehicleByReg != null)
                {
                    ModelState.AddModelError("", "Vehicle with this Registration Number already exists");
                    return StatusCode(422, ModelState);
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicleMap = _mapper.Map<Vehicle>(vehicleCreate);
            vehicleMap.Owner = _ownerRepository.GetClient(OwnerId);

            if (!_vehicleRepository.CreateVehicle(vehicleMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
        [HttpPut("{vehicleId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateVehicle(int vehicleId, [FromBody] VehicleDto updatedVehicle)
        {
            if (updatedVehicle == null)
                return BadRequest(ModelState);

            if (vehicleId != updatedVehicle.Id)
                return BadRequest(ModelState);

            if (!_vehicleRepository.VehicleExists(vehicleId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var vehicleMap = _mapper.Map<Vehicle>(updatedVehicle);

            if (!_vehicleRepository.UpdateVehicle(vehicleMap))
            {
                ModelState.AddModelError("", "Something went wrong updating vehicle");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        [HttpDelete("{vehicleId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteVehicle(int vehicleId)
        {
            if (!_vehicleRepository.VehicleExists(vehicleId))
            {
                return NotFound();
            }

            var vehicleToDelete = _vehicleRepository.GetVehicle(vehicleId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_vehicleRepository.DeleteVehicle(vehicleToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
            }

            return NoContent();
        }
    }
}
