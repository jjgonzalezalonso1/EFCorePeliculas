//using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations.Schema;

namespace EFCorePeliculas.Entidades
{
    public class Actor
    {
        public int Id { get; set; }
        public string FotoURL { get; set; }
        private string _nombre; //campo
        public string Nombre
        {
            //propiedad que trabaja con el campo _nombre
            get
            {
                return _nombre;
            }
            set
            {//cuando insertemos el valor, la primera letra de cada palabra va a estar en mayúsculas
                _nombre = string.Join(' ',
                       //Join para unir cada una de las palabras
                        value.Split(' ')
                        .Select(x => x[0].ToString().ToUpper() + x.Substring(1).ToLower()).ToArray());
                //pone el caracter[0] en mayus + concatena a partir del caracter [1] hasta el final de la palabra
                //en minusculas
            }
        }
        public string Biografia { get; set; }
        //[Column(TypeName ="Date")]
        public DateTime? FechaNacimiento { get; set; }
        public HashSet<PeliculaActor> PeliculasActores { get; set; }
        public Direccion Direccion { get; set; }    

        [NotMapped]
        public int? Edad
        {
            get
            {
                if (!FechaNacimiento.HasValue)
                {
                    return null;
                }

                var fechaNacimiento = FechaNacimiento.Value;

                var edad = DateTime.Today.Year - fechaNacimiento.Year;

                if (new DateTime(DateTime.Today.Year, fechaNacimiento.Month, fechaNacimiento.Day) > DateTime.Today)
                {
                    edad--; // Si estamos antes de su fecha de nacimiento le restamos uno a la edad
                }
                return edad;
            }
        }


    }
}
