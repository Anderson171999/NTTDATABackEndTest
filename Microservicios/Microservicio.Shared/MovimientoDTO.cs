namespace Microservicio.Shared
{
    public class MovimientoDTO
    {
        public string NumeroCuenta { get; set; }  // Cambiado de CuentaId a NumeroCuenta
        public DateTime Fecha { get; set; }
        public string? TipoMovimiento { get; set; } 
        public string Valor { get; set; }
        public decimal? Saldo { get; set; }
    }
}
