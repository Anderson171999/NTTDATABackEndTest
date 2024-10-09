namespace Microservicio.Shared
{
    public class CuentaReporteDTO
    {
        public string NumeroCuenta { get; set; }
        public string TipoCuenta { get; set; }
        public decimal SaldoInicial { get; set; }
        public bool Estado { get; set; }
        public List<MovimientoDTO> Movimientos { get; set; }
    }
}
