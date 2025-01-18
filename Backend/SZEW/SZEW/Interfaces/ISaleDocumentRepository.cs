using SZEW.Models;

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
