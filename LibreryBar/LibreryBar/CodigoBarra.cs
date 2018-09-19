using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using BarcodeLib;

namespace LibreryBar
{
    [ComVisible(true)]

    [ClassInterface(ClassInterfaceType.AutoDual)]

    [ProgId("LibreryBar.CodigoBarra")]
    public class CodigoBarra
    {
        private static BarcodeLib.Barcode _barcode = new Barcode();
        private string _error = string.Empty;
        public bool GenerarCodigoBarra(string ubicacion, string codigo)
        {
            bool bandera = false;
            try
            {
                _barcode.EncodedType = TYPE.CODE128C;
                int W = Convert.ToInt32(350);
                int H = Convert.ToInt32(75);
                _barcode.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                _barcode.IncludeLabel = true;
                _barcode.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;
                //===== Encoding performed here =====
                _barcode.Encode(_barcode.EncodedType, codigo, Color.Black, Color.White, W, H);
                //===================================
                _barcode.SaveImage(ubicacion, BarcodeLib.SaveTypes.JPG);
                bandera = true;
            }
            catch (Exception e)
            {
                bandera = false;
                _error = "Error al generar codigo de Barra " + e.Message;
            }
            return bandera;
        }

        //    "Rutina para el cálculo del dígito verificador 'módulo 10'"
        //# Ver RG 1702 AFIP
        public string DigitoVerificador(string codigo)
        {
            string str = codigo;
            double numero = Convert.ToDouble(str);
            double etapa1 = 0;
            double etapa2 = 0;
            double etapa3 = 0;
            double etapa4 = 0;
            double etapa5 = 0;

            try
            {
                //# Etapa 1: comenzar desde la izquierda, sumar todos los caracteres ubicados en las posiciones impares.
                //etapa1 = sum([int(c) for i,c in enumerate(codigo) if not i%2])
                foreach (var item in str)
                {
                    double aux = Convert.ToDouble(item.ToString());
                    if (aux % 2 != 0)
                    {
                        etapa1 += aux;
                    }
                }
                //# Etapa 2: multiplicar la suma obtenida en la etapa 1 por el número 3
                //etapa2 = etapa1 * 3
                etapa2 = etapa1 * 3;
                //# Etapa 3: comenzar desde la izquierda, sumar todos los caracteres que están ubicados en las posiciones pares.
                //etapa3 = sum([int(c) for i,c in enumerate(codigo) if i%2])
                foreach (var item in str)
                {
                    double aux = Convert.ToDouble(item.ToString());
                    if (aux % 2 == 0)
                    {
                        etapa3 += aux;
                    }
                }
                //# Etapa 4: sumar los resultados obtenidos en las etapas 2 y 3.
                //etapa4 = etapa2 + etapa3
                etapa4 = etapa2 + etapa3;
                //# Etapa 5: buscar el menor número que sumado al resultado obtenido en la etapa 4 dé un número múltiplo de 10.
                //# Este será el valor del dígito verificador del módulo 10.
                //digito = 10 - (etapa4 - (int(etapa4 / 10) * 10))
                for (int i = 0; i <= 10; i++)
                {
                    if (((etapa4 + Convert.ToDouble(i)) % 10) == 0)
                    {
                        if (i == 10) i = 0;
                        etapa5 = i;
                        break;
                    }
                }
                //if digito == 10:
                //    digito = 0
                //return str(digito)
            }
            catch (Exception e)
            {
                _error = "Error al generar digitor verificador " + e.Message;
            }
            return str + etapa5.ToString();
        }
    }
}
