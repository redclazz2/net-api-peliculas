using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using net_api_peliculas.Entidades;
using net_api_peliculas.Interfaces;

namespace net_api_peliculas.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GenerosController : Controller
    {
        public IRepositorio TestRepo { get; }
        public GenerosController(IRepositorio testRepo)
        {
            TestRepo = testRepo;
        }

        [HttpGet]
        public List<Genero> GetGeneros()
        {
            return [];
        }

        [HttpGet("{id:int}")]
        [OutputCache]
        public async Task<ActionResult<Genero>?> GetGenero(int id)
        {
            List<Genero> generos = [];
            generos.Add(new Genero
            {
                Nombre = "Acción"
            });

            generos.Add(new Genero
            {
                Nombre = "Comedia"
            });

            if (id < 0 || id > 1)
            {
                return NotFound();
            }

            TestRepo repo = new();

            return await repo.GetGenero();
        }

        [HttpGet("{nombre}")]
        public Genero? GetGenero(string id)
        {
            List<Genero> generos = [];
            generos.Add(new Genero
            {
                Nombre = "Acción"
            });

            generos.Add(new Genero
            {
                Nombre = "Comedia"
            });

            return generos[int.Parse(id)];
        }

        [HttpPost]
        public ActionResult PostGeneros([FromBody]Genero genero)
        {
            var existe = TestRepo.Existe(genero.Nombre);

            if (existe)
            {
                return BadRequest();
            }

            TestRepo.Crear(genero);

            return Ok();
        }
    }
}