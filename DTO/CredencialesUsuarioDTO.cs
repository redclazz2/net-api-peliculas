using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace net_api_peliculas.DTO
{
    public class CredencialesUsuarioDTO
    {
        [EmailAddress]
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}