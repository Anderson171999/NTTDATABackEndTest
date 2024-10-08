using MicroservicioClientePersona.RepositoriesClientPerson.IRepositoryClientPerson;
using MicroservicioClientePersona.RepositoriesClientPerson.RepositoryClientPerson;
using MicroservicioClientePersona.Utilidades;
using MicroservicioCuentaMovimientos.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configurar la cadena de conexión al DbContext
builder.Services.AddDbContext<DB_CuentaMovimientoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")));


// Add services to the container.
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddScoped<ICuentaRepository, CuentaRepository>();



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
