using SZEW.Data;
using SZEW.Models;

namespace SZEW
{
    public class TestData
    {
        private readonly DataContext dataContext;
        public TestData(DataContext context)
        {
            this.dataContext = context;
        }

        public void SeedDataContext()
        {
            if (!dataContext.Vehicles.Any())
            {
                var vehicles = new List<Vehicle>()
                {
                    new Vehicle()
                    {
                        Id = 1,
                        Make = "Toyota",
                        Model = "Corolla",
                        Year = new DateTime(2015, 1, 1).ToUniversalTime(),
                        RegistrationNumber = "ABC-123",
                        Color = "White",
                        VIN = "1HGCM82633A123456",
                        Owner = new WorkshopIndividualClient()
                        {
                            Id = 1,
                            Email = "john.doe@example.com",
                            Address = "123 Elm Street",
                            PhoneNumber = "123-456-7890",
                            Name = "John",
                            Surname = "Doe"
                        }
                    },
                    new Vehicle()
                    {
                        Id = 2,
                        Make = "Custom", // Unknown make
                        Model = null,    // Unknown model
                        Year = null,     // Unknown year
                        RegistrationNumber = "XYZ-987",
                        Color = "Black",
                        VIN = null, // No VIN
                        Owner = new WorkshopBusinessClient()
                        {
                            Id = 2,
                            Email = "support@acme.com",
                            Address = "456 Industry Blvd",
                            PhoneNumber = "800-555-1234",
                            Name = "ACME Corp",
                            NIP = "1234567890"
                        }
                    },
                    new Vehicle()
                    {
                        Id = 3,
                        Make = "Ford",
                        Model = "F-150",
                        Year = new DateTime(2020, 1, 1).ToUniversalTime(),
                        RegistrationNumber = "FTRUCK01",
                        Color = "Blue",
                        VIN = "1FTFW1ET9EKE12345",
                        Owner = new WorkshopIndividualClient()
                        {
                            Id = 3,
                            Email = "alice.smith@example.com",
                            Address = "789 Oak Avenue",
                            PhoneNumber = "987-654-3210",
                            Name = "Alice",
                            Surname = "Smith"
                        }
                    }
                };

                dataContext.Vehicles.AddRange(vehicles);
                dataContext.SaveChanges();
            }
        }
    }
}
