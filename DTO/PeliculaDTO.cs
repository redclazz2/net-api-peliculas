namespace net_api_peliculas.DTO
{
    public class PeliculaDTO
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public string? Trailer { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        public string? Poster { get; set; }

        public double PromedioVoto { get; set; }
        public double VotoUsuario { get; set; }
    }
}