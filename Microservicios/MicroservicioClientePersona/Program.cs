using MicroservicioClientePersona.Utilidades;
using Microsoft.EntityFrameworkCore;
using MicroservicioClientePersona.RepositoriesClientPerson;
using MicroservicioClientePersona.RepositoriesClientPerson.IRepositoryClientPerson;
using MicroservicioClientePersona.RepositoriesClientPerson.RepositoryClientPerson;
using MicroservicioClientePersona.Models;

var builder = WebApplication.CreateBuilder(args);

// Configurar la cadena de conexión al DbContext
builder.Services.AddDbContext<DB_ClientePersonaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")));


// Add services to the container.
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
