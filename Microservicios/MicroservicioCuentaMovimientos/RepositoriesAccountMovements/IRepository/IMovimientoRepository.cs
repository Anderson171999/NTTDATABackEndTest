using MicroservicioCuentaMovimientos.Models;
using System.Linq.Expressions;

namespace MicroservicioClientePersona.RepositoriesClientPerson.IRepositoryClientPerson
{
    public interface IMovimientoRepository
    {
        Task<MOVIMIENTO> Crear(MOVIMIENTO entidad);
        Task<List<MOVIMIENTO>> ObtenerMovimientosPorCuentaYFechas(int cuentaId, string fechaInicio, string fechaFin);

    }
}
