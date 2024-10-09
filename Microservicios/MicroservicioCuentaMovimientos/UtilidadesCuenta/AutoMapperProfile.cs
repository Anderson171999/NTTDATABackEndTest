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
            #endregion Cuenta

            #region Movimiento
            CreateMap<MOVIMIENTO, MovimientoDTO>().ReverseMap();
            #endregion Movimiento

        }
    }
}
