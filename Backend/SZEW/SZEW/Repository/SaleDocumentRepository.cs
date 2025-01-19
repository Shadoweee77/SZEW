using Microsoft.EntityFrameworkCore;
using SZEW.Data;
using SZEW.Interfaces;
using SZEW.Models;

namespace SZEW.Repository
{
    public class SaleDocumentRepository : ISaleDocumentRepository
    {
        private readonly DataContext _context;

        public SaleDocumentRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<SaleDocument> GetAllDocuments()
        {
            return _context.SaleDocuments.Include(SaleDocument => SaleDocument.RelatedJob).Include(SaleDocument => SaleDocument.DocumentIssuer).OrderBy(d => d.Id).ToList();
        }

        public SaleDocument GetDocumentById(int id)
        {
            return _context.SaleDocuments.Include(SaleDocument => SaleDocument.RelatedJob).Include(SaleDocument => SaleDocument.DocumentIssuer).FirstOrDefault(d => d.Id == id);
        }


        public bool SaleDocumentExists(int id)
        {
            return _context.SaleDocuments.Any(d => d.Id == id);
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public bool CreateSaleDocument(SaleDocument saleDocument)
        {
            _context.SaleDocuments.Add(saleDocument);
            return Save();
        }
        public bool UpdateSaleDocument(SaleDocument saleDocument)
        {
            _context.Update(saleDocument);
            return Save();
        }

        public bool DeleteSaleDocument(SaleDocument saleDocument)
        {
            _context.Remove(saleDocument);
            return Save();
        }
    }
}
