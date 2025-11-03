using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace net_api_peliculas.Entidades
{
    public class Pelicula : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public required string Titulo { get; set; }
        public string? Trailer { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        [Unicode(false)]
        public string? Poster { get; set; }

        public List<PeliculasGeneros> PeliculasGeneros { get; set; } = [];
        public List<PeliculasCines> PeliculasCines { get; set; } = [];
        public List<PeliculasActores> PeliculasActores { get; set; } = [];
    }
}