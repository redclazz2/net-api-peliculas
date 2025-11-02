using AutoMapper;
using net_api_peliculas.DTO;
using net_api_peliculas.Entidades;
using NetTopologySuite.Geometries;

namespace net_api_peliculas.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(
            GeometryFactory geometryFactory
        )
        {
            ConfigGeneros();
            ConfigActores();
            ConfigCines(geometryFactory);
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
        
        private void ConfigCines(GeometryFactory geometryFactory)
        {
            CreateMap<CineCreacionDTO, Cine>()
            .ForMember(x => x.Ubicacion, cineDTO => cineDTO.MapFrom(
                p => geometryFactory.CreatePoint(new Coordinate(p.Longitud, p.Latitud))
            ));

            CreateMap<Cine, CineDTO>().
            ForMember(
                x => x.Latitud, cine => cine.MapFrom(p => p.Ubicacion.Y)
            )
            .ForMember(
                x => x.Longitud, cine => cine.MapFrom(p => p.Ubicacion.X)
            );

        }
    }
}