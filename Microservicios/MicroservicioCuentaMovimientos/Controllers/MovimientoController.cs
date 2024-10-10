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


        [HttpPut]
        [Route("Editar/{id}")]
        public async Task<IActionResult> Editar(int id, [FromBody] MovimientoDTO request)
        {
            ResponseDTO<bool> _ResponseDTO = new ResponseDTO<bool>();
            try
            {
                // Obtener el movimiento existente por id
                MOVIMIENTO _movimientoParaEditar = await _movimientoRepository.Obtener(u => u.MovimientoId == id);

                if (_movimientoParaEditar == null)
                {
                    _ResponseDTO = new ResponseDTO<bool>() { status = false, msg = "No se encontró el movimiento", value = false };
                    return StatusCode(StatusCodes.Status404NotFound, _ResponseDTO);
                }

                // Obtener la cuenta asociada al movimiento
                var cuenta = await _cuentaRepository.ObtenerPorNumeroCuenta(request.NumeroCuenta);
                if (cuenta == null)
                {
                    _ResponseDTO = new ResponseDTO<bool>() { status = false, msg = "Cuenta no encontrada", value = false };
                    return StatusCode(StatusCodes.Status404NotFound, _ResponseDTO);
                }

                // Determinar el tipo de movimiento basado en el valor
                decimal valorMovimiento = Convert.ToDecimal(request.Valor);

                if (valorMovimiento < 0)
                {
                    // Retiro
                    _movimientoParaEditar.TipoMovimiento = "Retiro";

                    // Verificar si hay saldo suficiente para el retiro
                    if (cuenta.SaldoInicial + valorMovimiento < 0)
                    {
                        _ResponseDTO = new ResponseDTO<bool>() { status = false, msg = "Saldo no disponible para el retiro", value = false };
                        return StatusCode(StatusCodes.Status400BadRequest, _ResponseDTO);
                    }
                }
                else
                {
                    // Depósito
                    _movimientoParaEditar.TipoMovimiento = "Depósito";
                }

                // Actualizar el saldo de la cuenta y el saldo del movimiento
                cuenta.SaldoInicial += valorMovimiento; // Actualizar el saldo de la cuenta
                _movimientoParaEditar.Saldo = cuenta.SaldoInicial; // El saldo del movimiento será el saldo actualizado de la cuenta
                _movimientoParaEditar.Valor = request.Valor; // Actualizar el valor del movimiento

                // Guardar los cambios en el movimiento
                bool respuestaMovimiento = await _movimientoRepository.Editar(_movimientoParaEditar);

                if (respuestaMovimiento)
                {
                    // Actualizar el saldo en la tabla Cuenta
                    await _cuentaRepository.ActualizarSaldo(cuenta);

                    // En caso de éxito, asignamos los valores de respuesta adecuados
                    _ResponseDTO = new ResponseDTO<bool>()
                    {
                        status = true,
                        msg = "Movimiento editado correctamente.",
                        value = true
                    };
                }
                else
                {
                    _ResponseDTO = new ResponseDTO<bool>() { status = false, msg = "No se pudo editar el movimiento", value = false };
                }

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new ResponseDTO<bool>() { status = false, msg = ex.Message, value = false };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }


        [HttpDelete]
        [Route("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            ResponseDTO<bool> _ResponseDTO = new ResponseDTO<bool>();
            try
            {
                // Obtener el movimiento que se desea eliminar por su id
                MOVIMIENTO _movimientoParaEliminar = await _movimientoRepository.Obtener(u => u.MovimientoId == id);

                if (_movimientoParaEliminar == null)
                {
                    _ResponseDTO = new ResponseDTO<bool>() { status = false, msg = "No se encontró el movimiento", value = false };
                    return StatusCode(StatusCodes.Status404NotFound, _ResponseDTO);
                }

                // Obtener la cuenta asociada al movimiento
                var cuenta = await _cuentaRepository.Obtener(u => u.CuentaId == _movimientoParaEliminar.CuentaId);
                if (cuenta == null)
                {
                    _ResponseDTO = new ResponseDTO<bool>() { status = false, msg = "Cuenta no encontrada", value = false };
                    return StatusCode(StatusCodes.Status404NotFound, _ResponseDTO);
                }

                // Revertir el saldo de la cuenta. Si fue un depósito, restamos el valor, si fue un retiro, lo sumamos
                decimal valorMovimiento = Convert.ToDecimal(_movimientoParaEliminar.Valor);

                if (_movimientoParaEliminar.TipoMovimiento == "Depósito")
                {
                    cuenta.SaldoInicial -= valorMovimiento; // Restar el valor del depósito
                }
                else if (_movimientoParaEliminar.TipoMovimiento == "Retiro")
                {
                    cuenta.SaldoInicial += valorMovimiento; // Sumar el valor del retiro
                }

                // Eliminar el movimiento de la base de datos
                bool respuestaEliminacion = await _movimientoRepository.Eliminar(_movimientoParaEliminar);

                if (!respuestaEliminacion)
                {
                    _ResponseDTO = new ResponseDTO<bool>() { status = false, msg = "No se pudo eliminar el movimiento", value = false };
                    return StatusCode(StatusCodes.Status400BadRequest, _ResponseDTO);
                }

                // Actualizar el saldo de la cuenta en la base de datos
                await _cuentaRepository.ActualizarSaldo(cuenta);

                _ResponseDTO = new ResponseDTO<bool>()
                {
                    status = true,
                    msg = "Movimiento eliminado correctamente y saldo actualizado.",
                    value = true
                };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new ResponseDTO<bool>() { status = false, msg = ex.Message, value = false };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }

    }
}
