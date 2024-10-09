namespace Microservicio.Shared
{
    public class MovimientoDTO
    {
        public int CuentaId { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoMovimiento { get; set; }
        public string Valor { get; set; }
        public decimal Saldo { get; set; }
    }
}
