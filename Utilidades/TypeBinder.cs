using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace net_api_peliculas.Utilidades
{
    public class TypeBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            //Nombre de la propiedad del DTO
            //nombrePropiedad = GenerosIDs
            var nombrePropiedad = bindingContext.ModelName;
            //Valor raw del HTTP
            //Deberia ser una string
            var valor = bindingContext.ValueProvider.GetValue(nombrePropiedad); 

            if (valor == ValueProviderResult.None)
            {
                return Task.CompletedTask; //Null
            }

            try
            {
                //Gets the .NET type to convert to based on the DTO
                var tipoDestino = bindingContext.ModelMetadata.ModelType;

                //Deserializes the JSON into that .NET type
                var valorDeserializado = JsonSerializer.Deserialize(
                    valor.FirstValue!, tipoDestino, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                //Confirms transaction went OK
                bindingContext.Result = ModelBindingResult.Success(valorDeserializado);
            }
            catch
            {
                bindingContext.ModelState.TryAddModelError(
                    nombrePropiedad, "El valor dado no es el tipo adecuado");
            }

            return Task.CompletedTask;
        }
    }
}