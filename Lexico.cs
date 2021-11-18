using System;
using System.IO;

namespace Generador
{
    public class Lexico : Token, IDisposable
    {

        protected StreamReader archivo;
        protected StreamWriter bitacora;
        protected StreamWriter lenguaje;
        const int F = -1;
        const int E = -2;
        protected int linea, caracter;
        DateTime fecha = DateTime.Now;
        int[,] TRAND ={
           //WS, L, -, >, \, ;, ?, (, ), |,LA, *, /,EF,10
            { 0, 1, 2,10, 4,10,10,10,10,10,10,10,11, F, 0}, //Estado 0
            { F, 1, F, F, F, F, F, F, F, F, F, F, F, F, F}, //Estado 1
            { F, F, F, 3, F, F, F, F, F, F, F, F, F, F, F}, //Estado 2
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //Estado 3
            { F, F, F, F, F, 5, 6, 7, 8, 9, F, F, F, F, F}, //Estado 4
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //Estado 5
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //Estado 6
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //Estado 7
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //Estado 8
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //Estado 9
            { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F}, //Estado 10
            { F, F, F, F, F, F, F, F, F, F, F,13,12, F, F}, //Estado 11
            {12,12,12,12,12,12,12,12,12,12,12,12,12, 0, 0}, //Estado 12 
            {13,13,13,13,13,13,13,13,13,13,13,14,13, E,13}, //Estado 13
            {13,13,13,13,13,13,13,13,13,13,13,14, 0, E,13}, //Estado 14
        }; 
        public Lexico()
        {
            Console.WriteLine("Compilando el archivo prueba.gram");
            Console.WriteLine("Inicia analisis lexico");
            if (Path.GetExtension("C:\\Archivos\\prueba.gram") == ".gram")
            {
                if (File.Exists("C:\\Archivos\\prueba.gram"))
                {
                    linea = caracter = 1;
                    archivo = new StreamReader("C:\\Archivos\\prueba.gram");
                    bitacora = new StreamWriter("C:\\Archivos\\prueba.log");
                    lenguaje = new StreamWriter("C:\\Archivos\\lenguaje.cs");
                    bitacora.AutoFlush = true;
                    lenguaje.AutoFlush = true;
                    bitacora.WriteLine("Archivo: prueba.gram");
                    bitacora.WriteLine("Directorio: C:\\Archivos");
                    bitacora.WriteLine("Fecha: " + fecha);
                }
                else
                {
                    throw new Exception("El archivo prueba.gram no existe");
                }
            }
            else
            {

                throw new Exception("El archivo prueba.cpp no es archivo de c++");
                //Console.WriteLine("El archivo prueba.txt no existe");
                //Tarea: investigar el manejo de excepciones en c#
            }

        }
        //~Archivos()
        public Lexico(string nombre)
        {
            Console.WriteLine("Compilando el archivo " + Path.GetFileName(nombre));
            Console.WriteLine("Inicia analisis lexico");
            if (Path.GetExtension(nombre) == ".gram")
            {
                if (File.Exists(nombre))
                {
                    linea = caracter = 1;
                    archivo = new StreamReader(nombre);
                    bitacora = new StreamWriter(Path.ChangeExtension(nombre, "log"));
                    lenguaje = new StreamWriter("C:\\Archivos\\lenguaje.cs");
                    bitacora.AutoFlush = true;
                    lenguaje.AutoFlush = true;
                    bitacora.WriteLine("Archivo: " + Path.GetFileName(nombre));
                    bitacora.WriteLine("Directorio: " + Path.GetDirectoryName(nombre));
                    bitacora.WriteLine("Fecha: " + fecha);
                }
                else
                {
                    throw new Exception("El archivo " + Path.GetFileName(nombre) + " no existe");
                }
            }
            else
            {

                throw new Exception("El archivo " + Path.GetFileName(nombre) + " no es archivo de c++");
            }

        }
        public void Dispose()
        {
            CerrarArchivos();
            Console.WriteLine("\nFinaliza compilacion");
        }
        private void CerrarArchivos()
        {
            archivo.Close();
            bitacora.Close();
            lenguaje.Close();
        }
        public void NextToken()
        {
            //Console.WriteLine(".");
            char c;
            string palabra = "";
            int estado = 0;
            //const int   F= -1;
            //const int e = -2;
            while (estado >= 0)
            {
                c = (char)(archivo.Peek());
                estado = TRAND[estado, Columna(c)];
                Clasifica(estado);
                if (estado >= 0)
                {
                    archivo.Read();
                    caracter++;
                    if (c == 10)
                    {
                        linea++;
                        caracter = 1;
                    }
                    if (estado > 0)
                    {
                        palabra += c;
                    }
                    else
                    {
                        palabra = "";
                    }
                }
            }
            if (estado == E)
            {
                bitacora.WriteLine("Error Lexico: No se cerraron comentarios multilinea. En la linea: " + linea + " caracter: " + caracter);
                throw new Exception("Error Lexico: No se cerraron comentarios multilinea. En la linea: " + linea + " caracter: " + caracter);
            }
            setContenido(palabra);
            if (getClasificacion() == Clasificaciones.snt)
            {
                if (!char.IsUpper(getContenido()[0]))
                {
                    setClasificacion(Clasificaciones.st);

                }
            }
            if (getContenido() != "")
            {
                bitacora.WriteLine("Token = " + getContenido());
                bitacora.WriteLine("Clasificacion es igual a = " + getClasificacion());
            }
        }
        private int Columna(char t)
        {
            //WS, L, -, >, \, ;, ?, (, ), |, LA, *, /, EF, 10
            if(FinArchivo())
            {
                return 13;
            }
            else if(t==10)
            {
                return 14;
            }
            else if (char.IsWhiteSpace(t))
            {
                return 0;
            }
            else if (char.IsLetter(t))
            {
                return 1;
            }
            else if (t == '-')
            {
                return 2;
            }
            else if (t == '>')
            {
                return 3;
            }
            else if (t == '\\')
            {
                return 4;
            }
            else if (t == ';')
            {
                return 5;
            }
            else if (t == '?')
            {
                return 6;
            }
            else if (t == '(')
            {
                return 7;
            }
            else if (t == ')')
            {
                return 8;
            }
            else if (t == '|')
            {
                return 9;
            }
            else if (t == '*')
            {
                return 11;
            }
            else if (t == '/')
            {
                return 12;
            }
            else
            {
                return 10;
            }

        }
        private void Clasifica(int estado)
        {
            switch (estado)
            {
                case 1:
                    setClasificacion(Clasificaciones.snt);
                    break;
                case 2:
                case 4:
                case 10:
                    setClasificacion(Clasificaciones.st);
                    break;
                case 3:
                    setClasificacion(Clasificaciones.flechita);
                    break;
                case 5:
                    setClasificacion(Clasificaciones.finProduccion);
                    break;
                case 6:
                    setClasificacion(Clasificaciones.cerraduraEpsilon);
                    break;
                case 7:
                    setClasificacion(Clasificaciones.parentesisIzquierdo);
                    break;
                case 8:
                    setClasificacion(Clasificaciones.parentesisDerecho);
                    break;
                case 9:
                    setClasificacion(Clasificaciones.or);
                    break;

            }
        }
        public bool FinArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}