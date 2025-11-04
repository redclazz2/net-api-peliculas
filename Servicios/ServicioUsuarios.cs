using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace net_api_peliculas.Servicios
{
    public class ServicioUsuarios : IServicioUsuarios
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<IdentityUser> userManager;

        public ServicioUsuarios(
            IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager
        )
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public async Task<string> ObtenerUsuario()
        {
            var email = httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(
                x => x.Type == "email"
            )!.Value;

            var usuario = await userManager.FindByEmailAsync(email);

            return usuario!.Id;
        }
    }

    
}