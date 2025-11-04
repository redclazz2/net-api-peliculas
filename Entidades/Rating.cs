using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace net_api_peliculas.Entidades
{
    public class Rating
    {
        public int Id { get; set; }
        public int Puntuacion { get; set; }
        public int PeliculaId { get; set; }
        public required string UsuarioId { get; set; }
        public Pelicula Pelicula { get; set; } = null!;
        public IdentityUser Usuario { get; set; } = null!;
    }
}