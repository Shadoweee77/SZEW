using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;
using SZEW.Data;
using SZEW.Interfaces;
using SZEW.Repository;

var builder = WebApplication.CreateBuilder(args);

// Database connection string
string dbHost = Dns.GetHostEntry(Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost").AddressList.First().ToString();
string dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
string dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "SZEW_DB";
string dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "SZEW_DB_USER";
string dbPass = Environment.GetEnvironmentVariable("DB_PASS") ?? "SZEW_DB_PASS";
string connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPass}";
builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;

builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add test data service
builder.Services.AddTransient<SZEW.TestData>();

// Add repositories to the DI container
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IWorkshopClientRepository, WorkshopClientRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add controllers to the service collection
builder.Services.AddControllers();

// Configure OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Seed data if specified in the arguments
if (args.Length >= 1 && args[0].ToLower() == "testdata")
{
    bool forced = args.Length > 1 && args[1].ToLower() == "forced";
    SeedData(app, forced);
}

void SeedData(IHost app, bool forced = false)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using (var scope = scopedFactory?.CreateScope())
    {
        var service = scope?.ServiceProvider.GetService<SZEW.TestData>();
        service?.SeedDataContext(forced);
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(o => o.WithTheme(ScalarTheme.DeepSpace));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Configure the home page
app.MapGet("/", () => {
    string highlightedText =
        "<p><b>S</b>ystem\n<b>Z</b>arz¹dzania i\n<b>E</b>widencji\n<b>W</b>arsztatu</p>" +
        "<p>DB Connection String: " + builder.Configuration.GetConnectionString("DefaultConnection") + "</p>" +
        "<p><a href='/scalar/v1'>Redirect to /scalar/v1</a></p>";
    return Results.Content(highlightedText, "text/html; charset=utf-8");
});

app.Run();
