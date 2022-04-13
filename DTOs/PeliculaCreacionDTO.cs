namespace EFCorePeliculas.DTOs
{
    public class PeliculaCreacionDTO
    {
        public string Titulo { get; set; }
        public bool EnCartelera { get; set; }
        public DateTime FechaEstreno { get; set; }
        public List<int> Generos { get; set; }
        //quiero recibir un listado con los Ids de los géneros
        public List<int> SalasDeCine { get; set; }
        //quiero recibir un listado con los Ids de las SalasDeCine
        public List<PeliculaActorCreacionDTO> PeliculasActores { get; set; }
        //quiero recibir los actores de la película
    }
}
