using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using net_api_peliculas.Entidades;

namespace net_api_peliculas.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GenerosController : Controller
    {
        public IOutputCacheStore OutputCacheStore { get; }
        private const string CacheTag = "Genero";

        public GenerosController(
            IOutputCacheStore outputCacheStore
        )
        {
            OutputCacheStore = outputCacheStore;
        }

        [HttpGet]
        [OutputCache(Tags = [CacheTag])]
        public ActionResult<List<Genero>> GetGeneros()
        {
            return new List<Genero>
            {
                new() {Id = 1, Nombre = "Comedia"},
                new() {Id = 2, Nombre = "Acción"}
            };
        }

        [HttpGet("{id}")]
        public ActionResult<Genero> GetGenero(int id)
        {
            throw new NotImplementedException();            
        }

        [HttpPost]
        public async Task<ActionResult> Crear([FromBody] Genero genero)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public void Put()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}