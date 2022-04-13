using System.ComponentModel.DataAnnotations.Schema;

namespace EFCorePeliculas.Entidades
{
    public class Persona
    { // con dos propiedades
        public int Id { get; set; }
        public string Nombre { get; set; }
        //Necesito especificar a que llave foranea se corresponde MensajesEnviados
        //En Mensaje.cs tenemos EmisorId y ReceptorId.
        [InverseProperty("Emisor")]
        public List<Mensaje> MensajesEnviados { get; set; }
        [InverseProperty("Receptor")]
        public List<Mensaje> MensajesRecibidos { get; set; }
    }
}
