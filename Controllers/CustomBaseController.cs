using System.Linq.Expressions;
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
    public class CustomBaseController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private readonly string cacheTag;

        public CustomBaseController(
            ApplicationDbContext context,
            IMapper mapper,
            IOutputCacheStore outputCacheStore,
            string cacheTag
        )
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
            this.cacheTag = cacheTag;
        }

        protected async Task<List<DTO>> Get<Entidad, DTO>(
            PaginacionDTO paginacion,
            Expression<Func<Entidad, object>> orden
        )
            where Entidad : class
        {
            var queryable = context.Set<Entidad>().AsQueryable();
            await HttpContext.InsertarTotalRegistrosEnCabecera(queryable);
            return await queryable
                .OrderBy(orden)
                .Paginar(paginacion)
                .ProjectTo<DTO>(mapper.ConfigurationProvider).ToListAsync();
        }

        protected async Task<List<DTO>> Get<Entidad, DTO>(
            Expression<Func<Entidad, object>> orden
        )
            where Entidad : class
        {
            var queryable = context.Set<Entidad>().AsQueryable();
            await HttpContext.InsertarTotalRegistrosEnCabecera(queryable);
            return await queryable
                .OrderBy(orden)
                .ProjectTo<DTO>(mapper.ConfigurationProvider).ToListAsync();
        }

        protected async Task<ActionResult<DTO>> Get<Entidad, DTO>(int id)
            where Entidad : class, IId
            where DTO : IId
        {
            var entidad = await context.Set<Entidad>()
            .ProjectTo<DTO>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(
                x => x.Id == id
            );

            if (entidad == null)
                return NotFound();

            return entidad;
        }

        protected async Task<ActionResult> Post<Entidad, CreacionDTO, DTO>(
            CreacionDTO creacionDTO, string nombreRuta
        ) where Entidad : class, IId
        {
            var entidad = mapper.Map<Entidad>(creacionDTO);
            context.Add(entidad!);

            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);

            var entidadDTO = mapper.Map<DTO>(entidad);
            return CreatedAtRoute(nombreRuta, new { id = entidad.Id }, entidadDTO);
        }

        protected async Task<ActionResult> Put<Entidad, CreacionDTO>(int id, CreacionDTO creacionDTO)
            where Entidad : class, IId
        {
            var entidad = await context.Set<Entidad>().FirstOrDefaultAsync(e => e.Id == id);

            if (entidad is null)
                return NotFound();

            entidad = mapper.Map(creacionDTO, entidad);
            entidad.Id = id;

            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(tag: cacheTag, default);

            return NoContent();
        }
        
        protected async Task<ActionResult> Delete<Entidad>(int id)
            where Entidad : class , IId
        {
            var registrosBorrados = await context.Set<Entidad>().Where(
                (g) => g.Id == id
            ).ExecuteDeleteAsync();

            if (registrosBorrados == 0)
            {
                return NotFound();
            }

            await outputCacheStore.EvictByTagAsync(tag: cacheTag, default);
            return NoContent();
        }
    }
}