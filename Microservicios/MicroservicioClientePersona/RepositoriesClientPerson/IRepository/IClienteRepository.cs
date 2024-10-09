using Microservicio.Shared;
using MicroservicioClientePersona.Models;
using System.Linq.Expressions;

namespace MicroservicioClientePersona.RepositoriesClientPerson.IRepositoryClientPerson
{
    public interface IClienteRepository
    {
        Task<IQueryable<CLIENTE>> Consultar(Expression<Func<CLIENTE, bool>> filtro = null);
        Task<CLIENTE> Crear(CLIENTE entidad);
        Task<CLIENTE> Obtener(Expression<Func<CLIENTE, bool>> filtro = null);
        Task<bool> Editar(CLIENTE entidad);
        Task<bool> Eliminar(CLIENTE entidad);
        Task<CLIENTE> ObtenerClientePorPersonaId(int personaId);
    }
}
