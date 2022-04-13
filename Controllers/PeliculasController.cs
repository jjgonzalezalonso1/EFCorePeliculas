using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCorePeliculas.DTOs;
using EFCorePeliculas.Entidades;
using EFCorePeliculas.Entidades.SinLlaves;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCorePeliculas.Controllers
{//Hereda de ControllerBase, Ctrl + pto
    [ApiController]
    [Route("api/peliculas")]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        //Ctrl+pto, agregar campo
        //Agregamos un constructor
        public PeliculasController(ApplicationDbContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PeliculaDTO>> Get(int id)
        {
            var pelicula = await context.Peliculas
                //pulso F12 sobre Generos
                .Include(p => p.Generos.OrderByDescending(g => g.Nombre)) //quiero cargar los datoa de Generos
                .Include(p => p.SalasDeCine) //ThenInclude me permite entrar dentro de SalasDeCine
                    .ThenInclude (s => s.Cine)
                .Include(p => p.PeliculasActores.Where(pa =>pa.Actor.FechaNacimiento.Value.Year >= 1980))
                    .ThenInclude(s => s.Actor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula is null)
            {
                return NotFound();
            }

            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);
            //Es otra manera de hacer el mapeo
            peliculaDTO.Cines = peliculaDTO.Cines.DistinctBy(x => x.Id).ToList();

            return peliculaDTO;
        }

        [HttpGet("conprojectto/{id:int}")]
        public async Task<ActionResult<PeliculaDTO>> GetProjectTo(int id)
        {
            var pelicula = await context.Peliculas
                .ProjectTo<PeliculaDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula is null)
            {
                return NotFound();
            }

            pelicula.Cines = pelicula.Cines.DistinctBy(x => x.Id).ToList();
            return pelicula;
        }



        [HttpGet("cargadoselectivo/{id:int}")]
        public async Task<ActionResult> GetSelectivo(int id)
        {
            var pelicula = await context.Peliculas.Select(p =>
            new
            {//proyección de tipo anónimo
                Id = p.Id,
                Titulo = p.Titulo,
                Generos = p.Generos.OrderByDescending(g => g.Nombre).Select(g => g.Nombre).ToList(),
                CantidadActores = p.PeliculasActores.Count(),
                CantidadCines = p.SalasDeCine.Select(s => s.CineId).Distinct().Count()
                //en cuantos cines se encuentra esta película
            }).FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula is null)
            {
                return NotFound();
            }

            return Ok(pelicula);
        }




        [HttpGet("cargadoexplicito/{id:int}")]
        public async Task<ActionResult<PeliculaDTO>> GetExplicito(int id)
        {
            var pelicula = await context.Peliculas.AsTracking().FirstOrDefaultAsync(p => p.Id == id);
            //por defecto, a nivel global, tenemos definido AsNoTracking, por eso he puesto AsTracking

            //await context.Entry(pelicula).Collection(p => p.Generos).LoadAsync();
            var cantidadGeneros = await context.Entry(pelicula).Collection(p => p.Generos).Query().CountAsync();

            if (pelicula is null)
            {
                return NotFound();
            }

            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);

            return peliculaDTO;
        }



        [HttpGet("agrupadasPorEstreno")]
        public async Task<ActionResult> GetAgrupadasPorCartelera()
        {
            //agrupo por la propiedad en EnCartelera. Luego obligatoriamente viene el Select
            //proyectamos en un tipo anónimo con new
            //g.Key es el valor de p.Cartelera
            var peliculasAgrupadas = await context.Peliculas.GroupBy(p => p.EnCartelera)
                                        .Select(g => new
                                        {
                                            EnCartelera = g.Key,
                                            Conteo = g.Count(),
                                            Peliculas = g.ToList()
                                            //obtengo el listado de películas en cada grupo
                                        }).ToListAsync();

            return Ok(peliculasAgrupadas);
        }


        [HttpGet("agrupadasPorCantidadDeGeneros")]
        public async Task<ActionResult> GetAgrupadasPorCantidadDeGeneros()
        {
            var peliculasAgrupadas = await context.Peliculas.GroupBy(p => p.Generos.Count())
                                    .Select(g => new
                                    {
                                        Conteo = g.Key,
                                        Titulos = g.Select(x => x.Titulo),
                                        //sòlo quiero los Titulos de las peliculas
                                        Generos = g.Select(p => p.Generos)
                                        .SelectMany(gen => gen).Select(gen => gen.Nombre).Distinct()
                                        //quiero tener una única colección con todos los géneros
                                        //para eso se utiliza el SelectMany
                                    }).ToListAsync();

            return Ok(peliculasAgrupadas);
        }


        [HttpGet("filtrar")]
        public async Task<ActionResult<List<PeliculaDTO>>> Filtrar(
            [FromQuery] PeliculasFiltroDTO peliculasFiltroDTO)
        {
            var peliculasQueryable = context.Peliculas.AsQueryable();
            // para ir construyendo nuestra query
            if (!string.IsNullOrEmpty(peliculasFiltroDTO.Titulo))
            {
                //Si el titulo no es nulo
                peliculasQueryable = peliculasQueryable.Where(p => p.Titulo.Contains(peliculasFiltroDTO.Titulo));
            }

            if (peliculasFiltroDTO.EnCartelera)
            {
                peliculasQueryable = peliculasQueryable.Where(p => p.EnCartelera);
            }

            if (peliculasFiltroDTO.ProximosEstrenos)
            {
                var hoy = DateTime.Today;
                peliculasQueryable = peliculasQueryable.Where(p => p.FechaEstreno > hoy);
            }

            if (peliculasFiltroDTO.GeneroId != 0)
            {
                peliculasQueryable = peliculasQueryable.Where(p => 
                p.Generos.Select(g => g.Identificador)
                         .Contains(peliculasFiltroDTO.GeneroId));
            }

            var peliculas = await peliculasQueryable.Include(p => p.Generos).ToListAsync();
            // ejecutamos la query y además podemos hacer Include
            return mapper.Map<List<PeliculaDTO>>(peliculas);
            //le paso peliculas
        }


        [HttpPost]
        public async Task<ActionResult> Post(PeliculaCreacionDTO peliculaCreacionDTO)
        {
            //quiero insertar una pelicula nueva, pero con generos ya creados 
            //y salas de cine ya creadas. Para ello trabajamos con el Status
            //mapeamos a Pelicula desde peliculaCreacionDTO
            var pelicula = mapper.Map<Pelicula>(peliculaCreacionDTO);
            pelicula.Generos.ForEach(g => context.Entry(g).State = EntityState.Unchanged);
            //recojo los generos, el Framework con .Unchanged sabe que solo voy a leer esos generos
            //no los voy a modificar...
            pelicula.SalasDeCine.ForEach(s => context.Entry(s).State = EntityState.Unchanged);

            if (pelicula.PeliculasActores is not null)
            {
                for (int i = 0; i < pelicula.PeliculasActores.Count; i++)
                {
                    pelicula.PeliculasActores[i].Orden = i + 1; 
                    // llenamos el campo Orden
                }
            }

            context.Add(pelicula);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("PeliculasConConteos")]
        public async Task<ActionResult<IEnumerable<PeliculaConConteos>>> GetPeliculasConConteos()
        {
            return await context.Set<PeliculaConConteos>().ToListAsync();
        }
    }
}
