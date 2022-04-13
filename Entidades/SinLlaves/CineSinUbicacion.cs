using Microsoft.EntityFrameworkCore;

namespace EFCorePeliculas.Entidades.SinLlaves
{ 
    //[Keyless]  // entidad sin llave
    //o se pone en el API Fluente, ApplicationDBContext
    public class CineSinUbicacion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
