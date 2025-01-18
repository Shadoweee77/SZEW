using Microsoft.EntityFrameworkCore;
using SZEW.Models;

namespace SZEW.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<WorkshopClient> Clients { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WorkshopTask> Tasks {  get; set; }
        public DbSet<WorkshopJob> Jobs { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<ToolsOrder> ToolsOrders { get; set; }
        public DbSet<ToolsRequest> ToolsRequests { get; set; }
        public DbSet<SparePart> SpareParts { get; set; }
        public DbSet<SparePartsOrder> SparePartsOrders { get; set; }
        public DbSet<SaleDocument> SaleDocuments { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkshopClient>()
                .HasDiscriminator<ClientType>("ClientType")
                .HasValue<WorkshopIndividualClient>(ClientType.Individual)
                .HasValue<WorkshopBusinessClient>(ClientType.Business);

            modelBuilder.Entity<ToolsRequest>()
                .HasOne(tr => tr.Requester)
                .WithMany(u => u.ToolsRequestsRequested)
                .HasForeignKey(tr => tr.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ToolsRequest>()
                .HasOne(tr => tr.Verifier)
                .WithMany(u => u.ToolsRequestsVerified)
                .HasForeignKey(tr => tr.VerifierId)
                .OnDelete(DeleteBehavior.SetNull);
            // Konfiguracja relacji User(Requester) - ToolsRequest i User(Verifier) - ToolsRequest
        }
    }
}
