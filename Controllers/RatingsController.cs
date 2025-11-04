using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using net_api_peliculas.DTO;
using net_api_peliculas.Entidades;
using net_api_peliculas.Servicios;

namespace net_api_peliculas.Controllers
{
    [ApiController]
    [Route("api/ratings")]
    public class RatingsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IServicioUsuarios servicioUsuarios;

        public RatingsController(
            ApplicationDbContext context,
            IServicioUsuarios servicioUsuarios
        )
        {
            this.context = context;
            this.servicioUsuarios = servicioUsuarios;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post([FromBody] RatingCreacionDTO ratingCreacionDTO)
        {
            var usuarioId = await servicioUsuarios.ObtenerUsuario();
            var ratingActual = await context.RatingsPeliculas
                .FirstOrDefaultAsync(x => x.PeliculaId == ratingCreacionDTO.PeliculaId && x.UsuarioId == usuarioId);

            if (ratingActual is null)
            {
                var rating = new Rating()
                {
                    PeliculaId = ratingCreacionDTO.PeliculaId,
                    UsuarioId = usuarioId,
                    Puntuacion = ratingCreacionDTO.Puntuacion
                };

                context.Add(rating);
            }
            else
            {
                ratingActual.Puntuacion = ratingCreacionDTO.Puntuacion;
            }

            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}