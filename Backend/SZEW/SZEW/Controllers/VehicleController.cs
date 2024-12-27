using Microsoft.AspNetCore.Mvc;
using SZEW.Interfaces;
using SZEW.Models;

namespace SZEW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : Controller
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleController(IVehicleRepository vehicleRepository)
        {
            this._vehicleRepository = vehicleRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Vehicle>))]
        public IActionResult GetVehicles()
        {
            var vehicles = _vehicleRepository.GetVehicles();

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(vehicles);
        }
    }
}
