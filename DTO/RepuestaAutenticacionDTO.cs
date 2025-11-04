using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_api_peliculas.DTO
{
    public class RespuestaAutenticacionDTO
    {
        public required string Token { get; set; }
        public DateTime Expiracion { get; set; }
    }
}