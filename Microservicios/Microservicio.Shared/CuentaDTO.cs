namespace Microservicio.Shared
{
    public class CuentaDTO
    {

        public int ClienteId { get; set; }
        public string NumeroCuenta { get; set; }
        public string TipoCuenta { get; set; }
        public decimal SaldoInicial { get; set; }
        public bool Estado { get; set; }
    }
}
