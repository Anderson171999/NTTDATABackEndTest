using MicroservicioClientePersona.Models;
using Microsoft.AspNetCore.Mvc;
using Microservicio.Shared;
using AutoMapper;
using MicroservicioClientePersona.RepositoriesClientPerson.IRepositoryClientPerson;

namespace MicroservicioClientePersona.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonaController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPersonaRepository _personaRepository;

        public PersonaController(IMapper mapper, IPersonaRepository personaRepository)
        {
            _mapper = mapper;
            _personaRepository = personaRepository;
        }
        [HttpGet]
        [Route("identificacion/{identificacion}")]
        public async Task<IActionResult> ObtenerPorIdentificacion(string identificacion)
        {
            ResponseDTO<PersonaDTO> _ResponseDTO = new ResponseDTO<PersonaDTO>();
            try
            {
                // Busca la persona por identificación
                PERSONA persona = await _personaRepository.Obtener(u => u.Identificacion == identificacion);

                if (persona != null)
                {
                    _ResponseDTO = new ResponseDTO<PersonaDTO>
                    {
                        status = true,
                        msg = "Persona encontrada",
                        value = _mapper.Map<PersonaDTO>(persona)
                    };
                    return StatusCode(StatusCodes.Status200OK, _ResponseDTO);
                }
                else
                {
                    _ResponseDTO = new ResponseDTO<PersonaDTO> { status = false, msg = "Persona no encontrada" };
                    return StatusCode(StatusCodes.Status404NotFound, _ResponseDTO);
                }
            }
            catch (Exception ex)
            {
                _ResponseDTO = new ResponseDTO<PersonaDTO> { status = false, msg = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, _ResponseDTO);
            }
        }
    }
}
