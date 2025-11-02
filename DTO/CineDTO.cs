using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using net_api_peliculas.Entidades;
using NetTopologySuite.Geometries;

namespace net_api_peliculas.DTO
{
    public class CineDTO : IId
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required double Latitud { get; set; }
        public required double Longitud{ get; set; }
    }
}