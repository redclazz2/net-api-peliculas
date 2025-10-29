using System.ComponentModel.DataAnnotations;
using net_api_peliculas.Validaciones;

namespace net_api_peliculas.Entidades
{
    public class Genero
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos")]
        [PrimeraLetraMayus]
        public required string Nombre { get; set; }
    }
}