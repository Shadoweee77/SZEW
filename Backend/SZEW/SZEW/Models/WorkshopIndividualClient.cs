namespace SZEW.Models
{
    public class WorkshopIndividualClient : WorkshopClient
    {
        /*
        public WorkshopIndividualClient()
        {
            ClientType = ClientType.Individual;
        }
        */
        public required string Name { get; set; }
        public required string Surname { get; set; }
    }
}
