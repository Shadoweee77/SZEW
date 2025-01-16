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
        public IActionResult GetAllJobs()
        {
            var jobs = _mapper.Map<List<WorkshopJobDto>>(_workshopJobRepository.GetAllJobs());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(jobs);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(WorkshopJob))]
        public IActionResult GetJobById(int id)
        {
            var job = _mapper.Map<WorkshopJobDto>(_workshopJobRepository.GetJobById(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(job);
        }

        [HttpGet("{id}/exists")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        public IActionResult WorkshopJobExists(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Job {id} is not valid");
            }

            if (_workshopJobRepository.WorkshopJobExists(id))
            {
                return Ok(true);
            }
            else return Ok(false);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateWorkshopJob([FromQuery] int VehicleId, [FromBody] CreateWorkshopJobDto workshopJobCreate)
        {
            if (workshopJobCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // duplicate check could be added here maybe by VehicleId + description, idk if needed
            var workshopJobMap = _mapper.Map<WorkshopJob>(workshopJobCreate);
            workshopJobMap.Vehicle = _vehicleRepository.GetVehicle(VehicleId);

            try
            {
                // Manually set the ID based on the current max ID from the database
                var maxId = _workshopJobRepository.GetAllJobs().Max(v => v.Id);
                workshopJobMap.Id = maxId + 1;  // Set the new ID to max(id) + 1

                // Create the vehicle with the manually set ID
                if (!_workshopJobRepository.CreateWorkshopJob(workshopJobMap))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgresEx && postgresEx.SqlState == "23505")
            {
                ModelState.AddModelError("", "A vehicle with the same ID already exists");
                return StatusCode(409, ModelState); // HTTP 409 Conflict
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
            var existingWorkshopJob = _workshopJobRepository.GetJobById(workshopJobId);
            _mapper.Map(updatedWorkshopJob, existingWorkshopJob);
            if(workshopJobId!= existingWorkshopJob.Id)
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

            var workshopJobToDelete = _workshopJobRepository.GetJobById(workshopJobId);

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
