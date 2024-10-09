using MicroservicioClientePersona.RepositoriesClientPerson.IRepositoryClientPerson;
using MicroservicioCuentaMovimientos.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MicroservicioClientePersona.RepositoriesClientPerson.RepositoryClientPerson
{
    public class CuentaRepository : ICuentaRepository
    {
        private readonly DB_CuentaMovimientoContext _context;

        public CuentaRepository(DB_CuentaMovimientoContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<CUENTA>> Consultar(Expression<Func<CUENTA, bool>> filtro = null)
        {
            IQueryable<CUENTA> queryEntidad = filtro == null ? _context.CUENTA : _context.CUENTA.Where(filtro);
            return queryEntidad;
        }

        public async Task<CUENTA> Crear(CUENTA entidad)
        {
            try
            {
                _context.Set<CUENTA>().Add(entidad);
                await _context.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }


        public async Task<CUENTA> Obtener(Expression<Func<CUENTA, bool>> filtro = null)
        {
            try
            {
                return await _context.CUENTA.Where(filtro).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }


        public async Task<bool> Editar(CUENTA entidad)
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


        public async Task<bool> Eliminar(CUENTA entidad)
        {
            try
            {
                _context.CUENTA.Remove(entidad);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }


        public async Task<CUENTA> ObtenerPorId(int cuentaId)
        {
            return await _context.Set<CUENTA>().FindAsync(cuentaId);
        }

        public async Task ActualizarSaldo(CUENTA cuenta)
        {
            _context.Entry(cuenta).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<CUENTA>> ObtenerCuentasPorCliente(int clienteId)
        {
            return await _context.CUENTA
                .Where(c => c.ClienteId == clienteId)
                .ToListAsync();
        }
        public async Task<CUENTA> ObtenerPorNumeroCuenta(string numeroCuenta)
        {
            return await _context.CUENTA.FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);
        }




    }
}
