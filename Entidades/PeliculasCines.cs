using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_api_peliculas.Entidades
{
    public class PeliculasCines
    {
        public int CineId { get; set; }
        public int PeliculaId { get; set; }
        public Cine Cine { get; set; } = null!;
        public Peliculas Peliculas { get; set; } = null!;
    }
}