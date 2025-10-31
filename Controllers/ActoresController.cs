using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using net_api_peliculas.DTO;
using net_api_peliculas.Entidades;
using net_api_peliculas.Servicios;
using net_api_peliculas.Utilidades;

namespace net_api_peliculas.Controllers
{
    [Route("api/actores")]
    public class ActoresController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private const string cacheTag = "actores";
        private readonly string contenedor = "actores";

        public ActoresController(
            ApplicationDbContext context,
            IMapper mapper,
            IOutputCacheStore outputCacheStore,
            IAlmacenadorArchivos almacenadorArchivos
        )
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<List<ActorDTO>>> ObtenerTodosPaginado ([FromQuery] PaginacionDTO paginacion){
            var queryable = context.Actores;
            await HttpContext.InsertarTotalRegistrosEnCabecera(queryable);
            return await queryable
                .OrderBy(a => a.Nombre)
                .Paginar(paginacion)
                .ProjectTo<ActorDTO>(mapper.ConfigurationProvider).ToListAsync();
        }


        [HttpGet("{id:int}", Name = "ObtenerActorPorId")]
        public async Task<ActionResult<ActorDTO?>> Get(int id)
        {
            var actor = await context.Actores.ProjectTo<ActorDTO>(mapper.ConfigurationProvider).FirstOrDefaultAsync(a => a.Id == id); 
            
            if(actor == null)
                return NotFound();

            return actor;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actorCreacion)
        {
            var actor = mapper.Map<Actor>(actorCreacion);

            if (actorCreacion.Foto != null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, actorCreacion.Foto);
                actor.Foto = url;
            }

            context.Add(actor);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);

            return CreatedAtRoute("ObtenerActorPorId", new { id = actor.Id }, actor);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ActorCreacionDTO actorUpdate)
        {
            var actor = await context.Actores.FirstOrDefaultAsync(a => a.Id == id);

            if (actor is null)
                return NotFound();

            actor = mapper.Map(actorUpdate, actor);

            if (actorUpdate.Foto is not null)
                actor.Foto = await almacenadorArchivos.Editar(actor.Foto!, contenedor, actorUpdate.Foto!);

            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var borrados = await context.Actores.Where(a => a.Id == id).ExecuteDeleteAsync();
            
            if (borrados == 0)
                return NotFound();

            await outputCacheStore.EvictByTagAsync(tag: cacheTag, default);
            
            return NoContent();
        }
    }
}