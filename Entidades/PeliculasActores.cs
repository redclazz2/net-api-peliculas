using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace net_api_peliculas.Entidades
{
    public class PeliculasActores
    {
        public int ActorId { get; set; }
        public int PeliculaId { get; set; }
        [StringLength(100)]
        public required string Personaje { get; set; }
        public int Orden { get; set; }
        public Actor Actor { get; set; } = null!;
        public Peliculas Peliculas { get; set; } = null!;
    }
}