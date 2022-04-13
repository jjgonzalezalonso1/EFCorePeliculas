using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace EFCorePeliculas.Entidades
{
    public class Cine
    {//añado dos propiedades
        public int Id { get; set; }
        public string Nombre { get; set; }
        //[Precision (precision:9, scale:2)]
        //public decimal Precio { get; set; }
        //Borro el Precio porque es SalaDeCine quien tiene el precio
        public Point Ubicacion { get; set; }
        // agregamos una propiedad de navegación
        public CineOferta CineOferta { get; set; }
        //era solamente una oferta
        public HashSet<SalaDeCine> SalasDeCine { get; set; }
        //HashSet es una colección (+ de una) de SalasDeCine. Problema:no ordena por SalasDeCine
        //public ICollection<SalaDeCine> SalasDeCine { get; set; } // Si ordena
        //public List<SalaDeCine> SalasDeCine { get; set; }
    }
}
