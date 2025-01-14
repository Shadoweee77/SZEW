using SZEW.Models;
using System.Collections.Generic;

namespace SZEW.Interfaces
{
    public interface ISaleDocumentRepository
    {
        ICollection<SaleDocument> GetAllDocuments();
        SaleDocument GetDocumentById(int id);
        bool AddSaleDocument(SaleDocument saleDocument);
        bool SaleDocumentExists(int id);
        bool Save();
    }
}
