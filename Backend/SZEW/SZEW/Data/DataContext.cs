using Microsoft.EntityFrameworkCore;
using SZEW.Models;

namespace SZEW.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<WorkshopClient> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkshopClient>()
                .HasDiscriminator<ClientType>("ClientType")
                .HasValue<WorkshopIndividualClient>(ClientType.Individual)
                .HasValue<WorkshopBusinessClient>(ClientType.Business);

            base.OnModelCreating(modelBuilder);

            /*
             * Relacje wiele do wielu category1 i category2 np zlecenie i usługa.
             * https://youtu.be/EmV_IBYIlyo?si=FCDPgHcsGV3xhWai&t=800
            modelBuilder.Entity<Cat1Cat2>()
                .HasKey(pc => new { pc.cat1Id, pc.cat2Id });
            */
        }
    }
}
