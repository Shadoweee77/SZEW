namespace SZEW.Models
{
    public class WorkshopBusinessClient : WorkshopClient
    {
        /*
        public WorkshopBusinessClient()
        {
            ClientType = ClientType.Business;
        }
        */
        public required string Name { get; set; }
        public required string NIP { get; set; }
    }
}
