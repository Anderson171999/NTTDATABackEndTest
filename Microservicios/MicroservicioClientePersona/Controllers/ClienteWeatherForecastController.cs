using Microsoft.AspNetCore.Mvc;

namespace MicroservicioClientePersona.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteWeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ClienteWeatherForecastController> _logger;

        public ClienteWeatherForecastController(ILogger<ClienteWeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecastCliente")]
        public IEnumerable<WeatherForecastCliente> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecastCliente
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
