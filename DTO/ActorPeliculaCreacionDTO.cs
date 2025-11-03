using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_api_peliculas.DTO
{
    public class ActorPeliculaCreacionDTO
    {
        public int Id { get; set; }
        public required string Personaje { get; set; }
    }
}