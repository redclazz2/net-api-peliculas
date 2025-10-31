using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_api_peliculas.Servicios
{
    public class AlmacenadorLocal : IAlmacenadorArchivos
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AlmacenadorLocal(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> Almacenar(string contenedor, IFormFile archivo)
        {
            var extension = Path.GetExtension(archivo.FileName);
            var nombre = $"{Guid.NewGuid()}{extension}";

            string folder = Path.Combine(env.WebRootPath, contenedor);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            
            string ruta = Path.Combine(folder, nombre);
            using (var ms = new MemoryStream())
            {
                await archivo.CopyToAsync(ms);
                var contenido = ms.ToArray();
                await File.WriteAllBytesAsync(ruta, contenido);
            }

            var request = httpContextAccessor.HttpContext!.Request;
            var url = $"{request.Scheme}://{request.Host}";
            var urlArchivo = Path.Combine(url, contenedor, nombre).Replace('\\', '/');

            return urlArchivo;
        }

        public Task Borrar(string ruta, string contenedor)
        {
            if (string.IsNullOrWhiteSpace(ruta))
            {
                return Task.CompletedTask;
            }

            var nombre = Path.GetFileName(ruta);
            var directorioArchivo = Path.Combine(env.WebRootPath, contenedor, nombre);

            if (File.Exists(directorioArchivo))
            {
                File.Delete(directorioArchivo);
            }

            return Task.CompletedTask;
        }
    }
}