using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;
        public WorkshopTaskController(IWorkshopTaskRepository workshopTaskRepository, IMapper mapper)
        {
            this._workshopTaskRepository = workshopTaskRepository;
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
    }
}
