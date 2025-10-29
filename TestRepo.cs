using net_api_peliculas.Entidades;
using net_api_peliculas.Interfaces;

namespace net_api_peliculas
{
    public class TestRepo : IRepositorio
    {
        private readonly List<Genero> generos =
        [
            new Genero(){ Id= 0, Nombre = "Accion"},
            new Genero(){ Id= 0, Nombre = "Comedia"}
        ];

        public async Task<Genero> GetGenero()
        {
            await Task.Delay(TimeSpan.FromSeconds(3));

            return new Genero
            {
                Nombre = "Pepe"
            };
        }

        public bool Existe(string nombre)
        {
            return generos.Exists((g) => g.Nombre == nombre);
        }

        public List<Genero> ObtenerTodosLosGeneros()
        {
            return generos;
        }

        public async Task<Genero?> ObtenerPorId(int id)
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
            return generos.FirstOrDefault(g => g.Id == id);
        }
    
        public void Crear(Genero genero)
        {
            genero.Id = generos.Count;
            generos.Add(genero);
        }
    }
}