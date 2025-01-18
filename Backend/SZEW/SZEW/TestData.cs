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

        public void SeedDataContext(bool forced = false)
        {
            if (forced)
            {
                ClearDatabase();
            }

            if (!dataContext.Vehicles.Any() || forced)
            {
                var client1 = new WorkshopIndividualClient()
                {
                    Id = 1,
                    Email = "john.doe@example.com",
                    Address = "123 Elm Street",
                    PhoneNumber = "123-456-7890",
                    Name = "John",
                    Surname = "Doe",
                    ClientType = ClientType.Individual
                };

                var client2 = new WorkshopBusinessClient()
                {
                    Id = 2,
                    Email = "support@acme.com",
                    Address = "456 Industry Blvd",
                    PhoneNumber = "800-555-1234",
                    Name = "ACME Corp",
                    NIP = "1234567890",
                    ClientType = ClientType.Business
                };

                var client3 = new WorkshopIndividualClient()
                {
                    Id = 3,
                    Email = "alice.smith@example.com",
                    Address = "789 Oak Avenue",
                    PhoneNumber = "987-654-3210",
                    Name = "Alice",
                    Surname = "Smith",
                    ClientType = ClientType.Individual
                };

                var client4 = new WorkshopIndividualClient()
                {
                    Id = 4,
                    Email = "bob.jones@example.com",
                    Address = "321 Pine Street",
                    PhoneNumber = "456-789-0123",
                    Name = "Bob",
                    Surname = "Jones",
                    ClientType = ClientType.Individual
                };

                var client5 = new WorkshopBusinessClient()
                {
                    Id = 5,
                    Email = "info@widgetworks.com",
                    Address = "789 Factory Lane",
                    PhoneNumber = "800-555-5678",
                    Name = "Widget Works",
                    NIP = "9876543210",
                    ClientType = ClientType.Business
                };

                dataContext.AddRange(client1, client2, client3, client4, client5);

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
                        Owner = client1
                    },
                    new Vehicle()
                    {
                        Id = 2,
                        Make = "Custom",
                        Model = null,
                        Year = null,
                        RegistrationNumber = "XYZ-987",
                        Color = "Black",
                        VIN = null,
                        Owner = client2
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
                        Owner = client3
                    },
                    new Vehicle()
                    {
                        Id = 4,
                        Make = "Lamborghini",
                        Model = "Gallardo",
                        Year = new DateTime(2009, 12, 10).ToUniversalTime(),
                        RegistrationNumber = null,
                        Color = "Black",
                        VIN = "ABCDE12345AAS2323",
                        Owner = client3
                    },
                    new Vehicle()
                    {
                        Id = 5,
                        Make = "Chevrolet",
                        Model = "Camaro",
                        Year = new DateTime(2018, 6, 15).ToUniversalTime(),
                        RegistrationNumber = "CAMARO01",
                        Color = "Red",
                        VIN = "1G1FH1R78J0105678",
                        Owner = client4
                    },
                    new Vehicle()
                    {
                        Id = 6,
                        Make = "Tesla",
                        Model = "Model S",
                        Year = new DateTime(2021, 3, 30).ToUniversalTime(),
                        RegistrationNumber = "TESLA01",
                        Color = "Silver",
                        VIN = "5YJSA1E47HF123456",
                        Owner = client5
                    },
                    new Vehicle()
                    {
                        Id = 7,
                        Make = "BMW",
                        Model = "X5",
                        Year = new DateTime(2022, 8, 12).ToUniversalTime(),
                        RegistrationNumber = "BMWX507",
                        Color = "Grey",
                        VIN = "5UXCR6C54L0X12345",
                        Owner = client4
                    }
                };

                var users = new List<User>()
                {
                    new User()
                    {
                        Id = 1,
                        Login = "admin1",
                        Name = "Admin",
                        Surname = "Oneeeeeeeee",
                        Email = "admin1@workshop.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
                        UserType = UserType.Admin
                    },
                    new User()
                    {
                        Id = 2,
                        Login = "mechanic1",
                        Name = "Mechanic",
                        Surname = "One",
                        Email = "mechanic1@workshop.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("mechanic"),
                        UserType = UserType.Mechanic
                    }
                };

                var workshopJobs = new List<WorkshopJob>()
                {
                    new WorkshopJob()
                    {
                        Id = 1,
                        Vehicle = vehicles.First(v => v.Id == 1), // Toyota Corolla
                        IsComplete = false,
                        Description = "Oil change and tire rotation.",
                        AdmissionDate = DateTime.UtcNow.AddDays(-7),
                        RelatedSaleDocument = null
                    },
                    new WorkshopJob()
                    {
                        Id = 2,
                        Vehicle = vehicles.First(v => v.Id == 3), // Ford F-150
                        IsComplete = true,
                        Description = "Brake pad replacement and battery check.",
                        AdmissionDate = DateTime.UtcNow.AddDays(-30),
                        RelatedSaleDocument = null
                    }
                };


                var workshopTasks = new List<WorkshopTask>()
                {
                    new WorkshopTask()
                    {
                        Id = 1,
                        Complete = false,
                        Name = "Oil Change",
                        Description = "Replace engine oil with synthetic oil.",
                        Price = 200,
                        AssignedWorker = users.First(u => u.Login == "mechanic1"),
                        WorkshopJob = workshopJobs.First(w => w.Id == 1)

                    },
                    new WorkshopTask()
                    {
                        Id = 2,
                        Name = "Tire Rotation",
                        Description = "Rotate all four tires to ensure even wear.",
                        Price = 20.00,
                        Complete = false,
                        AssignedWorker = users.First(u => u.Login == "mechanic1"),
                        WorkshopJob = workshopJobs.First(w => w.Id == 1)
                    },
                    new WorkshopTask()
                    {
                        Id = 3,
                        Name = "Brake Pad Replacement",
                        Description = "Replace front and rear brake pads.",
                        Price = 150.00,
                        Complete = true,
                        AssignedWorker = users.First(u => u.Login == "mechanic1"),
                        WorkshopJob = workshopJobs.First(w => w.Id == 2)
                    },
                    new WorkshopTask()
                    {
                        Id = 4,
                        Name = "Battery Check",
                        Description = "Inspect and test battery health.",
                        Price = 30.00,
                        Complete = true,
                        AssignedWorker = users.First(u => u.Login == "mechanic1"),
                        WorkshopJob = workshopJobs.First(w => w.Id == 2)
                    }
                };

                var saleDocuments = new List<SaleDocument>()
                {
                    new SaleDocument()
                    {
                        Id = 1,
                        DocumentType = DocumentType.Paragon,
                        IssueDate = DateTime.UtcNow.AddDays(-5),
                        DocumentIssuer = users.First(u => u.Login == "admin1"),
                        RelatedJobId = workshopJobs.First(w => w.Id == 1).Id,
                        RelatedJob = workshopJobs.First(w => w.Id == 1),
                        IsPaid = false
                    },
                    new SaleDocument()
                    {
                        Id = 2,
                        DocumentType = DocumentType.Faktura,
                        IssueDate = DateTime.UtcNow.AddDays(-20),
                        DocumentIssuer = users.First(u => u.Login == "admin1"),
                        RelatedJobId = workshopJobs.First(w => w.Id == 1).Id,
                        RelatedJob = workshopJobs.First(w => w.Id == 2),
                        IsPaid = true
                    }
                };

                var sparePartsOrders = new List<SparePartsOrder>()
                {
                    new SparePartsOrder()
                    {
                        Id = 1,
                        Orderer = users.First(u => u.Login == "admin1"),
                        RegistrationDate = DateTime.UtcNow.AddDays(-10),
                        SpareParts = null
                    },
                    new SparePartsOrder()
                    {
                        Id = 2,
                        Orderer = users.First(u => u.Login == "mechanic1"),
                        RegistrationDate = DateTime.UtcNow.AddDays(-5),
                        SpareParts = null
                    }
                };

                var spareParts = new List<SparePart>()
                {
                    new SparePart()
                    {
                        Id = 1,
                        Order = sparePartsOrders.First(o => o.Id == 1),
                        Name = "Brake Pads",
                        Price = 30.00,
                        Amount = 4
                    },
                    new SparePart()
                    {
                        Id = 2,
                        Order = sparePartsOrders.First(o => o.Id == 1),
                        Name = "Oil Filter",
                        Price = 10.00,
                        Amount = 2
                    },
                    new SparePart()
                    {
                        Id = 3,
                        Order = sparePartsOrders.First(o => o.Id == 2),
                        Name = "Air Filter",
                        Price = 20.00,
                        Amount = 3
                    },
                    new SparePart()
                    {
                        Id = 4,
                        Order = sparePartsOrders.First(o => o.Id == 2),
                        Name = "Spark Plugs",
                        Price = 15.00,
                        Amount = 6
                    }
                };

                var toolsOrders = new List<ToolsOrder>()
                {
                    new ToolsOrder()
                    {
                        Id = 1,
                        Orderer = users.First(u => u.Login == "admin1"),
                        RegistrationDate = DateTime.UtcNow.AddDays(-20),
                    },
                    new ToolsOrder()
                    {
                        Id = 2,
                        Orderer = users.First(u => u.Login == "mechanic1"),
                        RegistrationDate = DateTime.UtcNow.AddDays(-10),
                    }
                };

                var tools = new List<Tool>()
                {
                    new Tool()
                    {
                        Id = 1,
                        Order = toolsOrders[0],
                        Name = "Hammer",
                        Price = 15.00,
                        Amount = 3
                    },
                    new Tool()
                    {
                        Id = 2,
                        Order = toolsOrders[0],
                        Name = "Screwdriver Set",
                        Price = 25.00,
                        Amount = 2
                    },
                    new Tool()
                    {
                        Id = 3,
                        Order = toolsOrders[1],
                        Name = "Wrench",
                        Price = 10.00,
                        Amount = 5
                    },
                    new Tool()
                    {
                        Id = 4,
                        Order = toolsOrders[1],
                        Name = "Power Drill",
                        Price = 75.00,
                        Amount = 1
                    }
                };

                //toolsOrders[0].Tools.Add(tools.First(t => t.Id == 1));
                //toolsOrders[0].Tools.Add(tools.First(t => t.Id == 2));
                //toolsOrders[1].Tools.Add(tools.First(t => t.Id == 3));
                //toolsOrders[1].Tools.Add(tools.First(t => t.Id == 4));

                var toolsRequests = new List<ToolsRequest>()
                {
                    new ToolsRequest()
                    {
                        Id = 1,
                        RequesterId = users.First(u => u.Login == "mechanic1").Id,
                        Requester = users.First(u => u.Login == "mechanic1"),
                        VerifierId = users.FirstOrDefault(u => u.Login == "admin1")?.Id,
                        Verifier = users.FirstOrDefault(u => u.Login == "admin1"),
                        Accepted = true,
                        Description = "Request for a new set of torque wrenches.",
                        Price = 150.00,
                        Amount = 1,
                        RequestDate = DateTime.UtcNow.AddDays(-10)
                    },
                    new ToolsRequest()
                    {
                        Id = 2,
                        RequesterId = users.First(u => u.Login == "mechanic1").Id,
                        Requester = users.First(u => u.Login == "mechanic1"),
                        VerifierId = null,
                        Verifier = null,
                        Accepted = false,
                        Description = "Request for a hydraulic jack.",
                        Price = 300.00,
                        Amount = 1,
                        RequestDate = DateTime.UtcNow.AddDays(-5)
                    },
                    new ToolsRequest()
                    {
                        Id = 3,
                        RequesterId = users.First(u => u.Login == "mechanic1").Id,
                        Requester = users.First(u => u.Login == "mechanic1"),
                        VerifierId = users.FirstOrDefault(u => u.Login == "admin1")?.Id,
                        Verifier = users.FirstOrDefault(u => u.Login == "admin1"),
                        Accepted = true,
                        Description = "Request for a pneumatic nail gun.",
                        Price = 200.00,
                        Amount = 2,
                        RequestDate = DateTime.UtcNow.AddDays(-2)
                    }
                };

                dataContext.ToolsRequests.AddRange(toolsRequests);
                dataContext.Tools.AddRange(tools);
                dataContext.ToolsOrders.AddRange(toolsOrders);
                dataContext.SpareParts.AddRange(spareParts);
                dataContext.SparePartsOrders.AddRange(sparePartsOrders);
                dataContext.SaleDocuments.AddRange(saleDocuments);
                dataContext.Jobs.AddRange(workshopJobs);
                dataContext.Tasks.AddRange(workshopTasks);
                dataContext.Vehicles.AddRange(vehicles);
                dataContext.Users.AddRange(users);






                dataContext.SaveChanges();
            }
        }

        private void ClearDatabase()
        {
            dataContext.Vehicles.RemoveRange(dataContext.Vehicles);
            dataContext.Users.RemoveRange(dataContext.Users);
            dataContext.Clients.RemoveRange(dataContext.Clients);

            dataContext.ToolsRequests.RemoveRange(dataContext.ToolsRequests);
            dataContext.Tools.RemoveRange(dataContext.Tools);
            dataContext.ToolsOrders.RemoveRange(dataContext.ToolsOrders);
            dataContext.SpareParts.RemoveRange(dataContext.SpareParts);
            dataContext.SparePartsOrders.RemoveRange(dataContext.SparePartsOrders);
            dataContext.SaleDocuments.RemoveRange(dataContext.SaleDocuments);
            dataContext.Jobs.RemoveRange(dataContext.Jobs);
            dataContext.Tasks.RemoveRange(dataContext.Tasks);


            dataContext.SaveChanges();
        }
    }
}
