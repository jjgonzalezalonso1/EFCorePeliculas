using System.ComponentModel.DataAnnotations.Schema;

namespace EFCorePeliculas.Entidades
{
    public class Log
    {//Ctrl + pto
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //no me generes ningún valor
        public Guid Id { get; set; }
        public string Mensaje { get; set; }
    }
}
