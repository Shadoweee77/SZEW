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
    public class WorkshopJobController : Controller
    {
        private readonly IWorkshopJobRepository _workshopJobRepository;

        public WorkshopJobController(IWorkshopJobRepository workshopJobRepository)
        {
            this._workshopJobRepository = workshopJobRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<WorkshopJob>))]
        public IActionResult GetAllJobs()
        {
            var jobs = _workshopJobRepository.GetAllJobs();

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
            var job = _workshopJobRepository.GetJobById(id);

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
    }
}
