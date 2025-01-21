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
        public IActionResult GetAllWorkshopClients()
        {
            var clients = _mapper.Map<List<WorkshopClientDto>>(_workshopClientRepository.GetAllWorkshopClients());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(clients);
        }

        [HttpGet("{workshopClientId:int}")]
        [ProducesResponseType(200, Type = typeof(WorkshopClient))]
        [ProducesResponseType(400)]
        public IActionResult GetWorkshopClientById(int workshopClientId)
        {
            if (!_workshopClientRepository.WorkshopClientExists(workshopClientId))
            {
                return NotFound();
            }

            var client = _mapper.Map<WorkshopClientDto>(_workshopClientRepository.GetWorkshopClientById(workshopClientId));

            if (!ModelState.IsValid)
            {
                return BadRequest($"Workshop Client {workshopClientId} is not valid");
            }

            return Ok(client);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(WorkshopClient))]
        [ProducesResponseType(400)]
        public IActionResult CreateWorkshopClient([FromBody] WorkshopClientDto clientDto)
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
                    var vehicle = _vehicleRepository.GetVehicleById(vehicleId);
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
                    var vehicle = _vehicleRepository.GetVehicleById(vehicleId);
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

            return CreatedAtAction(nameof(GetWorkshopClientById), new { id = clientToCreate.Id }, clientToCreate);
        }

        [HttpPut("{workshopClientId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateWorkshopClient(int workshopClientId, [FromBody] WorkshopClientDto clientDto)
        {
            if (clientDto == null || workshopClientId != clientDto.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_workshopClientRepository.WorkshopClientExists(workshopClientId))
            {
                return NotFound();
            }

            var existingClient = _workshopClientRepository.GetWorkshopClientById(workshopClientId);

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
                var vehicle = _vehicleRepository.GetVehicleById(vehicleId);
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

        [HttpDelete("{workshopClientId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteWorkshopClient(int workshopClientId)
        {
            if (!_workshopClientRepository.WorkshopClientExists(workshopClientId))
            {
                return NotFound();
            }

            var clientToDelete = _workshopClientRepository.GetWorkshopClientById(workshopClientId);

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

        [HttpGet("{workshopClientId}/vehicles")]
        [ProducesResponseType(200, Type = typeof(ICollection<Vehicle>))]
        [ProducesResponseType(400)]
        public IActionResult GetWorkshopClientsVehicles(int workshopClientId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Workshop Client {workshopClientId} is not valid");
            }

            return Ok(_workshopClientRepository.GetWorkshopClientsVehicles(workshopClientId));
        }

        [HttpGet("{workshopClientId}/type")]
        [ProducesResponseType(200, Type = typeof(ClientType))]
        [ProducesResponseType(400)]
        public IActionResult GetWorkshopClientType(int workshopClientId)
        {
            if (!ModelState.IsValid || !_workshopClientRepository.WorkshopClientExists(workshopClientId))
            {
                return BadRequest($"Workshop Client {workshopClientId} is not valid");
            }

            return Ok(_workshopClientRepository.GetWorkshopClientType(workshopClientId));
        }

        [HttpGet("{workshopClientId}/exists")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        public IActionResult WorkshopClientExists(int workshopClientId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Workshop Client {workshopClientId} is not valid");
            }

            return Ok(_workshopClientRepository.WorkshopClientExists(workshopClientId));
        }
    }
}
