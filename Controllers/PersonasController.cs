using EFCorePeliculas.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCorePeliculas.Controllers
{
    [ApiController]
    [Route("api/personas")]
    public class PersonasController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public PersonasController(ApplicationDbContext context)
        {
            this.context = context;
        }
        //quiero traer a la persona por id, y sus mensajes 
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Persona>> Get(int id)
        {
            return await context.Personas
                        .Include(p => p.MensajesEnviados)
                        .Include(p => p.MensajesRecibidos)
                        .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
