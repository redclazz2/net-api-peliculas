using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using net_api_peliculas.DTO;
using net_api_peliculas.Entidades;

namespace net_api_peliculas.Controllers
{
    [Route("api/cines")]
    [ApiController]
    public class CinesController : CustomBaseController
    {
        private const string cacheTag = "cines";
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;

        public CinesController(
            ApplicationDbContext context,
            IMapper mapper,
            IOutputCacheStore outputCacheStore
        ) : base(context, mapper, outputCacheStore, cacheTag)
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
        }

        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<CineDTO>> ObtenerTodosPaginado([FromQuery] PaginacionDTO paginacion)
        {
            return await Get<Cine, CineDTO>(
                paginacion, orden: a => a.Nombre
            );
        }

        [HttpGet("{id:int}", Name = "ObtenerCinePorId")]
        public async Task<ActionResult<CineDTO>> Get(int id)
        {
            return await Get<Cine, CineDTO>(id);
        }

        [HttpPost]
        public async Task<ActionResult<CineCreacionDTO>> Post([FromBody] CineCreacionDTO cineCreacion)
        {
            return await Post<Cine, CineCreacionDTO, CineDTO>(
                cineCreacion, "ObtenerCinePorId"
            );
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] CineCreacionDTO cinceCreacion)
        {
            return await Put<Cine, CineCreacionDTO>(
                id, cinceCreacion
            );
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Cine>(id);
        }
    }
}