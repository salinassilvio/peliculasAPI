
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using peliculasAPI.DTOs;
using peliculasAPI.Entidades;
using peliculasAPI.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace peliculasAPI.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GenerosController:ControllerBase
    {
        private readonly AplicactionDbContext context;
        private readonly ILogger<GenerosController> logger;
        private readonly IMapper mapper;

        public GenerosController(ILogger<GenerosController> logger,
            AplicactionDbContext context,
            IMapper mapper
            )
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable =  context.Generos.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEncabecera(queryable);
            var generos = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<GeneroDTO>>(generos);
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<GeneroDTO>> Get(int Id)
        {
            var genero = await context.Generos.FirstOrDefaultAsync(x => x.Id == Id);
            if(genero == null)
            {
                return NotFound();
            }
            return mapper.Map<GeneroDTO>(genero);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = mapper.Map<Genero>(generoCreacionDTO);
            context.Add(genero);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = await context.Generos.FirstOrDefaultAsync(x => x.Id == id);
            if(genero == null)
            {
                return NotFound();
            }
            genero = mapper.Map(generoCreacionDTO, genero);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            throw new NotImplementedException();
        }
    }
}
