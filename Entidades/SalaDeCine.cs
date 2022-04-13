using System.ComponentModel.DataAnnotations.Schema;

namespace EFCorePeliculas.Entidades
{
    public class SalaDeCine
    {
        // prop tab-tab
        public int Id { get; set; }
        public TipoSalaDeCine TipoSalaDeCine { get; set; }
        public decimal Precio { get; set; }
        //public int CineId { get; set; }
        //public Cine Cine { get; set; }
        public int ElCine { get; set; }
        [ForeignKey(nameof(ElCine))]
        public Cine Cine { get; set; }
        public HashSet<Pelicula> Peliculas { get; set; }
        public Moneda Moneda { get; set; }
    }
}
