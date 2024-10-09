using MicroservicioClientePersona.RepositoriesClientPerson.IRepositoryClientPerson;
using MicroservicioCuentaMovimientos.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MicroservicioClientePersona.RepositoriesClientPerson.RepositoryClientPerson
{
    public class MovimientoRepository : IMovimientoRepository
    {
        private readonly DB_CuentaMovimientoContext _context;

        public MovimientoRepository(DB_CuentaMovimientoContext context)
        {
            _context = context;
        }


        public async Task<MOVIMIENTO> Crear(MOVIMIENTO entidad)
        {
            try
            {
                _context.Set<MOVIMIENTO>().Add(entidad);
                await _context.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }


        public async Task<List<MOVIMIENTO>> ObtenerMovimientosPorCuentaYFechas(int cuentaId, string fechaInicio, string fechaFin)
        {
            var inicio = DateTime.Parse(fechaInicio);
            var fin = DateTime.Parse(fechaFin);

            return await _context.MOVIMIENTO
                .Where(m => m.CuentaId == cuentaId && m.Fecha >= inicio && m.Fecha <= fin) // Filtramos por cuenta y rango de fechas
                .ToListAsync();
        }


    }
}
