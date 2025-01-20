using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Security.Claims;
using SZEW.DTO;
using SZEW.Interfaces;
using SZEW.Models;
using SZEW.Repository;

namespace SZEW.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SaleDocumentController : ControllerBase
    {
        private readonly ISaleDocumentRepository _saleDocumentRepository;
        private readonly IUserRepository _issuerRepository;
        private readonly IMapper _mapper;
        private readonly IWorkshopJobRepository _workshopJobRepository;

        public SaleDocumentController(ISaleDocumentRepository saleDocumentRepository, IUserRepository issuerRepository, IWorkshopJobRepository workshopJobRepository, IMapper mapper)
        {
            _saleDocumentRepository = saleDocumentRepository;
            _mapper = mapper;
            _issuerRepository = issuerRepository;
            _workshopJobRepository = workshopJobRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<SaleDocumentDto>))]
        public IActionResult GetAllDocuments()
        {
            var documents = _saleDocumentRepository.GetAllDocuments();
            var documentDtos = _mapper.Map<List<SaleDocumentDto>>(documents);
            return Ok(documentDtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(SaleDocumentDto))]
        [ProducesResponseType(404)]
        public IActionResult GetDocumentById(int id)
        {
            var document = _saleDocumentRepository.GetDocumentById(id);
            if (document == null)
                return NotFound();

            var documentDto = _mapper.Map<SaleDocumentDto>(document);
            return Ok(documentDto);
        }
        

        [HttpGet("{id}/exists")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult SaleDocumentExists(int id)
        {
            return Ok(_saleDocumentRepository.SaleDocumentExists(id));
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateSaleDocument([FromQuery] int relatedJobId, [FromBody] CreateSaleDocumentDto saleDocumentCreate)
        {
            if (saleDocumentCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var issuerIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (issuerIdClaim == null)
            {
                return Unauthorized("Requester ID not found in the token.");
            }

            if (!_workshopJobRepository.WorkshopJobExists(relatedJobId))
            {
                return BadRequest($"Workshop Job {relatedJobId} is not valid");
            }

            var issuerId = int.Parse(issuerIdClaim.Value);
            var saleDocumentMap = _mapper.Map<SaleDocument>(saleDocumentCreate);
            saleDocumentMap.DocumentIssuer = _issuerRepository.GetUserById(issuerId);
            saleDocumentMap.RelatedJob = _workshopJobRepository.GetJobById(relatedJobId);
            try
            {
                var maxId = _saleDocumentRepository.GetAllDocuments().Select(v => v.Id).DefaultIfEmpty(0).Max();
                saleDocumentMap.Id = maxId + 1;

                if (!_saleDocumentRepository.CreateSaleDocument(saleDocumentMap))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgresEx && postgresEx.SqlState == "23505")
            {
                ModelState.AddModelError("", "A sale document with the same ID already exists");
                return StatusCode(409, ModelState); // HTTP 409 Conflict
            }

            return Ok("Successfully created");
        }
        [HttpPut("{SaleDocumentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateSaleDocument(int SaleDocumentId, [FromBody] CreateSaleDocumentDto updatedSaleDocument)
        {
            if (updatedSaleDocument == null)
                return BadRequest(ModelState);

            if (!_saleDocumentRepository.SaleDocumentExists(SaleDocumentId))
                return NotFound();

            var existingSaleDocument = _saleDocumentRepository.GetDocumentById(SaleDocumentId);
            if (existingSaleDocument == null)
                return NotFound();

            _mapper.Map(updatedSaleDocument, existingSaleDocument);
            if (SaleDocumentId != existingSaleDocument.Id)
                return BadRequest(ModelState);

            if (!_saleDocumentRepository.UpdateSaleDocument(existingSaleDocument))
            {
                ModelState.AddModelError("", "Something went wrong updating the sale document");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        [HttpDelete("{saleDocumentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteSaleDocument(int saleDocumentId)
        {
            if (!_saleDocumentRepository.SaleDocumentExists(saleDocumentId))
            {
                return NotFound();
            }

            var saleDocumentToDelete = _saleDocumentRepository.GetDocumentById(saleDocumentId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_saleDocumentRepository.DeleteSaleDocument(saleDocumentToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the sale document");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


    }
}
