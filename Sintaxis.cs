using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generador
{
    public class Sintaxis : Lexico
    {
        public Sintaxis()
        {
            Console.WriteLine("Inicia analisis sintactico");
            NextToken();
        }
        public Sintaxis(string nombre) : base(nombre)
        {
            Console.WriteLine("Inicia analisis sintactico");
            NextToken();
        }
        protected void match(string espera)
        {
            //Console.WriteLine(getContenido() + " = " + espera);
            if (espera == getContenido())
            {
                NextToken();
            }
            else
            {
                bitacora.WriteLine("Error de Sintaxis: Se espera un " + espera + ". En la linea: " + linea + " caracter: " + caracter);
                throw new Exception("Error de Sintaxis: Se espera un " + espera+ ". En la linea: " + linea + " caracter: " + caracter);
            }
        }
        protected void match(Clasificaciones espera)
        {
            //Console.WriteLine(getContenido() + " = " + espera);
            if (espera == getClasificacion())
            {
                NextToken();
            }
            else
            {
                bitacora.WriteLine("Error de Sintaxis: Se espera un " + espera + ". En la linea: " + linea + " caracter: " + caracter);
                throw new Exception("Error de Sintaxis: Se espera un " + espera + ". En la linea: " + linea + " caracter: " + caracter);
            }
        }
    }
}