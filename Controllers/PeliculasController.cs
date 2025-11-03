using System.Diagnostics.CodeAnalysis;
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
    [Route("api/peliculas")]
    [ApiController]
    public class PeliculasController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private const string cacheTag = "peliculas";
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "peliculas";

        public PeliculasController(
            ApplicationDbContext context,
            IMapper mapper,
            IOutputCacheStore outputCacheStore,
            IAlmacenadorArchivos almacenadorArchivos) : base(context, mapper, outputCacheStore, cacheTag)
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet("landing")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<LandingPageDTO>> Get()
        {
            var top = 6;
            var hoy = DateTime.Today;

            var ProximosEstrenos = await context.Peliculas
                .Where(p => p.FechaLanzamiento > hoy)
                .OrderBy(p => p.FechaLanzamiento)
                .Take(top)
                .ProjectTo<PeliculaDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

            var EnCines = await context.Peliculas.
                Where(p => p.PeliculasCines.Select(pc => pc.PeliculaId).Contains(p.Id))
                .OrderBy(p => p.FechaLanzamiento)
                .Take(top)
                .ProjectTo<PeliculaDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

            return new LandingPageDTO
            {
                ProximosEstrenos = ProximosEstrenos,
                EnCines = EnCines
            };
        }

        [HttpGet("PostGet")]
        public async Task<ActionResult<PeliculasPostGetDTO>> PostGet()
        {
            var cines = await context.Cines.ProjectTo<CineDTO>(mapper.ConfigurationProvider).ToListAsync();
            var generos = await context.Generos.ProjectTo<GeneroDTO>(mapper.ConfigurationProvider).ToListAsync();

            return new PeliculasPostGetDTO
            {
                Cines = cines,
                Generos = generos
            };
        }

        [HttpGet("PutGet/{id:int}")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<PeliculaPutGetDTO>> PutGet(int id)
        {
            var pelicula = await context.Peliculas.ProjectTo<PeliculaDetallesDTO>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == id);

            if (pelicula is null)
                return NotFound();

            var generosSeleccionadosIds = pelicula.Generos.Select(g => g.Id).ToList();
            var generosNoSeleccionados = await context.Generos.Where(g => !generosSeleccionadosIds.Contains(g.Id))
                .ProjectTo<GeneroDTO>(mapper.ConfigurationProvider).ToListAsync();

            var cinesSeleccionados = pelicula.Cines.Select(g => g.Id).ToList();
            var cinesNoSeleccionados = await context.Cines.Where(c => !cinesSeleccionados.Contains(c.Id))
                .ProjectTo<CineDTO>(mapper.ConfigurationProvider).ToListAsync();

            var response = new PeliculaPutGetDTO
            {
                Pelicula = pelicula,
                GenerosSeleccionados = pelicula.Generos,
                GenerosNoSeleccionados = generosNoSeleccionados,
                CinesSeleccionados = pelicula.Cines,
                CinesNoSeleccionados = cinesNoSeleccionados,
                Actores = pelicula.Actores
            };

            return response;
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = await context.Peliculas
                .Include(p => p.PeliculasActores)
                .Include(p => p.PeliculasGeneros)
                .Include(p => p.PeliculasCines)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula == null)
                return NotFound();

            pelicula = mapper.Map(peliculaCreacionDTO, pelicula);

            if (peliculaCreacionDTO.Poster is not null)
            {
                pelicula.Poster = await almacenadorArchivos.Editar(
                    pelicula.Poster!, contenedor, peliculaCreacionDTO.Poster);
            }

            AsignarOrdenActores(pelicula);

            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = mapper.Map<Pelicula>(peliculaCreacionDTO);
            if (peliculaCreacionDTO.Poster is not null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, peliculaCreacionDTO.Poster);
                pelicula.Poster = url;
            }

            AsignarOrdenActores(pelicula);

            context.Add(pelicula);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);

            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);
            return CreatedAtRoute("ObtenerPeliculaPorId", new { id = pelicula.Id }, peliculaDTO);
        }

        [HttpGet("{id:int}", Name = "ObtenerPeliculaPorId")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<PeliculaDetallesDTO>> Get(int id)
        {
            var pelicula = await context.Peliculas
                .ProjectTo<PeliculaDetallesDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula == null)
                return NotFound();

            return pelicula;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Pelicula>(id);
        }

        [HttpGet("filtrar")]
        public async Task<ActionResult<List<PeliculaDTO>>> Filtrar([FromQuery] PeliculasFiltrarDTO peliculasFiltrarDTO)
        {
            var peliculasQueryable = context.Peliculas.AsQueryable();
            if (!string.IsNullOrWhiteSpace(peliculasFiltrarDTO.Titulo))
            {
                peliculasQueryable = peliculasQueryable.Where(p => p.Titulo.Contains(peliculasFiltrarDTO.Titulo));
            }

            if (peliculasFiltrarDTO.EnCines)
            {
                peliculasQueryable = peliculasQueryable.Where(p => p.PeliculasCines.Select(pc => pc.PeliculaId).Contains(p.Id));
            }

            if (peliculasFiltrarDTO.ProximosEstrenos)
            {
                peliculasQueryable = peliculasQueryable.Where(p => p.FechaLanzamiento > DateTime.Today);
            }

            if (peliculasFiltrarDTO.GeneroId != 0)
            {
                peliculasQueryable = peliculasQueryable.Where(p => p.PeliculasGeneros.Select(pg => pg.GeneroId).Contains(peliculasFiltrarDTO.GeneroId));
            }

            await HttpContext.InsertarTotalRegistrosEnCabecera(peliculasQueryable);
            var peliculas = await peliculasQueryable.Paginar(peliculasFiltrarDTO.Paginacion)
                            .ProjectTo<PeliculaDTO>(mapper.ConfigurationProvider)
                            .ToListAsync();
            return peliculas;
        }
        
        private void AsignarOrdenActores(Pelicula peliculas)
        {
            if(peliculas.PeliculasActores is not null)
            {
                for(int i = 0; i < peliculas.PeliculasActores.Count; i++)
                {
                    peliculas.PeliculasActores[i].Orden = i;
                }
            }
        }
    }
}