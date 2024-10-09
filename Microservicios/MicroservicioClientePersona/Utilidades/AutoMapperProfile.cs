using AutoMapper;
using MicroservicioClientePersona.Models;
using Microservicio.Shared;
using System.Globalization;

namespace MicroservicioClientePersona.Utilidades
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Cliente
            CreateMap<CLIENTE, ClienteDTO>().ReverseMap();

            CreateMap<CLIENTE, ClienteDTO>()
                .ForMember(destino =>
                    destino.Clave,
                    opt => opt.MapFrom(origen => origen.Clave)
                    )
                .ForMember(destino =>
                    destino.PersonaId,
                    opt => opt.MapFrom(origen => origen.PersonaId)
                    );
            #endregion Cliente

            #region Persona
            CreateMap<PERSONA, PersonaDTO>().ReverseMap();
            #endregion Persona

        }
    }
}
