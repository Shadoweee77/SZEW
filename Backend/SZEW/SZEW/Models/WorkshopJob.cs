namespace SZEW.Models
{
    public class WorkshopJob
    {
        //UML - Zlecenie (jedno na jeden pojazd w danym czasie. zawiera wiele usług/WorkshopTask )
        public required int Id { get; set; }
        public required Vehicle Vehicle { get; set; }
        public required bool Complete { get; set; }
        public required string Description {  get; set; }
        public required DateTime AdmissionDate { get; set; } = DateTime.Now;
        public ICollection<WorkshopTask>? Tasks { get; set; }
        public SaleDocument? RelatedSaleDocument { get; set; }

    }
}
