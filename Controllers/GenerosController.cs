using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using net_api_peliculas.DTO;
using net_api_peliculas.Entidades;
using net_api_peliculas.Utilidades;

namespace net_api_peliculas.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GenerosController : Controller
    {
        public IOutputCacheStore OutputCacheStore { get; }
        private const string CacheTag = "Genero";
        public ApplicationDbContext context { get; }
        public IMapper Mapper { get; }

        public GenerosController(
            IOutputCacheStore outputCacheStore,
            ApplicationDbContext applicationDbContext,
            IMapper mapper
        )
        {
            context = applicationDbContext;
            Mapper = mapper;
            OutputCacheStore = outputCacheStore;
        }

        [HttpGet]
        [OutputCache(Tags = [CacheTag])]
        public async Task<ActionResult<List<GeneroDTO>>> GetGeneros([FromQuery] PaginacionDTO paginacion)
        {
            var queryable = context.Generos;
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            return await queryable.
            OrderBy(g => g.Nombre).
            Paginar(paginacion).
            ProjectTo<GeneroDTO>(Mapper.ConfigurationProvider).ToListAsync();
        }

        [HttpGet("{id}", Name = "ObtenerGeneroPorId")]
        public async Task<ActionResult<GeneroDTO?>> GetGenero(int id)
        {
            var result = await context.Generos.Where((g) => g.Id == id).FirstOrDefaultAsync();

            if (result == null)
                return NotFound();

            return Mapper.Map<GeneroDTO>(result);
        }

        [HttpPost]
        public async Task<ActionResult> Crear([FromBody] GeneroCreacionDTO generoDTO)
        {
            var genero = Mapper.Map<Genero>(generoDTO);

            context.Add(genero);
            await context.SaveChangesAsync();

            await OutputCacheStore.EvictByTagAsync(tag: CacheTag, default);

            return CreatedAtRoute("ObtenerGeneroPorId", new { id = genero.Id }, genero);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var generoExiste = await context.Generos.AnyAsync((g) => g.Id == id);

            if (!generoExiste)
                return NotFound();

            var genero = Mapper.Map<Genero>(generoCreacionDTO);
            genero.Id = id;
            context.Update(genero);
            await context.SaveChangesAsync();
            await OutputCacheStore.EvictByTagAsync(tag: CacheTag, default);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var registrosBorrados = await context.Generos.Where((g) => g.Id == id).ExecuteDeleteAsync();

            if (registrosBorrados == 0)
            {
                return NotFound();
            }

            await OutputCacheStore.EvictByTagAsync(tag: CacheTag, default);

            return NoContent();
        }
    }
}