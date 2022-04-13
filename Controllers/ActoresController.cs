using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCorePeliculas.DTOs;
using EFCorePeliculas.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCorePeliculas.Controllers
{
    [ApiController]
    [Route("api/actores")]
    //Heredamos de ControllerBase, Ctrl + pto
    public class ActoresController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        //hacemos un constructor para tener una instancia del ApplicationDbContext
        //Ctrl  + pto, Crear y asignar campo
        public ActoresController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IEnumerable<ActorDTO>> Get()
        {
            var actores= await context.Actores.ProjectTo<ActorDTO>(mapper.ConfigurationProvider).ToListAsync();
            return actores;
        }


        [HttpPost]
        public async Task<ActionResult> Post(ActorCreacionDTO actorCreacionDTO)
        {
            var actor = mapper.Map<Actor>(actorCreacionDTO);
            //mapeamos hacia Actor
            context.Add(actor);
            await context.SaveChangesAsync();
            return Ok();
        }




        [HttpPut("{id:int}")] //para actualizar
        public async Task<ActionResult> Put(ActorCreacionDTO actorCreacionDTO, int id)
        {
            var actorDB = await context.Actores.AsTracking().FirstOrDefaultAsync(a => a.Id == id);
            //traigo el actor de la base de datos
            if (actorDB is null)
            {
                return NotFound();
            }

            actorDB = mapper.Map(actorCreacionDTO, actorDB);
            //mapeo (copio) los datos desde actorCreacionDTO a actorDB
            //porngo actorDB= para que automapper tenga la misma instancia de actorDB en memoria,
            //así Entity puede seguir haciendo seguimiento a esa instancia.
            await context.SaveChangesAsync();
            return Ok();
        }



        [HttpPut("desconectado/{id:int}")]
        public async Task<ActionResult> PutDesconectado(ActorCreacionDTO actorCreacionDTO, int id)
        {
            var existeActor = await context.Actores.AnyAsync(a => a.Id == id);
            //AnyAsync es más rápido.Determina de forma asincrónica si una secuencia contiene elementos.
            if (!existeActor)
            {
                return NotFound();
            }

            var actor = mapper.Map<Actor>(actorCreacionDTO); //cargo el actor
            actor.Id = id;

            context.Update(actor); //cambia el status a modificado
            await context.SaveChangesAsync();
            return Ok();
        }

    }
}
