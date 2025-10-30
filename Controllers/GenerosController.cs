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
        public ApplicationDbContext context { get; }

        public GenerosController(
            IOutputCacheStore outputCacheStore,
            ApplicationDbContext applicationDbContext
        )
        {
            context = applicationDbContext;
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

        [HttpGet("{id}", Name = "ObtenerGeneroPorId")]
        public ActionResult<Genero?> GetGenero(int id)
        {
            var result = context.Generos.Where((g) => g.Id == id).FirstOrDefault();
            if (result == null)
                return NotFound();

            return result;
        }

        [HttpPost]
        public async Task<ActionResult> Crear([FromBody] Genero genero)
        {
            context.Add(genero);
            await context.SaveChangesAsync();

            return CreatedAtRoute("ObtenerGeneroPorId", new {id = genero.Id}, genero);
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