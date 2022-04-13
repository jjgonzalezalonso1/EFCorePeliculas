namespace EFCorePeliculas.Entidades
{
    public class PeliculaActor
    {
        public int PeliculaId { get; set; }
        public int ActorId { get; set; }
        public string Personaje { get; set; }
        public int Orden { get; set; }
        //propiedades de navegación para la relación muchos a muchos
        //Pelicula-Actor
        public Pelicula Pelicula { get; set; }
        public Actor Actor { get; set; }
    }
}
