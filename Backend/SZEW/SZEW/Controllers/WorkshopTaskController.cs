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
        public IActionResult GetAllWorkshopTasks()
        {
            var tasks = _mapper.Map<List<WorkshopTaskDto>>(_workshopTaskRepository.GetAllWorkshopTasks());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(tasks);
        }

        [HttpGet("{workshopTaskId:int}")]
        [ProducesResponseType(200, Type = typeof(Vehicle))]
        [ProducesResponseType(400)]
        public IActionResult GetWorkshopTaskById(int workshopTaskId)
        {
            if (!_workshopTaskRepository.WorkshopTaskExists(workshopTaskId))
            {
                return NotFound();
            }

            var task = _mapper.Map<WorkshopTaskDto>(_workshopTaskRepository.GetWorkshopTaskById(workshopTaskId));

            if (!ModelState.IsValid)
            {
                return BadRequest($"Task {workshopTaskId} is not valid");
            }

            return Ok(task);
        }

        [HttpGet("{workshopTaskId}/exists")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        public IActionResult WorkshopTaskExists(int workshopTaskId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Workshop task with id {workshopTaskId} is not valid");
            }

            if (_workshopTaskRepository.WorkshopTaskExists(workshopTaskId))
            {
                return Ok(true);
            }
            else return Ok(false);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateWorkshopJob([FromQuery] int workshopJobId, [FromQuery] int assignedWorkerId, [FromBody] CreateWorkshopTaskDto workshopTaskCreate)
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
            if (!_workshopJobRepository.WorkshopJobExists(workshopJobId))
            {
                return BadRequest($"Workshop Job {workshopJobId} is not valid");
            }
            workshopTaskMap.WorkshopJob = _workshopJobRepository.GetWorkshopJobById(workshopJobId);
            if (!_userRepository.UserExists(assignedWorkerId))
            {
                return BadRequest($"User {assignedWorkerId} is not valid");
            }
            workshopTaskMap.AssignedWorker = _userRepository.GetUserById(assignedWorkerId);

            try
            {
                var maxId = _workshopTaskRepository.GetAllWorkshopTasks().Select(v => v.Id).DefaultIfEmpty(0).Max();
                workshopTaskMap.Id = maxId + 1;

                if (!_workshopTaskRepository.CreateWorkshopTask(workshopTaskMap))
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

            var existingWorkshopTask = _workshopTaskRepository.GetWorkshopTaskById(workshopTaskId);
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

            var workshopTaskToDelete = _workshopTaskRepository.GetWorkshopTaskById(workshopTaskId);

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
