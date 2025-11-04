using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace net_api_peliculas.DTO
{
    public class RatingCreacionDTO
    {
        public int PeliculaId { get; set; }
        [Range(1,5)]
        public int Puntuacion { get; set; }
    }
}