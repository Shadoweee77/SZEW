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
    public class WorkshopTaskController : Controller
    {
        private readonly IWorkshopTaskRepository _workshopTaskRepository;
        private readonly IWorkshopJobRepository _workshopJobRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public WorkshopTaskController(IWorkshopTaskRepository workshopTaskRepository, IWorkshopJobRepository workshopJobRepository, IUserRepository userRepository, IMapper mapper)
        {
            this._workshopTaskRepository = workshopTaskRepository;
            this._workshopJobRepository = workshopJobRepository;
            this._userRepository = userRepository;
            _mapper = mapper;

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<WorkshopTask>))]
        public IActionResult GetAllTasks()
        {
            var tasks = _mapper.Map<List<WorkshopTaskDto>>(_workshopTaskRepository.GetAllTasks());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(tasks);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(Vehicle))]
        [ProducesResponseType(400)]
        public IActionResult GetTaskById(int id)
        {
            if (!_workshopTaskRepository.WorkshopTaskExists(id))
            {
                return NotFound();
            }

            var task = _mapper.Map<WorkshopTaskDto>(_workshopTaskRepository.GetTaskById(id));

            if (!ModelState.IsValid)
            {
                return BadRequest($"Task {id} is not valid");
            }

            return Ok(task);
        }

        [HttpGet("{id}/exists")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        public IActionResult WorkshopTaskExists(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Vehicle {id} is not valid");
            }

            if (_workshopTaskRepository.WorkshopTaskExists(id))
            {
                return Ok(true);
            }
            else return Ok(false);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateWorkshopJob([FromQuery] int WorkshopJobId, [FromQuery] int AssignedWorkerId, [FromBody] CreateWorkshopTaskDto workshopTaskCreate)
        {
            if (workshopTaskCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // duplicate check could be added here maybe by VehicleId + description, idk if needed
            var workshopTaskMap = _mapper.Map<WorkshopTask>(workshopTaskCreate);
            workshopTaskMap.WorkshopJob = _workshopJobRepository.GetJobById(WorkshopJobId);
            workshopTaskMap.AssignedWorker = _userRepository.GetUserById(AssignedWorkerId);

            try
            {
                // Manually set the ID based on the current max ID from the database
                var maxId = _workshopTaskRepository.GetAllTasks().Max(v => v.Id);
                workshopTaskMap.Id = maxId + 1;  // Set the new ID to max(id) + 1

                // Create the vehicle with the manually set ID
                if (!_workshopTaskRepository.CreateWorkshopTask(workshopTaskMap))
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
        [HttpPut("{workshopTaskId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateWorkshopTask(int workshopTaskId, [FromBody] CreateWorkshopTaskDto updatedWorkshopTask)
        {
            if (updatedWorkshopTask == null)
                return BadRequest(ModelState);


            if (!_workshopTaskRepository.WorkshopTaskExists(workshopTaskId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var existingWorkshopTask = _workshopTaskRepository.GetTaskById(workshopTaskId);
            _mapper.Map(updatedWorkshopTask, existingWorkshopTask);

            if (workshopTaskId != existingWorkshopTask.Id)
                return BadRequest(ModelState);
            if (!_workshopTaskRepository.UpdateWorkshopTask(existingWorkshopTask))
            {
                ModelState.AddModelError("", "Something went wrong updating workshop task");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        [HttpDelete("{workshopTaskId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteWorkshopTask(int workshopTaskId)
        {
            if (!_workshopTaskRepository.WorkshopTaskExists(workshopTaskId))
            {
                return NotFound();
            }

            var workshopTaskToDelete = _workshopTaskRepository.GetTaskById(workshopTaskId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_workshopTaskRepository.DeleteWorkshopTask(workshopTaskToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting workshop task");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
