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
        private readonly ICuentaRepository _cuentaRepository;  // Repositorio para actualizar la cuenta

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
                // Obtener la cuenta asociada
                var cuenta = await _cuentaRepository.ObtenerPorId(request.CuentaId);
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

                // Mapear el DTO a la entidad MOVIMIENTO
                MOVIMIENTO movimiento = _mapper.Map<MOVIMIENTO>(request);

                // Crear el movimiento en la base de datos
                MOVIMIENTO movimientoCreado = await _movimientoRepository.Crear(movimiento);

                // Actualizar el saldo en la tabla Cuenta
                await _cuentaRepository.ActualizarSaldo(cuenta);

                _ResponseDTO = new ResponseDTO<MovimientoDTO>()
                {
                    status = true,
                    msg = "Movimiento creado con éxito",
                    value = _mapper.Map<MovimientoDTO>(movimientoCreado)
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
