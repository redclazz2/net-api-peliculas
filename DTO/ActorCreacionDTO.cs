using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_api_peliculas.DTO
{
    public class ActorCreacionDTO
    {
        public required string Nombre { get; set; }
        public DateTime FechaNacimiento{ get; set; }
        public IFormFile? Foto { get; set; }
    }
}