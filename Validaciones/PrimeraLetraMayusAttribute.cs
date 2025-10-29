using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace net_api_peliculas.Validaciones
{
    public class PrimeraLetraMayusAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var primeraLetra = value.ToString()![0].ToString();

            if (primeraLetra != primeraLetra.ToUpper())
            {
                return new ValidationResult("Error, la primera letra no es mayuscula");
            }
            
            return ValidationResult.Success;
        }
    }
}