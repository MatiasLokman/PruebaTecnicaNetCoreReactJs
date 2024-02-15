using API.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Reflection;
using FluentValidation;

using API.Services.ClienteServices.Queries.GetClienteByIdQuery;
using API.Services.ClienteServices.Commands.CreateClienteCommand;
using API.Services.ClienteServices.Commands.UpdateClienteCommand;
using API.Services.ClienteServices.Commands.UpdateEstadoClienteCommand;

using API.Services.CuentaServices.Queries.GetCuentasByIdQuery;
using API.Services.CuentaServices.Commands.CreateCuentaCommand;
using API.Services.CuentaServices.Commands.CreateTransferenciaCuentaCommand;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services.AddCors();
// Add services to the container.

builder.Services.AddDbContext<ControlGlobalContext>(options =>
 options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddMediatR(opt =>
{
  opt.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

// Validaciones para CRUD (Clientes)
builder.Services.AddScoped<IValidator<GetClienteByIdQuery>, GetClienteByIdQueryValidator>();
builder.Services.AddScoped<IValidator<CreateClienteCommand>, CreateClienteCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateClienteCommand>, UpdateClienteCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateEstadoClienteCommand>, UpdateEstadoClienteCommandValidator>();

// Validaciones para CRUD (Cuentas)
builder.Services.AddScoped<IValidator<CreateCuentaCommand>, CreateCuentaCommandValidator>();
builder.Services.AddScoped<IValidator<CreateTransferenciaCuentaCommand>, CreateTransferenciaCuentaCommandValidator>();
builder.Services.AddScoped<IValidator<GetCuentasByIdQuery>, GetCuentasByIdQueryValidator>();

builder.Services.AddControllers();

// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// swagger
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors(c =>
{
  c.AllowAnyHeader();
  c.AllowAnyMethod();
  c.WithOrigins("http://localhost:3000");
  c.AllowCredentials();
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<CuentaHub>("/cuentaHub");

app.Run();