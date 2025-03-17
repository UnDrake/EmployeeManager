using EmployeeManager.Infrastructure;
using EmployeeManager.Data.Repositories;
using EmployeeManager.Core.Services;
using EmployeeManager.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException("Database connection string is missing.");

builder.Services.AddScoped(provider => new DatabaseConnection(connectionString));

builder.Services.AddScoped<CompanyRepository>();
builder.Services.AddScoped<EmployeeRepository>();
builder.Services.AddScoped<AddressRepository>();
builder.Services.AddScoped<PositionRepository>();
builder.Services.AddScoped<DepartmentRepository>();

builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddControllers();

var app = builder.Build();

var corsPolicy = builder.Configuration.GetValue<string>("CorsPolicy") ?? "AllowAll";
app.UseCors(corsPolicy);

app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
