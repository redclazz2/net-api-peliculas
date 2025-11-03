using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_api_peliculas.DTO
{
    public class PeliculasPostGetDTO
    {
        public List<GeneroDTO> Generos { get; set; } = [];
        public List<CineDTO> Cines { get; set; } = [];
    }
}