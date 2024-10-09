using AutoMapper;
using Microservicio.Shared;
using MicroservicioClientePersona.RepositoriesClientPerson.IRepositoryClientPerson;
using MicroservicioCuentaMovimientos.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicioCuentaMovimientos.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovimientoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMovimientoRepository _movimientoRepository;
        private readonly ICuentaRepository _cuentaRepository;

        public MovimientoController(IMapper mapper, IMovimientoRepository movimientoRepository, ICuentaRepository cuentaRepository)
        {
            _mapper = mapper;
            _movimientoRepository = movimientoRepository;
            _cuentaRepository = cuentaRepository;
        }


        [HttpPost]
        [Route("Crear")]
        public async Task<IActionResult> Crear([FromBody] MovimientoDTO request)
        {
            ResponseDTO<MovimientoDTO> _ResponseDTO = new ResponseDTO<MovimientoDTO>();

            try
            {
                // Obtener la cuenta asociada por NumeroCuenta
                var cuenta = await _cuentaRepository.ObtenerPorNumeroCuenta(request.NumeroCuenta);
                if (cuenta == null)
                {
                    _ResponseDTO = new ResponseDTO<MovimientoDTO>() { status = false, msg = "Cuenta no encontrada" };
                    return StatusCode(StatusCodes.Status404NotFound, _ResponseDTO);
                }

                // Determinar si es un depósito o retiro basado en el valor
                decimal valorMovimiento = Convert.ToDecimal(request.Valor);

                if (valorMovimiento < 0)
                {
                    request.TipoMovimiento = "Retiro";

                    // Verificar si hay saldo suficiente para el retiro
                    if (cuenta.SaldoInicial + valorMovimiento < 0)
                    {
                        _ResponseDTO = new ResponseDTO<MovimientoDTO>() { status = false, msg = "Saldo no disponible" };
                        return StatusCode(StatusCodes.Status400BadRequest, _ResponseDTO);
                    }
                }
                else
                {
                    request.TipoMovimiento = "Depósito";
                }

                // Actualizar el saldo de la cuenta y el saldo del movimiento
                cuenta.SaldoInicial += valorMovimiento;
                request.Saldo = cuenta.SaldoInicial;  // El saldo del movimiento será el saldo actualizado de la cuenta

                // Asignar la fecha actual si no fue enviada
                if (request.Fecha == default(DateTime))
                {
                    request.Fecha = DateTime.Now;
                }

                // Mapear el DTO a la entidad MOVIMIENTO
                MOVIMIENTO movimiento = _mapper.Map<MOVIMIENTO>(request);

                // Asignar CuentaId en el movimiento mapeado
                movimiento.CuentaId = cuenta.CuentaId;

                // Crear el movimiento en la base de datos
                MOVIMIENTO movimientoCreado = await _movimientoRepository.Crear(movimiento);

                // Actualizar el saldo en la tabla Cuenta
                await _cuentaRepository.ActualizarSaldo(cuenta);

                // Mapear de nuevo el movimiento creado al DTO para la respuesta
                var movimientoDTO = _mapper.Map<MovimientoDTO>(movimientoCreado);

                // Asignar el NumeroCuenta a la respuesta
                movimientoDTO.NumeroCuenta = cuenta.NumeroCuenta;  // Asegurar que el NumeroCuenta esté presente en la respuesta

                _ResponseDTO = new ResponseDTO<MovimientoDTO>()
                {
                    status = true,
                    msg = "Movimiento creado con éxito",
                    value = movimientoDTO
                };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new ResponseDTO<MovimientoDTO>() { status = false, msg = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }


    }
}
