using AutoMapper;
using Microservicio.Shared;
using MicroservicioClientePersona.RepositoriesClientPerson.IRepositoryClientPerson;
using MicroservicioCuentaMovimientos.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicioCuentaMovimientos.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CuentaController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICuentaRepository _cuentaRepository;

        public CuentaController(IMapper mapper, ICuentaRepository cuentaRepository)
        {
            _mapper = mapper;
            _cuentaRepository = cuentaRepository;
        }

        [HttpGet]
        [Route("Listar")]
        public async Task<IActionResult> Lista()
        {
            ResponseDTO<List<CuentaDTO>> _ResponseDTO = new ResponseDTO<List<CuentaDTO>>();

            try
            {
                List<CuentaDTO> ListaCuentas = new List<CuentaDTO>();
                IQueryable<CUENTA> query = await _cuentaRepository.Consultar();

                ListaCuentas = _mapper.Map<List<CuentaDTO>>(query.ToList());

                if (ListaCuentas.Count >= 0)
                    _ResponseDTO = new ResponseDTO<List<CuentaDTO>>() { status = true, msg = "ok", value = ListaCuentas };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new ResponseDTO<List<CuentaDTO>>() { status = false, msg = ex.Message, value = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }

        [HttpPost]
        [Route("Crear")]
        public async Task<IActionResult> Crear([FromBody] CuentaDTO request)
        {
            ResponseDTO<CuentaDTO> _ResponseDTO = new ResponseDTO<CuentaDTO>();
            try
            {
                CUENTA _cuenta = _mapper.Map<CUENTA>(request);

                CUENTA _cuentaCreado = await _cuentaRepository.Crear(_cuenta);

                if (_cuentaCreado.CuentaId != 0)
                    _ResponseDTO = new ResponseDTO<CuentaDTO>() { status = true, msg = "ok", value = _mapper.Map<CuentaDTO>(_cuentaCreado) };
                else
                    _ResponseDTO = new ResponseDTO<CuentaDTO>() { status = false, msg = "No se pudo crear la cuenta" };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new ResponseDTO<CuentaDTO>() { status = false, msg = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }


        [HttpPut]
        [Route("Editar/{id}")]
        public async Task<IActionResult> Editar(int id, [FromBody] CuentaDTO request)
        {
            ResponseDTO<bool> _ResponseDTO = new ResponseDTO<bool>();
            try
            {
                CUENTA _cuentaParaEditar = await _cuentaRepository.Obtener(u => u.CuentaId == id);

                if (_cuentaParaEditar != null)
                {
                    _cuentaParaEditar.ClienteId = request.ClienteId;
                    _cuentaParaEditar.NumeroCuenta = request.NumeroCuenta;
                    _cuentaParaEditar.TipoCuenta = request.TipoCuenta;
                    _cuentaParaEditar.SaldoInicial = request.SaldoInicial;
                    _cuentaParaEditar.Estado = request.Estado;

                    bool respuesta = await _cuentaRepository.Editar(_cuentaParaEditar);

                    if (respuesta)
                        _ResponseDTO = new ResponseDTO<bool>() { status = true, msg = "ok", value = true };
                    else
                        _ResponseDTO = new ResponseDTO<bool>() { status = false, msg = "No se pudo editar la cuenta" };
                }
                else
                {
                    _ResponseDTO = new ResponseDTO<bool>() { status = false, msg = "No se encontró la cuenta" };
                }

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new ResponseDTO<bool>() { status = false, msg = ex.Message };
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
                CUENTA _cuentaParaEliminar = await _cuentaRepository.Obtener(u => u.CuentaId == id);

                if (_cuentaParaEliminar != null)
                {
                    // Llamar al repositorio para eliminar el cliente
                    bool respuesta = await _cuentaRepository.Eliminar(_cuentaParaEliminar);

                    if (respuesta)
                        _ResponseDTO = new ResponseDTO<bool>() { status = true, msg = "Cuenta eliminado correctamente", value = true };
                    else
                        _ResponseDTO = new ResponseDTO<bool>() { status = false, msg = "No se pudo eliminar la cuenta" };
                }
                else
                {
                    _ResponseDTO = new ResponseDTO<bool>() { status = false, msg = "No se encontró la cuenta" };
                }

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new ResponseDTO<bool>() { status = false, msg = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }



    }
}
