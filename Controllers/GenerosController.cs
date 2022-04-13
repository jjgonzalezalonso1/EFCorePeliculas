using EFCorePeliculas.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCorePeliculas.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GenerosController:ControllerBase
    {
        //Recibe pediciones Http en nuestro WebApi en relacion a la entidad Generos
  
        //El constructor recibe el ApplicationDbContext
        // Ctrol + pto, asignar como un campo
        private readonly ApplicationDbContext context;
        public GenerosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Genero>> Get()
        {
            //return await context.Generos.OrderBy(g => g.Nombre).ToListAsync();
            //agrego un filtro
            context.Logs.Add(new Log
            {
                Id=Guid.NewGuid(),  //Así puedo generar yo el GUID
                Mensaje = "Ejecutando el método GenerosController.Get"
            });
            await context.SaveChangesAsync();
            return await context.Generos.Where(g => !g.EstaBorrado).OrderByDescending(g => EF.Property<DateTime>(g, "FechaCreacion")).ToListAsync();

            //return await context.Generos.Where(g => !g.EstaBorrado).OrderBy(g => g.Nombre).ToListAsync();
            //pulso Ctrl + F5 para ejecutar, me lleva a Swagger
        }

        
        [HttpGet("{id:int}")] //recibimos el id como un parametro
        public async Task<ActionResult<Genero>> Get(int id)
        {
            var genero = await context.Generos.AsTracking().FirstOrDefaultAsync(g => g.Identificador == id);

            if (genero is null)
            {
                return NotFound();
            }
            var fechacreacion = context.Entry(genero).Property<DateTime>("FechaCreacion").CurrentValue;
            return Ok(new
            {
                Id = genero.Identificador,
                NoContent = genero.Nombre,
                fechacreacion
            });
        }


        [HttpPost]
        public async Task<ActionResult> Post(Genero genero)
        {
            var existeGeneroConNombre = await context.Generos.AnyAsync(g => g.Nombre == genero.Nombre);

            if (existeGeneroConNombre)
            {
                return BadRequest("Ya existe un género con ese nombre: " + genero.Nombre);
            }

            var estatus1 = context.Entry(genero).State; 
            context.Add(genero);
            var estatus2 = context.Entry(genero).State;
            // está marcando el genero con el status de agregado
            // no se está agregando nada a la base de datos.
            await context.SaveChangesAsync();
            var estatus3 = context.Entry(genero).State;
            // vamos a verificar todos los objetos  a los  que el entity framework core 
            // hace seguimiento, vemos el status de ese objeto, y luego hacer algo
            // en nuestro caso lo agrega a nuestra tabla de géneros
            return Ok(); //para retornar algo
        }



        [HttpPost("varios")]
        public async Task<ActionResult> Post(Genero[] generos)
            // recibimos un arreglo de generos
        {
            context.AddRange(generos);
            await context.SaveChangesAsync();
            return Ok();
        }



        [HttpPost("agregar2")]
        public async Task<ActionResult> Agregar2(int id) //recibo el id del género
        {
            var genero = await context.Generos.AsTracking().FirstOrDefaultAsync(g => g.Identificador == id);
            //por defecto tenemos configurado a nivel global AsNoTracking
            //la clase Genero tiene un campo que se llama Identificador
            //MODO CONECTADO: utilizo context para cargar el genero
            if (genero is null)
            {
                return NotFound();
            }

            genero.Nombre += " 2";
            await context.SaveChangesAsync();
            //MODO CONECTADO: utilizo el mismo context para actualizar el genero
            return Ok();
        }




        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var genero = await context.Generos.FirstOrDefaultAsync(g => g.Identificador == id);

            if (genero is null)
            {
                return NotFound();
            }

            context.Remove(genero);//cambia el status del genero a borrado
            await context.SaveChangesAsync();//borra de la base de datos
            return Ok();
        }



        [HttpDelete("borradoSuave/{id:int}")]
        public async Task<ActionResult> DeleteSuave(int id)
        {
            var genero = await context.Generos.AsTracking().FirstOrDefaultAsync(g => g.Identificador == id);

            if (genero is null)
            {
                return NotFound();
            }

            genero.EstaBorrado = true;
            await context.SaveChangesAsync();
            return Ok();
        }



        [HttpPost("Restaurar/{id:int}")]
        public async Task<ActionResult> Restaurar(int id)
        {
            var genero = await context.Generos.AsTracking()
                .IgnoreQueryFilters() // IMPORTANTE
                .FirstOrDefaultAsync(g => g.Identificador == id);

            if (genero is null)
            {
                return NotFound();
            }

            genero.EstaBorrado = false;
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
