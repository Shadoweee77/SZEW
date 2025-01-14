using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SZEW.DTO;
using SZEW.Interfaces;
using SZEW.Models;
using System.Collections.Generic;

namespace SZEW.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SaleDocumentController : ControllerBase
    {
        private readonly ISaleDocumentRepository _saleDocumentRepository;
        private readonly IMapper _mapper;

        public SaleDocumentController(ISaleDocumentRepository saleDocumentRepository, IMapper mapper)
        {
            _saleDocumentRepository = saleDocumentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<SaleDocumentDto>))]
        public IActionResult GetAllDocuments()
        {
            var documents = _saleDocumentRepository.GetAllDocuments();
            var documentsDto = _mapper.Map<List<SaleDocumentDto>>(documents);
            return Ok(documentsDto);
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

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(SaleDocumentDto))]
        [ProducesResponseType(400)]
        public IActionResult AddSaleDocument([FromBody] SaleDocumentDto saleDocumentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var saleDocument = _mapper.Map<SaleDocument>(saleDocumentDto);

            if (!_saleDocumentRepository.AddSaleDocument(saleDocument))
                return BadRequest("Could not add the sale document.");

            return CreatedAtAction("GetDocumentById", new { id = saleDocument.Id }, saleDocumentDto);
        }

        [HttpGet("exists/{id}")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult SaleDocumentExists(int id)
        {
            return Ok(_saleDocumentRepository.SaleDocumentExists(id));
        }
    }
}
