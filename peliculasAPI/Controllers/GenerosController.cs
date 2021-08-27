
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using peliculasAPI.Entidades;
using peliculasAPI.Entidades.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace peliculasAPI.Controllers
{
    [Route("api/generos")]
    public class GenerosController:ControllerBase
    {
        private readonly IRepositorio repositorio;
        public GenerosController(IRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpGet]
        public List<Genero> Get()
        {
            return repositorio.ObtenerTodosLosGeneros();
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Genero>> Get(int Id, [FromHeader] string nombre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var genero = await repositorio.ObtenerPorId(Id);

            if(genero == null)
            {
                return NotFound();
            }

            return genero;
        }

        [HttpPost]
        public ActionResult Post()
        {
            return NoContent();
        }

        [HttpPut]
        public ActionResult Put()
        {
            return NoContent();
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            return NoContent();
        }
    }
}
