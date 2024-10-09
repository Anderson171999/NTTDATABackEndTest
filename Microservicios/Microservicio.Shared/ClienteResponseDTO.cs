namespace Microservicio.Shared
{
    public class ClienteResponseDTO
    {
        public bool Status { get; set; }
        public string Msg { get; set; }
        public ClienteDTO Value { get; set; }
    }

}
