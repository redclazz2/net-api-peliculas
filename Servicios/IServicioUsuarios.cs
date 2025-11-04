using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_api_peliculas.Servicios
{
    public interface IServicioUsuarios
    {
        Task<string> ObtenerUsuario();
    }
}