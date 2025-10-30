using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using net_api_peliculas.DTO;
using net_api_peliculas.Entidades;

namespace net_api_peliculas.Controllers
{
    [Route("api/actores")]
    public class ActoresController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private readonly string cacheTag = "actores";

        public ActoresController(
            ApplicationDbContext context,
            IMapper mapper,
            IOutputCacheStore outputCacheStore
        )
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
        }

        [HttpGet("{id:int}", Name = "ObtenerActorPorId")]
        public async Task<ActionResult> Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actorCreacion)
        {
            var actor = mapper.Map<Actor>(actorCreacion);


            context.Add(actor);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);

            return CreatedAtRoute("ObtenerActorPorId", new { id = actor.Id }, actor);
        }
    }
}