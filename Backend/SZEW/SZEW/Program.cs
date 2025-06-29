using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;
using SZEW.Data;
using SZEW.Interfaces;
using SZEW.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Collections.Specialized;
using SZEW.Models;


var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment.EnvironmentName;
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

// Database connection string
string dbHost = Dns.GetHostEntry(Environment.GetEnvironmentVariable("DB_HOST") ?? "127.0.0.1").AddressList.First().ToString();
string dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
string dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "SZEW_DB";
string dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "SZEW_DB_USER";
string dbPass = Environment.GetEnvironmentVariable("DB_PASS") ?? "SZEW_DB_PASS";
string connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPass}";
builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;

builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add test data service
builder.Services.AddTransient<SZEW.TestData>();

// Add JWT Authentication to the services
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
    };
});

// Add Authorization to the service collection
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("MechanicOnly", policy => policy.RequireRole("Mechanic"));
});

// Add repositories to the DI container
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IWorkshopClientRepository, WorkshopClientRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWorkshopTaskRepository, WorkshopTaskRepository>();
builder.Services.AddScoped<IWorkshopJobRepository, WorkshopJobRepository>();
builder.Services.AddScoped<IToolRepository, ToolRepository>();
builder.Services.AddScoped<ISparePartRepository, SparePartRepository>();
builder.Services.AddScoped<ISparePartsOrderRepository, SparePartsOrderRepository>();
builder.Services.AddScoped<IToolsOrderRepository, ToolsOrderRepository>();
builder.Services.AddScoped<IToolsRequestRepository, ToolsRequestRepository>();
builder.Services.AddScoped<ISaleDocumentRepository, SaleDocumentRepository>();


// Add controllers to the service collection
builder.Services.AddControllers();

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
if (app.Environment.IsDevelopment() || true) //Docker build enviornment is being interpreted as the Prod.
{
    app.MapOpenApi();
    app.MapScalarApiReference(o => o.WithTheme(ScalarTheme.DeepSpace));
}

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Configure the home page
app.MapGet("/", () => {
    string highlightedText =
        "<p><b>S</b>ystem\n<b>Z</b>arządzania i\n<b>E</b>widencji\n<b>W</b>arsztatu</p>" +
        "<p>DB Connection String: " + builder.Configuration.GetConnectionString("DefaultConnection") + "</p>" +
        "<p><a href='/scalar/v1'>Redirect to /scalar/v1</a></p>";
    return Results.Content(highlightedText, "text/html; charset=utf-8");
});

app.Run();