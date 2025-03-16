using EmployeeManager.API.Services;
using EmployeeManager.Infrastructure;
using EmployeeManager.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddSingleton(new DatabaseConnection(connectionString));
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<EmployeeService>();
// Â Program.cs
builder.Services.AddScoped<CompanyRepository>();
builder.Services.AddScoped<EmployeeRepository>();
builder.Services.AddScoped<AddressRepository>();
builder.Services.AddScoped<PositionRepository>();
builder.Services.AddScoped<DepartmentRepository>();


//builder.Services.AddScoped<IBaseRepository<Employee>, EmployeeRepository>();
//builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseRouting();
app.UseAuthorization();
app.UseCors();
app.MapControllers();
app.Run();