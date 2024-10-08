using AutoMapper;
using MicroservicioCuentaMovimientos.Models;
using Microservicio.Shared;
using System.Globalization;

namespace MicroservicioClientePersona.Utilidades
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Cuenta
            CreateMap<CUENTA, CuentaDTO>().ReverseMap();

            CreateMap<CUENTA, CuentaDTO>()
                .ForMember(destino =>
                    destino.ClienteId,
                    opt => opt.MapFrom(origen => origen.ClienteId)
                    );
            #endregion Cuenta


        }
    }
}
