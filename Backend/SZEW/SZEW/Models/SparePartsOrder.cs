namespace SZEW.Models
{
    public class SparePartsOrder
    {
        public required int Id { get; set; }
        public required User Orderer { get; set; }
        public required DateTime RegistrationDate { get; set; }
        public ICollection<SparePart>? SpareParts { get; set; }
    }
}
