using net_api_peliculas.Entidades;

namespace net_api_peliculas.Interfaces
{
    public interface IRepositorio
    {
        public List<Genero> ObtenerTodosLosGeneros();
        Task<Genero?> ObtenerPorId(int id);
        public bool Existe(string nombre);
        public void Crear(Genero genero);
    }
}