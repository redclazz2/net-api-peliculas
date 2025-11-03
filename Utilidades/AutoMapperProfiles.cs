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
            ConfigPeliculas();
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
            CreateMap<Actor, PeliculaActorDTO>();
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

        private void ConfigPeliculas()
        {
            CreateMap<PeliculaCreacionDTO, Pelicula>()
            .ForMember(x => x.Poster, opciones => opciones.Ignore())
            .ForMember(x => x.PeliculasGeneros,
            dto => dto.MapFrom(p => p.GenerosIds!.Select(id =>
                new PeliculasGeneros { GeneroId = id }
            )))
            .ForMember(x => x.PeliculasCines,
                dto => dto.MapFrom(p => p.CinesIds!.Select(id =>
                new PeliculasCines { CineId = id }
            )))
            .ForMember(x => x.PeliculasActores,
                dto => dto.MapFrom(p => p.Actores!.Select(
                    actor => new PeliculasActores
                    {
                        ActorId = actor.Id,
                        Personaje = actor.Personaje,
                    }
                )));

            CreateMap<Pelicula, PeliculaDTO>();
            CreateMap<Pelicula, PeliculaDetallesDTO>()
                .ForMember(p => p.Generos, entidad => entidad.MapFrom(p => p.PeliculasGeneros))
                .ForMember(p => p.Cines, entidad => entidad.MapFrom(p => p.PeliculasCines))
                .ForMember(p => p.Actores, entidad => entidad.MapFrom(p => p.PeliculasActores));

            CreateMap<PeliculasGeneros, GeneroDTO>()
                .ForMember(g => g.Id, pg => pg.MapFrom(p => p.GeneroId))
                .ForMember(g => g.Nombre, pg => pg.MapFrom(p => p.Genero.Nombre));

            CreateMap<PeliculasCines, CineDTO>()
                .ForMember(g => g.Id, pc => pc.MapFrom(p => p.CineId))
                .ForMember(g => g.Nombre, pc => pc.MapFrom(p => p.Cine.Nombre))
                .ForMember(g => g.Latitud, pc => pc.MapFrom(p => p.Cine.Ubicacion.Y))
                .ForMember(g => g.Longitud, pc => pc.MapFrom(p => p.Cine.Ubicacion.X));

            CreateMap<PeliculasActores, PeliculaActorDTO>()
                .ForMember(dto => dto.Id, entidad => entidad.MapFrom(p => p.ActorId))
                .ForMember(dto => dto.Nombre, entidad => entidad.MapFrom(p => p.Actor.Nombre))
                .ForMember(dto => dto.Foto, entidad => entidad.MapFrom(p => p.Actor.Foto));
        }
    }
}