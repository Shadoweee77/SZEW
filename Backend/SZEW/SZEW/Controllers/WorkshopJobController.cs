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
    public class WorkshopJobController : Controller
    {
        private readonly IWorkshopJobRepository _workshopJobRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;
        public WorkshopJobController(IWorkshopJobRepository workshopJobRepository, IVehicleRepository vehicleRepository, IMapper mapper)
        {
            this._workshopJobRepository = workshopJobRepository;
            this._vehicleRepository = vehicleRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<WorkshopJob>))]
        public IActionResult GetAllWorkshopJobs()
        {
            var jobs = _mapper.Map<List<WorkshopJobDto>>(_workshopJobRepository.GetAllWorkshopJobs());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(jobs);
        }

        [HttpGet("{workshopJobId}")]
        [ProducesResponseType(200, Type = typeof(WorkshopJob))]
        public IActionResult GetWorkshopJobById(int workshopJobId)
        {
            var job = _mapper.Map<WorkshopJobDto>(_workshopJobRepository.GetWorkshopJobById(workshopJobId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(job);
        }

        [HttpGet("{workshopJobId}/exists")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        public IActionResult WorkshopJobExists(int workshopJobId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Job {workshopJobId} is not valid");
            }

            if (_workshopJobRepository.WorkshopJobExists(workshopJobId))
            {
                return Ok(true);
            }
            else return Ok(false);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateWorkshopJob([FromQuery] int vehicleId, [FromBody] CreateWorkshopJobDto workshopJobCreate)
        {
            if (workshopJobCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_vehicleRepository.VehicleExists(vehicleId))
            {
                return BadRequest($"Vehicle {vehicleId} is not valid");
            }
            // duplicate check could be added here maybe by VehicleId + description, idk if needed
            var workshopJobMap = _mapper.Map<WorkshopJob>(workshopJobCreate);
            workshopJobMap.Vehicle = _vehicleRepository.GetVehicleById(vehicleId);

            try
            {
                var maxId = _workshopJobRepository.GetAllWorkshopJobs().Select(v => v.Id).DefaultIfEmpty(0).Max();
                workshopJobMap.Id = maxId + 1;

                if (!_workshopJobRepository.CreateWorkshopJob(workshopJobMap))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgresEx && postgresEx.SqlState == "23505")
            {
                ModelState.AddModelError("", "A vehicle with the same ID already exists");
                return StatusCode(409, ModelState);
            }
            return Ok("Successfully created");
        }

        [HttpPut("{workshopJobId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateWorkshopJob(int workshopJobId, [FromBody] CreateWorkshopJobDto updatedWorkshopJob)
        {
            if (updatedWorkshopJob == null)
                return BadRequest(ModelState);


            if (!_workshopJobRepository.WorkshopJobExists(workshopJobId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var existingWorkshopJob = _workshopJobRepository.GetWorkshopJobById(workshopJobId);
            _mapper.Map(updatedWorkshopJob, existingWorkshopJob);

            if(workshopJobId!= existingWorkshopJob.Id)
                return BadRequest(ModelState);
            if (!_workshopJobRepository.UpdateWorkshopJob(existingWorkshopJob))
            {
                ModelState.AddModelError("", "Something went wrong updating workshop job");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{workshopJobId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteWorkshopJob(int workshopJobId)
        {
            if (!_workshopJobRepository.WorkshopJobExists(workshopJobId))
            {
                return NotFound();
            }

            var workshopJobToDelete = _workshopJobRepository.GetWorkshopJobById(workshopJobId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_workshopJobRepository.DeleteWorkshopJob(workshopJobToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting workshop job");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
