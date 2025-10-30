using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_api_peliculas.DTO
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int cantidadMaximaRecordsPorPagina = 50;
        private int recordsPorPagina = 10;
        public int RecordsPorPagina { get { return recordsPorPagina; } set
            {
                recordsPorPagina = (value > cantidadMaximaRecordsPorPagina) ? cantidadMaximaRecordsPorPagina : value;
            } }

    }
}