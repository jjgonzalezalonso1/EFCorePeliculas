using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCorePeliculas.Entidades.Conversiones
{
    //Heredo de ValueConverter, Ctrl + pto
    //Transformamos de Moneda a string
    public class MonedaASimboloConverter:ValueConverter<Moneda,string>
    {
        //utilizamos el constructor de la clase
        public MonedaASimboloConverter()
            : base(
                    valor => MapeoMonedaString(valor),
                    valor => MapeoStringMoneda(valor)
                 )
        {

        }

        //creo un método para mapear de moneda a string
        private static string MapeoMonedaString(Moneda valor)
        {
            return valor switch
            {
                Moneda.PesoDominicano => "RD$",
                Moneda.DolarEstadounidense => "$",
                Moneda.Euro => "€",
                _ => "" //valor por defecto es un valor vacio
            };
        }
        //creo un método para mapear de string a moneda
        private static Moneda MapeoStringMoneda(string valor)
        {
            return valor switch
            {
                "RD$" => Moneda.PesoDominicano,
                "$" => Moneda.DolarEstadounidense,
                "€" => Moneda.Euro,
                _ => Moneda.Desconocida
            };
        }
    }
}
