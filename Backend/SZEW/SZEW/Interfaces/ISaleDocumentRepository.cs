using SZEW.Models;

namespace SZEW.Interfaces
{
    public interface ISaleDocumentRepository
    {
        ICollection<SaleDocument> GetAllSaleDocuments();
        SaleDocument GetSaleDocumentById(int id);
        
        bool SaleDocumentExists(int id);
        bool CreateSaleDocument(SaleDocument saleDocument);
        bool UpdateSaleDocument(SaleDocument saleDocument);
        bool DeleteSaleDocument(SaleDocument saleDocument);
        bool Save();
    }
}
