
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

namespace EFCorePeliculas.Entidades
{
    [Index(nameof(Nombre), IsUnique =true)] 
    //Creo un indice con la columna Nombre y es unico
    //Me garantiza que no vamos a tener dos generos 
    //con el mismo nombre
    public class Genero
    {//escribo prop y pulso dos veces tabulador
        public int Identificador{ get; set; }
        //[StringLength(150)] es un modo
        //[MaxLength(150)] // es otro modo exactamente igual al anterior
        //[Required]  // es requerido,no puede ser nulo
        //[Column("NombreGenero")]
        public string Nombre { get; set; }
        //por defecto es no nulo
        //Hay que pulsar en EFCorePeliculas 
        public bool EstaBorrado { get; set; }   
        public HashSet<Pelicula> Peliculas { get; set; }
    }
}
