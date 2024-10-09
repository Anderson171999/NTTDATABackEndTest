using MicroservicioClientePersona.Models;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Shared;
using AutoMapper;
using MicroservicioClientePersona.RepositoriesClientPerson.IRepositoryClientPerson;

namespace MicroservicioClientePersona.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IClienteRepository _clienteRepository;

        public ClienteController(IMapper mapper, IClienteRepository clienteRepository)
        {
            _mapper = mapper;
            _clienteRepository = clienteRepository;
        }

        [HttpGet]
        [Route("Listar")]
        public async Task<IActionResult> Lista()
        {
            ResponseDTO<List<ClienteDTO>> _ResponseDTO = new ResponseDTO<List<ClienteDTO>>();

            try
            {
                List<ClienteDTO> ListaClientes = new List<ClienteDTO>();
                IQueryable<CLIENTE> query = await _clienteRepository.Consultar();

                ListaClientes = _mapper.Map<List<ClienteDTO>>(query.ToList());

                if (ListaClientes.Count >= 0)
                    _ResponseDTO = new ResponseDTO<List<ClienteDTO>>() { status = true, msg = "ok", value = ListaClientes };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new ResponseDTO<List<ClienteDTO>>() { status = false, msg = ex.Message, value = null };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }

        [HttpPost]
        [Route("Crear")]
        public async Task<IActionResult> Crear([FromBody] ClienteDTO request)
        {
            ResponseDTO<ClienteDTO> _ResponseDTO = new ResponseDTO<ClienteDTO>();
            try
            {
                CLIENTE _cliente = _mapper.Map<CLIENTE>(request);

                CLIENTE _clienteCreado = await _clienteRepository.Crear(_cliente);

                if (_clienteCreado.ClienteId != 0)
                    _ResponseDTO = new ResponseDTO<ClienteDTO>() { status = true, msg = "ok", value = _mapper.Map<ClienteDTO>(_clienteCreado) };
                else
                    _ResponseDTO = new ResponseDTO<ClienteDTO>() { status = false, msg = "No se pudo crear el cliente" };

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new ResponseDTO<ClienteDTO>() { status = false, msg = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }


        [HttpPut]
        [Route("Editar/{id}")]
        public async Task<IActionResult> Editar(int id, [FromBody] ClienteDTO request)
        {
            ResponseDTO<bool> _ResponseDTO = new ResponseDTO<bool>();
            try
            {
                CLIENTE _clienteParaEditar = await _clienteRepository.Obtener(u => u.ClienteId == id);

                if (_clienteParaEditar != null)
                {
                    _clienteParaEditar.PersonaId = request.PersonaId;
                    _clienteParaEditar.Clave = request.Clave;
                    _clienteParaEditar.Estado = request.Estado;

                    bool respuesta = await _clienteRepository.Editar(_clienteParaEditar);

                    if (respuesta)
                        _ResponseDTO = new ResponseDTO<bool>() { status = true, msg = "ok", value = true };
                    else
                        _ResponseDTO = new ResponseDTO<bool>() { status = false, msg = "No se pudo editar el cliente" };
                }
                else
                {
                    _ResponseDTO = new ResponseDTO<bool>() { status = false, msg = "No se encontró el cliente" };
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
                // Buscar al cliente a eliminar por su ID
                CLIENTE _clienteParaEliminar = await _clienteRepository.Obtener(u => u.ClienteId == id);

                if (_clienteParaEliminar != null)
                {
                    // Llamar al repositorio para eliminar el cliente
                    bool respuesta = await _clienteRepository.Eliminar(_clienteParaEliminar);

                    if (respuesta)
                        _ResponseDTO = new ResponseDTO<bool>() { status = true, msg = "Cliente eliminado correctamente", value = true };
                    else
                        _ResponseDTO = new ResponseDTO<bool>() { status = false, msg = "No se pudo eliminar el cliente" };
                }
                else
                {
                    _ResponseDTO = new ResponseDTO<bool>() { status = false, msg = "No se encontró el cliente" };
                }

                return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
            }
            catch (Exception ex)
            {
                _ResponseDTO = new ResponseDTO<bool>() { status = false, msg = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            ResponseDTO<ClienteDTO> _ResponseDTO = new ResponseDTO<ClienteDTO>();
            try
            {
                CLIENTE cliente = await _clienteRepository.Obtener(u => u.ClienteId == id);

                if (cliente != null)
                {
                    _ResponseDTO = new ResponseDTO<ClienteDTO>
                    {
                        status = true,
                        msg = "Cliente encontrado",
                        value = _mapper.Map<ClienteDTO>(cliente)
                    };
                    return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
                }
                else
                {
                    _ResponseDTO = new ResponseDTO<ClienteDTO> { status = false, msg = "Cliente no encontrado" };
                    return StatusCode(StatusCodes.Status404NotFound, _ResponseDTO);
                }
            }
            catch (Exception ex)
            {
                _ResponseDTO = new ResponseDTO<ClienteDTO> { status = false, msg = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }


        [HttpGet("persona/{personaId}")]
        public async Task<IActionResult> ObtenerPorPersonaId(int personaId)
        {
            ResponseDTO<ClienteDTO> _ResponseDTO = new ResponseDTO<ClienteDTO>();

            try
            {
                // Usamos el método que ya tienes para buscar el cliente por PersonaId
                CLIENTE cliente = await _clienteRepository.ObtenerClientePorPersonaId(personaId);

                if (cliente != null)
                {
                    _ResponseDTO = new ResponseDTO<ClienteDTO>
                    {
                        status = true,
                        msg = "Cliente encontrado",
                        value = _mapper.Map<ClienteDTO>(cliente)
                    };
                    return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
                }
                else
                {
                    _ResponseDTO = new ResponseDTO<ClienteDTO>
                    {
                        status = false,
                        msg = "Cliente no encontrado"
                    };
                    return StatusCode(StatusCodes.Status404NotFound, _ResponseDTO);
                }
            }
            catch (Exception ex)
            {
                _ResponseDTO = new ResponseDTO<ClienteDTO>
                {
                    status = false,
                    msg = ex.Message
                };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }




    }

}
