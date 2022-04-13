using AutoMapper;
using EFCorePeliculas.DTOs;
using EFCorePeliculas.Entidades;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace EFCorePeliculas.Servicios
{//Heredamos de Profile, Ctrl + pto
    public class AutoMapperProfiles: Profile
    {
        //Creamos un constructor
        public AutoMapperProfiles()
        {
            // < de donde a donde va la proyección>
            CreateMap<Actor, ActorDTO>();
            CreateMap<Cine, CineDTO>()
               .ForMember(dto => dto.Latitud, ent => ent.MapFrom(prop => prop.Ubicacion.Y))
               .ForMember(dto => dto.Longitud, ent => ent.MapFrom(prop => prop.Ubicacion.X));

            // Sin ProjectTo
            CreateMap<Genero, GeneroDTO>();
            CreateMap<Pelicula, PeliculaDTO>()
               .ForMember(dto => dto.Cines, ent => ent.MapFrom(prop => prop.SalasDeCine.Select(s => s.Cine)))
               .ForMember(dto => dto.Actores, ent => ent.MapFrom(prop => prop.PeliculasActores.Select(pa => pa.Actor)));

            // Con ProjectTo
            //CreateMap<Pelicula, PeliculaDTO>()
            //    .ForMember(dto => dto.Generos, ent => ent.MapFrom(prop =>
            //        prop.Generos.OrderByDescending(g => g.Nombre)))
            //    .ForMember(dto => dto.Cines, ent => ent.MapFrom(prop => prop.SalasDeCine.Select(s => s.Cine)))
            //    .ForMember(dto => dto.Actores, ent =>
            //        ent.MapFrom(prop => prop.PeliculasActores.Select(pa => pa.Actor)));

            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            CreateMap<CineCreacionDTO, Cine>()  // mapeamos desde CineCreacionDTO a Cine
                // tengo que hacer una configuración especial para el campo Ubicación
                .ForMember(ent => ent.Ubicacion,
                    dto => dto.MapFrom(campo =>
                        geometryFactory.CreatePoint(new Coordinate(campo.Longitud, campo.Latitud))));
            CreateMap<CineOfertaCreacionDTO, CineOferta>();
            CreateMap<SalaDeCineCreacionDTO, SalaDeCine>();



            CreateMap<PeliculaCreacionDTO, Pelicula>()
               .ForMember(ent => ent.Generos,
                   dto => dto.MapFrom(campo => campo.Generos.Select(id => new Genero() { Identificador = id })))
               //mapeo de un listado de enteros a un listado de géneros (Identificador es un campo de la clase Genero)
               .ForMember(ent => ent.SalasDeCine,
                   dto => dto.MapFrom(campo => campo.SalasDeCine.Select(id => new SalaDeCine() { Id = id })));
               // la clase SalaDeCine tiene el campo Id
            CreateMap<PeliculaActorCreacionDTO, PeliculaActor>();

            CreateMap<ActorCreacionDTO, Actor>();
        }
    }
}
