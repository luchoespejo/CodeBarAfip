using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreryBar;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var codigo = new CodigoBarra();

            string calculo = "33708521359" + "01" + "00004" + "68379506949497"+ "20180922";

            var digito = codigo.DigitoVerificador(calculo);

            var barra = codigo.GenerarCodigoBarra("C:\\Synagro\\Contabilidad3\\codigo.jpg", digito.ToString());
        }
    }
}
