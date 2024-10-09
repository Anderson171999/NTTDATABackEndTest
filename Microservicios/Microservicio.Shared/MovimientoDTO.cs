using System.Text.Json.Serialization;

namespace Microservicio.Shared
{
    public class MovimientoDTO
    {
        //[JsonIgnore]
        public string NumeroCuenta { get; set; }
        public DateTime Fecha { get; set; }
        public string? TipoMovimiento { get; set; } 
        public string Valor { get; set; }
        public decimal? Saldo { get; set; }
    }
}
