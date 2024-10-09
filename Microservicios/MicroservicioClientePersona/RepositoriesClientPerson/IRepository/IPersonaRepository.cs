using Microservicio.Shared;
using MicroservicioClientePersona.Models;
using System.Linq.Expressions;

namespace MicroservicioClientePersona.RepositoriesClientPerson.IRepositoryClientPerson
{
    public interface IPersonaRepository
    {
        Task<PERSONA> Obtener(Expression<Func<PERSONA, bool>> filtro = null);

    }
}
