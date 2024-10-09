using MicroservicioCuentaMovimientos.Models;
using System.Linq.Expressions;

namespace MicroservicioClientePersona.RepositoriesClientPerson.IRepositoryClientPerson
{
    public interface ICuentaRepository
    {
        Task<IQueryable<CUENTA>> Consultar(Expression<Func<CUENTA, bool>> filtro = null);
        Task<CUENTA> Crear(CUENTA entidad);
        Task<CUENTA> Obtener(Expression<Func<CUENTA, bool>> filtro = null);
        Task<bool> Editar(CUENTA entidad);
        Task<bool> Eliminar(CUENTA entidad);

        Task<CUENTA> ObtenerPorId(int cuentaId);
        Task ActualizarSaldo(CUENTA cuenta);

        Task<List<CUENTA>> ObtenerCuentasPorCliente(int clienteId);
        Task<CUENTA> ObtenerPorNumeroCuenta(string numeroCuenta);


    }
}
