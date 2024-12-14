using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SZEW.Data;
using SZEW.Models;

var builder = WebApplication.CreateBuilder(args);

//DB Connection String
string dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
string dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
string dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "SZEW_DB";
string dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "SZEW_DB_USER";
string dbPass = Environment.GetEnvironmentVariable("DB_PASS") ?? "SZEW_DB_PASS";

string connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPass}";
builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;

builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
//modelBuilder.UseSnakeCaseNamingConvention();

//Add test data
builder.Services.AddTransient<SZEW.TestData>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "testdata")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory?.CreateScope())
    {
        var service = scope?.ServiceProvider.GetService<SZEW.TestData>();
        service?.SeedDataContext();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(o => o.WithTheme(ScalarTheme.DeepSpace));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "$DB_USER");

app.Run();

