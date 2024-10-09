using Microservicio.Shared;
using MicroservicioClientePersona.Models;
using MicroservicioClientePersona.RepositoriesClientPerson.IRepositoryClientPerson;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MicroservicioClientePersona.RepositoriesClientPerson.RepositoryClientPerson
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly DB_ClientePersonaContext _context;

        public ClienteRepository(DB_ClientePersonaContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<CLIENTE>> Consultar(Expression<Func<CLIENTE, bool>> filtro = null)
        {
            IQueryable<CLIENTE> queryEntidad = filtro == null ? _context.CLIENTE : _context.CLIENTE.Where(filtro);
            return queryEntidad;
        }

        public async Task<CLIENTE> Crear(CLIENTE entidad)
        {
            try
            {
                _context.Set<CLIENTE>().Add(entidad);
                await _context.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }


        public async Task<CLIENTE> Obtener(Expression<Func<CLIENTE, bool>> filtro = null)
        {
            try
            {
                return await _context.CLIENTE.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }


        public async Task<bool> Editar(CLIENTE entidad)
        {
            try
            {
                _context.Update(entidad);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }


        public async Task<bool> Eliminar(CLIENTE entidad)
        {
            try
            {
                _context.CLIENTE.Remove(entidad);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }



        public async Task<CLIENTE> ObtenerClientePorPersonaId(int personaId)
        {
            return await _context.CLIENTE.FirstOrDefaultAsync(c => c.PersonaId == personaId);
        }

    }
}
