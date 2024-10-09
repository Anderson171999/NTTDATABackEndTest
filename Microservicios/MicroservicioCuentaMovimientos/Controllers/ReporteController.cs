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
        private readonly HttpClient _httpClient; // Para llamar al microservicio de Cliente

        public ReporteController(ICuentaRepository cuentaRepository, IMovimientoRepository movimientoRepository, HttpClient httpClient)
        {
            _cuentaRepository = cuentaRepository;
            _movimientoRepository = movimientoRepository;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerReporte([FromQuery] string fechaInicio, [FromQuery] string fechaFin, [FromQuery] string identificacionCliente)
        {
            // Llamar al microservicio de Cliente para obtener el cliente
            var clienteResponse = await _httpClient.GetAsync($"https://localhost:7188/api/Cliente/{identificacionCliente}");
            if (!clienteResponse.IsSuccessStatusCode)
            {
                return NotFound(new { message = "Cliente no encontrado" });
            }

            var clienteData = await clienteResponse.Content.ReadAsStringAsync();
            var clienteResponseDTO = JsonConvert.DeserializeObject<ClienteResponseDTO>(clienteData); // Deserializa el JSON

            if (!clienteResponseDTO.Status)
            {
                return NotFound(new { message = "Cliente no encontrado" });
            }

            var cliente = clienteResponseDTO.Value; // Obtener el cliente deserializado

            var cuentas = await _cuentaRepository.ObtenerCuentasPorCliente(cliente.ClienteId);

            // Verifica si se encontraron cuentas
            if (cuentas == null || !cuentas.Any())
            {
                return Ok(new ReporteEstadoCuentaDTO
                {
                    Fecha = $"{fechaInicio} - {fechaFin}",
                    Cliente = cliente.PersonaId.ToString(), 
                    Cuentas = new List<CuentaReporteDTO>() // Lista vacía
                });
            }

            var reporte = new ReporteEstadoCuentaDTO
            {
                Fecha = $"{fechaInicio} - {fechaFin}",
                Cliente = cliente.PersonaId.ToString(), // Cambia esto según lo que desees mostrar
                Cuentas = new List<CuentaReporteDTO>()
            };

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