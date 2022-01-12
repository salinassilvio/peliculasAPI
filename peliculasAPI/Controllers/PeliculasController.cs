using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using peliculasAPI.DTOs;
using peliculasAPI.Entidades;
using peliculasAPI.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace peliculasAPI.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    public class PeliculasController:ControllerBase
    {
        private readonly AplicactionDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "peliculas";


        public PeliculasController(AplicactionDbContext context,
            IMapper mapper,
            IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = mapper.Map<Pelicula>(peliculaCreacionDTO);
            if (peliculaCreacionDTO.Poster != null)
            {
                pelicula.Poster = await almacenadorArchivos.GuardarArchivo(contenedor, peliculaCreacionDTO.Poster);
            }

            EscribirOrdenActores(pelicula);

            context.Add(pelicula);
            await context.SaveChangesAsync();
            return NoContent();
        }

        private void EscribirOrdenActores(Pelicula pelicula)
        {
            if(pelicula.PeliculaActores != null)
            {
                for (int i =0; i < pelicula.PeliculaActores.Count; i++) {
                    pelicula.PeliculaActores[i].Orden = i;
                }
            }
        }

    }
}
