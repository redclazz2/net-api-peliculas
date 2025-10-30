using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using net_api_peliculas.DTO;

namespace net_api_peliculas.Utilidades
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionDTO paginacion)
        {
            return queryable
            .Skip((paginacion.Pagina - 1) * paginacion.RecordsPorPagina)
            .Take(paginacion.RecordsPorPagina);
        }
    }
}