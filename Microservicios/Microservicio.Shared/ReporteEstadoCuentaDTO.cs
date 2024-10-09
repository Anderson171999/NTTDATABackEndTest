namespace Microservicio.Shared
{
    public class ReporteEstadoCuentaDTO
    {
        public string Fecha { get; set; }
        public string Cliente { get; set; }
        public List<CuentaReporteDTO> Cuentas { get; set; }
    }
}
