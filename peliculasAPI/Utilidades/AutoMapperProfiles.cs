﻿using AutoMapper;
using NetTopologySuite.Geometries;
using peliculasAPI.DTOs;
using peliculasAPI.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace peliculasAPI.Utilidades
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>()
                .ForMember(x => x.Foto, options => options.Ignore());

            CreateMap<CineCreacionDTO, Cine>()
                .ForMember(x => x.Ubicacion, x => x.MapFrom(dto =>
                geometryFactory.CreatePoint(new Coordinate(dto.Longitud, dto.Latitud))));

            CreateMap<Cine, CineDTO>()
                .ForMember(x => x.Latitud, dto => dto.MapFrom(campo => campo.Ubicacion.Y))
                .ForMember(x => x.Longitud, dto => dto.MapFrom(campo => campo.Ubicacion.X));


            CreateMap<PeliculaCreacionDTO, Pelicula>()
                .ForMember(x => x.Poster, opciones => opciones.Ignore())
                .ForMember(x => x.PeliculasGeneros, opciones => opciones.MapFrom(MapearPeliculasGeneros))
                .ForMember(x => x.PeliculasCines, opciones => opciones.MapFrom(MapearPeliculasCines))
                .ForMember(x => x.PeliculaActores, opciones => opciones.MapFrom(MapearPeliculasActores));
        }

        private List<PeliculasGeneros> MapearPeliculasGeneros(PeliculaCreacionDTO peliculaCreacionDTO,
            Pelicula pelicula)
        {
            var resultado = new List<PeliculasGeneros>();

            if(peliculaCreacionDTO.GenerosIds == null) { return resultado; }

            foreach(var id in peliculaCreacionDTO.GenerosIds)
            {
                resultado.Add(new PeliculasGeneros() { GeneroId = id });
            }

            return resultado;
        }

        private List<PeliculasCines> MapearPeliculasCines(PeliculaCreacionDTO peliculaCreacionDTO, 
            Pelicula pelicula)
        {
            var resultado = new List<PeliculasCines>();

            if (peliculaCreacionDTO.CinesIds == null) { return resultado; }

            foreach (var id in peliculaCreacionDTO.CinesIds)
            {
                resultado.Add(new PeliculasCines() { CineId = id });
            }

            return resultado;
        }

        private List<PeliculaActores> MapearPeliculasActores(PeliculaCreacionDTO peliculaCreacionDTO,
    Pelicula pelicula)
        {
            var resultado = new List<PeliculaActores>();

            if (peliculaCreacionDTO.Actores == null) { return resultado; }

            foreach (var actor in peliculaCreacionDTO.Actores)
            {
                resultado.Add(new PeliculaActores() { ActorId = actor.Id, Personaje =  actor.Personaje});
            }

            return resultado;
        }

    }
}
