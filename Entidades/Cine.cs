using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace net_api_peliculas.Entidades
{
    public class Cine
    {
        public int Id { get; set; }
        [Required]
        [StringLength(75)]
        public required string Nombre { get; set; }
        public required Point Ubicacion { get; set; }
    }
}