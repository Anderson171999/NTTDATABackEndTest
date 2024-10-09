using MicroservicioClientePersona.Models;
using MicroservicioClientePersona.RepositoriesClientPerson.IRepositoryClientPerson;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MicroservicioClientePersona.RepositoriesClientPerson.RepositoryClientPerson
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly DB_ClientePersonaContext _context;

        public PersonaRepository(DB_ClientePersonaContext context)
        {
            _context = context;
        }

        public async Task<PERSONA> Obtener(Expression<Func<PERSONA, bool>> filtro = null)
        {
            try
            {
                return await _context.PERSONA.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

    }
}
