using AutoMapper;
using Microservicio.Shared;
using MicroservicioClientePersona.RepositoriesClientPerson.IRepositoryClientPerson;
using MicroservicioClientePersona.RepositoriesClientPerson.RepositoryClientPerson;
using MicroservicioCuentaMovimientos.Models;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace MicroservicioCuentaMovimientos.Controllers
{
    [ApiController]
    [Route("reportes")]
    public class ReporteController : ControllerBase
    {
        private readonly ICuentaRepository _cuentaRepository;
        private readonly IMovimientoRepository _movimientoRepository;
        private readonly HttpClient _httpClient; // Para llamar al microservicio de Persona

        public ReporteController(ICuentaRepository cuentaRepository, IMovimientoRepository movimientoRepository, HttpClient httpClient)
        {
            _cuentaRepository = cuentaRepository;
            _movimientoRepository = movimientoRepository;
            _httpClient = httpClient;
        }


        [HttpGet]
        public async Task<IActionResult> ObtenerReporte([FromQuery] string fechaInicio, [FromQuery] string fechaFin, [FromQuery] string identificacionCliente)
        {
            // Llamar al microservicio de Persona para obtener la persona usando la identificación
            var personaResponse = await _httpClient.GetAsync($"https://localhost:7188/api/Persona/identificacion/{identificacionCliente}");

            if (!personaResponse.IsSuccessStatusCode)
            {
                return NotFound(new { message = "Persona no encontrada" });
            }

            // Leer el contenido de la respuesta del microservicio
            var personaData = await personaResponse.Content.ReadAsStringAsync();
            var personaResponseDTO = JsonConvert.DeserializeObject<ResponseDTO<PersonaDTO>>(personaData); // Deserializa el JSON

            if (!personaResponseDTO.status)
            {
                return NotFound(new { message = "Persona no encontrada" });
            }

            var persona = personaResponseDTO.value; // Obtener el objeto de la persona deserializada

            // Llamar al microservicio de Cliente para obtener el cliente usando el PersonaId
            var clienteResponse = await _httpClient.GetAsync($"https://localhost:7188/api/Cliente/persona/{persona.PersonaId}");

            if (!clienteResponse.IsSuccessStatusCode)
            {
                return NotFound(new { message = "Cliente no encontrado" });
            }

            // Leer el contenido de la respuesta del microservicio
            var clienteData = await clienteResponse.Content.ReadAsStringAsync();
            var clienteResponseDTO = JsonConvert.DeserializeObject<ResponseDTO<ClienteDTO>>(clienteData);

            if (!clienteResponseDTO.status)
            {
                return NotFound(new { message = "Cliente no encontrado" });
            }

            var cliente = clienteResponseDTO.value; // Obtener el objeto de cliente deserializado

            // Ahora busca cuentas usando el ClienteId
            var cuentas = await _cuentaRepository.ObtenerCuentasPorCliente(cliente.ClienteId);

            // Verifica si se encontraron cuentas
            if (cuentas == null || !cuentas.Any())
            {
                return Ok(new ReporteEstadoCuentaDTO
                {
                    Fecha = $"{fechaInicio} - {fechaFin}",
                    Cliente = persona.Identificacion, // Muestra la identificación en el reporte
                    Cuentas = new List<CuentaReporteDTO>() // Lista vacía
                });
            }

            var reporte = new ReporteEstadoCuentaDTO
            {
                Fecha = $"{fechaInicio} - {fechaFin}",
                Cliente = persona.Identificacion, // Muestra la identificación en el reporte
                Cuentas = new List<CuentaReporteDTO>()
            };

            // Agregar detalles de las cuentas al reporte
            foreach (var cuenta in cuentas)
            {
                var movimientos = await _movimientoRepository.ObtenerMovimientosPorCuentaYFechas(cuenta.CuentaId, fechaInicio, fechaFin);

                var cuentaReporte = new CuentaReporteDTO
                {
                    NumeroCuenta = cuenta.NumeroCuenta,
                    TipoCuenta = cuenta.TipoCuenta,
                    SaldoInicial = cuenta.SaldoInicial,
                    Estado = cuenta.Estado,
                    Movimientos = movimientos.Select(m => new MovimientoDTO
                    {
                        Fecha = m.Fecha,
                        TipoMovimiento = m.TipoMovimiento,
                        Valor = m.Valor,
                        Saldo = m.Saldo
                    }).ToList()
                };

                reporte.Cuentas.Add(cuentaReporte);
            }

            return Ok(reporte);
        }


    }
}