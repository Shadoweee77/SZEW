using Microsoft.AspNetCore.Mvc;
using SZEW.Interfaces;
using SZEW.Models;
using SZEW.Repository;
using AutoMapper;
using SZEW.DTO;
using Microsoft.AspNetCore.Authorization;

namespace SZEW.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkshopClientController : Controller
    {
        private readonly IWorkshopClientRepository _workshopClientRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;

        public WorkshopClientController(IWorkshopClientRepository workshopClientRepository, IVehicleRepository vehicleRepository, IMapper mapper)
        {
            this._workshopClientRepository = workshopClientRepository;
            this._vehicleRepository = vehicleRepository;
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

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(WorkshopClient))]
        [ProducesResponseType(400)]
        public IActionResult CreateClient([FromBody] WorkshopClientDto clientDto)
        {
            if (clientDto == null)
            {
                return BadRequest(ModelState);
            }

            WorkshopClient clientToCreate;

            if (clientDto.ClientType == ClientType.Individual)
            {
                if (string.IsNullOrWhiteSpace(clientDto.Name) || string.IsNullOrWhiteSpace(clientDto.Surname))
                {
                    ModelState.AddModelError("", "Name and Surname are required for individual clients.");
                    return BadRequest(ModelState);
                }

                var individualClient = new WorkshopIndividualClient
                {
                    Id = clientDto.Id,
                    Email = clientDto.Email,
                    Address = clientDto.Address,
                    PhoneNumber = clientDto.PhoneNumber,
                    ClientType = ClientType.Individual,
                    Name = clientDto.Name,
                    Surname = clientDto.Surname,
                    Vehicles = new List<Vehicle>()
                };

                foreach (var vehicleId in clientDto.VehicleIds)
                {
                    var vehicle = _vehicleRepository.GetVehicle(vehicleId);
                    if (vehicle != null)
                    {
                        individualClient.Vehicles.Add(vehicle);
                    }
                }

                clientToCreate = individualClient;
            }
            else if (clientDto.ClientType == ClientType.Business)
            {
                if (string.IsNullOrWhiteSpace(clientDto.NIP) || string.IsNullOrWhiteSpace(clientDto.BusinessName))
                {
                    ModelState.AddModelError("", "NIP and Business Name are required for business clients.");
                    return BadRequest(ModelState);
                }

                var businessClient = new WorkshopBusinessClient
                {
                    Id = clientDto.Id,
                    Email = clientDto.Email,
                    Address = clientDto.Address,
                    PhoneNumber = clientDto.PhoneNumber,
                    ClientType = ClientType.Business,
                    NIP = clientDto.NIP,
                    BusinessName = clientDto.BusinessName,
                    Vehicles = new List<Vehicle>()
                };

                foreach (var vehicleId in clientDto.VehicleIds)
                {
                    var vehicle = _vehicleRepository.GetVehicle(vehicleId);
                    if (vehicle != null)
                    {
                        businessClient.Vehicles.Add(vehicle);
                    }
                }

                clientToCreate = businessClient;
            }
            else
            {
                return BadRequest("Invalid client type");
            }

            bool isCreated = _workshopClientRepository.CreateWorkshopClient(clientToCreate);

            if (!isCreated)
            {
                ModelState.AddModelError("", "Something went wrong while saving the client");
                return StatusCode(500, ModelState);
            }

            return CreatedAtAction(nameof(GetClient), new { id = clientToCreate.Id }, clientToCreate);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateClient(int id, [FromBody] WorkshopClientDto clientDto)
        {
            if (clientDto == null || id != clientDto.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_workshopClientRepository.ClientExists(id))
            {
                return NotFound();
            }

            var existingClient = _workshopClientRepository.GetClient(id);

            if (existingClient == null)
            {
                return NotFound();
            }

            if (clientDto.ClientType == ClientType.Individual)
            {
                if (string.IsNullOrWhiteSpace(clientDto.Name) || string.IsNullOrWhiteSpace(clientDto.Surname))
                {
                    ModelState.AddModelError("", "Name and Surname are required for individual clients.");
                    return BadRequest(ModelState);
                }

                var individualClient = existingClient as WorkshopIndividualClient;
                if (individualClient != null)
                {
                    individualClient.Name = clientDto.Name;
                    individualClient.Surname = clientDto.Surname;
                }
            }
            else if (clientDto.ClientType == ClientType.Business)
            {
                if (string.IsNullOrWhiteSpace(clientDto.NIP) || string.IsNullOrWhiteSpace(clientDto.BusinessName))
                {
                    ModelState.AddModelError("", "NIP and Business Name are required for business clients.");
                    return BadRequest(ModelState);
                }

                var businessClient = existingClient as WorkshopBusinessClient;
                if (businessClient != null)
                {
                    businessClient.NIP = clientDto.NIP;
                    businessClient.BusinessName = clientDto.BusinessName;
                }
            }
            else
            {
                return BadRequest("Invalid client type");
            }

            existingClient.Email = clientDto.Email;
            existingClient.Address = clientDto.Address;
            existingClient.PhoneNumber = clientDto.PhoneNumber;

            existingClient.Vehicles.Clear();
            foreach (var vehicleId in clientDto.VehicleIds)
            {
                var vehicle = _vehicleRepository.GetVehicle(vehicleId);
                if (vehicle != null)
                {
                    existingClient.Vehicles.Add(vehicle);
                }
            }

            bool isUpdated = _workshopClientRepository.UpdateWorkshopClient(existingClient);

            if (!isUpdated)
            {
                ModelState.AddModelError("", "Something went wrong while updating the client");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteClient(int id)
        {
            if (!_workshopClientRepository.ClientExists(id))
            {
                return NotFound();
            }

            var clientToDelete = _workshopClientRepository.GetClient(id);

            if (clientToDelete == null)
            {
                return NotFound();
            }

            bool isDeleted = _workshopClientRepository.DeleteWorkshopClient(clientToDelete);

            if (!isDeleted)
            {
                ModelState.AddModelError("", "Something went wrong while deleting the client");
                return StatusCode(500, ModelState);
            }

            return NoContent();
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

            return Ok(_workshopClientRepository.ClientExists(id));
        }
    }
}
