using SZEW.Data;
using SZEW.Interfaces;
using SZEW.Models;
using System.Collections.Generic;
using System.Linq;

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
            return _context.SaleDocuments.OrderBy(d => d.Id).ToList();
        }

        public SaleDocument GetDocumentById(int id)
        {
            return _context.SaleDocuments.FirstOrDefault(d => d.Id == id);
        }

        public bool AddSaleDocument(SaleDocument saleDocument)
        {
            _context.SaleDocuments.Add(saleDocument);
            return Save();
        }

        public bool SaleDocumentExists(int id)
        {
            return _context.SaleDocuments.Any(d => d.Id == id);
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
