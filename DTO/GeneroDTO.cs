using net_api_peliculas.Entidades;

namespace net_api_peliculas.DTO
{
    public class GeneroDTO : IId
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
    }
}