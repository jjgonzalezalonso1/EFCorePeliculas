namespace EFCorePeliculas.Entidades.SinLlaves
{
    public class PeliculaConConteos
    {//coloco las mismas propiedades que devuelve el query
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int CantidadGeneros { get; set; }
        public int CantidadCines { get; set; }
        public int CantidadActores { get; set; }
    }
}
