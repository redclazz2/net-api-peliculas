using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace net_api_peliculas.DTO
{
    public class CineCreacionDTO
    {
        [Required]
        [StringLength(75)]
        public required string Nombre { get; set; }
        [Range(-90, 90)]
        public required double Latitud { get; set; }
        [Range(-180,180)]
        public required double Longitud { get; set; }
    }
}