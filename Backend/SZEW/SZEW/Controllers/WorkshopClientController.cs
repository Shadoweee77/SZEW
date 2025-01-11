using Microsoft.AspNetCore.Mvc;
using SZEW.Interfaces;
using SZEW.Models;
using SZEW.Repository;
using AutoMapper;
using SZEW.DTO;

namespace SZEW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkshopClientController : Controller
    {
        private readonly IWorkshopClientRepository _workshopClientRepository;
        private readonly IMapper _mapper;
        public WorkshopClientController(IWorkshopClientRepository workshopClientRepository, IMapper mapper)
        {
            this._workshopClientRepository = workshopClientRepository;
            _mapper = mapper;

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<WorkshopClient>))]
        public IActionResult GetClients()
        {
            var clients = _mapper.Map<List<WorkshopClientDto>>(_workshopClientRepository.GetClients());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(clients);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(WorkshopClient))]
        [ProducesResponseType(400)]
        public IActionResult GetClient(int id)
        {
            if (!_workshopClientRepository.ClientExists(id))
            {
                return NotFound();
            }

            var client = _mapper.Map<WorkshopClientDto>(_workshopClientRepository.GetClient(id));

            if (!ModelState.IsValid)
            {
                return BadRequest($"Workshop Client {id} is not valid");
            }

            return Ok(client);
        }

        [HttpGet("{id}/vehicles")]
        [ProducesResponseType(200, Type = typeof(ICollection<Vehicle>))]
        [ProducesResponseType(400)]
        public IActionResult GetVehicles(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Workshop Client {id} is not valid");
            }

            return Ok(_workshopClientRepository.GetVehicles(id));
        }


        [HttpGet("{id}/type")]
        [ProducesResponseType(200, Type = typeof(ClientType))]
        [ProducesResponseType(400)]
        public IActionResult GetClientType(int id)
        {
            if (!ModelState.IsValid || !_workshopClientRepository.ClientExists(id))
            {
                return BadRequest($"Workshop Client {id} is not valid");
            }

            return Ok(_workshopClientRepository.GetClientType(id));
        }

        [HttpGet("{id}/exists")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        public IActionResult ClientExists(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Workshop Client {id} is not valid");
            }

            if (_workshopClientRepository.ClientExists(id))
            {
                return Ok(true);
            }
            else return Ok(false);
        }
    }
}
