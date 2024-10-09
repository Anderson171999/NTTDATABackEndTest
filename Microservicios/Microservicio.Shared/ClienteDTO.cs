using System.Text.Json.Serialization;

namespace Microservicio.Shared
{
    public class ClienteDTO
    {
        public int ClienteId { get; set; }
        public int PersonaId { get; set; }
        public string Clave { get; set; }
        public bool Estado { get; set; }

    }
}
