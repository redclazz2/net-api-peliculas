using AutoMapper;
using net_api_peliculas.DTO;
using net_api_peliculas.Entidades;

namespace net_api_peliculas.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            ConfigGeneros();
            ConfigActores();
        }

        private void ConfigGeneros()
        {
            CreateMap<GeneroCreacionDTO, Genero>();
            CreateMap<GeneroDTO, Genero>();
            CreateMap<Genero, GeneroDTO>();
        }
        
        private void ConfigActores()
        {
            CreateMap<ActorCreacionDTO, Actor>().ForMember(x => x.Foto, opciones => opciones.Ignore());
            CreateMap<Actor, ActorDTO>();
        }
    }
}