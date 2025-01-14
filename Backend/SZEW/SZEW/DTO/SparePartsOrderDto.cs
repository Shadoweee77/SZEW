namespace SZEW.DTO
{
    public class SparePartsOrderDto
    {
        public int Id { get; set; }
        public int OrdererId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public ICollection<int> SparePartsIds { get; set; }
    }
}
