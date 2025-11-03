using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using net_api_peliculas.DTO;
using net_api_peliculas.Entidades;

namespace net_api_peliculas.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GenerosController : CustomBaseController
    {
        public IOutputCacheStore OutputCacheStore { get; }
        private const string cacheTag = "Genero";
        public ApplicationDbContext context { get; }
        public IMapper Mapper { get; }

        public GenerosController(
            IOutputCacheStore outputCacheStore,
            ApplicationDbContext applicationDbContext,
            IMapper mapper
        ) : base(applicationDbContext, mapper, outputCacheStore,cacheTag)
        {
            context = applicationDbContext;
            Mapper = mapper;
            OutputCacheStore = outputCacheStore;
        }

        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<List<GeneroDTO>>> GetGeneros([FromQuery] PaginacionDTO paginacion)
        {
            return await Get<Genero, GeneroDTO>(
                paginacion, orden: g => g.Nombre
            );
        }

        [HttpGet("{id}", Name = "ObtenerGeneroPorId")]
        public async Task<ActionResult<GeneroDTO>> GetGenero(int id)
        {
            return await Get<Genero, GeneroDTO>(id);
        }

        [HttpPost]
        public async Task<ActionResult<GeneroDTO>> Crear([FromBody] GeneroCreacionDTO generoDTO)
        {
            return await Post<Genero, GeneroCreacionDTO, GeneroDTO>(
                generoDTO ,"ObtenerGeneroPorId"
            );
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Actualizar(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            return await Put<Genero, GeneroCreacionDTO>(
                id, generoCreacionDTO
            );
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Borrar(int id)
        {
            return await Delete<Genero>(id);
        }
    }
}