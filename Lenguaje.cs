using System;
using System.Collections.Generic;
using System.Text;
/*
    Requerimiento 1: Agregar comentarios de linea y multilinea a nivel lexico
    Requerimiento 2: El proyecto se debne de llamar igual que el lenguaje
    Requerimiento 3: Indentar el codigo generado tip: Escribe (int numeroTabs, string instruccion)
    Requerimiento 4: En la cerradura epsilon considerar getClasificacion y getContenido
    Requerimiento 5: Implementar el operador OR
    Lenguaje -> lenguaje:identificador; ListaProducciones
    ListaProducciones -> snt flechita ListaSimbolos finProduccion 
    ListaSimbolos -> snt | st ListaSimbolos?
*/
namespace Generador
{
    public class Lenguaje:Sintaxis
    {
        public Lenguaje()
        {
            Console.WriteLine("Iniciando analisis gramatical.");
        }
        public Lenguaje(string nombre) : base(nombre)
        {
            Console.WriteLine("Iniciando analisis gramatical.");
        }
        //Lenguaje -> lenguaje:identificador; ListaProducciones
        public void gramatica(){
            match("lenguaje");
            match(":");
            if (getClasificacion()==Clasificaciones.snt)
            {
                match(Clasificaciones.snt);
            }
            else
            {
                match(Clasificaciones.st);
            }
            match(";");
            Cabecera();
            match("{");
            ListaProducciones();
            match("}");
            lenguaje.WriteLine("    }");
            lenguaje.WriteLine("}");
        }
        //ListaProducciones -> snt flechita ListaSimbolos finProduccion ListaProducciones?
        private void ListaProducciones()
        {
            lenguaje.WriteLine("        public void "+getContenido()+"()");
            match(Clasificaciones.snt);
            match(Clasificaciones.flechita);
            lenguaje.WriteLine("        {");
            ListaSimbolos();
            lenguaje.WriteLine("        }");
            match(Clasificaciones.finProduccion);
            if(getClasificacion()==Clasificaciones.snt)
            {
                ListaProducciones();
            }
        }
        //ListaSimbolos -> snt | st ListaSimbolos?
        private void ListaSimbolos()
        {
            if (getClasificacion()==Clasificaciones.snt)
            {
                lenguaje.WriteLine("            "+getContenido()+"();");
                match(Clasificaciones.snt);
            }
            else if (getClasificacion()==Clasificaciones.st)
            {
                if(esClasificacion(getContenido()))
                {
                    lenguaje.WriteLine("            "+"match(clasificaciones."+getContenido() +");");
                }
                else
                {
                    lenguaje.WriteLine("            "+"match(\""+getContenido() +"\");");
                }
                match(Clasificaciones.st);
            }
            else if(getClasificacion()==Clasificaciones.parentesisIzquierdo)
            {
                match(Clasificaciones.parentesisIzquierdo);
                lenguaje.WriteLine("            "+"if(getContenido() == \""+getContenido() +"\")");
                lenguaje.WriteLine("            "+"{");
                if(esClasificacion(getContenido()))
                {
                    lenguaje.WriteLine("            "+"match(clasificaciones."+getContenido() +");");
                }
                else
                {
                    lenguaje.WriteLine("            "+"match(\""+getContenido() +"\");");
                }
                match(Clasificaciones.st);
                if(getClasificacion()==Clasificaciones.snt||getClasificacion()==Clasificaciones.st)
                {
                    ListaSimbolos();
                }
                match(Clasificaciones.parentesisDerecho);
                lenguaje.WriteLine("            "+"}");
                match(Clasificaciones.cerraduraEpsilon);
            }
            else if(getClasificacion()==Clasificaciones.corcheteIzquierdo)
            {
                match(Clasificaciones.corcheteIzquierdo);
                lenguaje.WriteLine("            "+"if(getContenido() == \""+getContenido() +"\")");
                lenguaje.WriteLine("            "+"{");
                if(esClasificacion(getContenido()))
                {
                    lenguaje.WriteLine("            "+"match(clasificaciones."+getContenido() +");");
                }
                else
                {
                    lenguaje.WriteLine("            "+"match(\""+getContenido() +"\");");
                }
                match(Clasificaciones.st);
                if(getClasificacion()==Clasificaciones.snt||getClasificacion()==Clasificaciones.st)
                {
                    ListaORs();
                }
                match(Clasificaciones.corcheteDerecho);
                lenguaje.WriteLine("            "+"}");
            }
            if(getClasificacion()==Clasificaciones.snt||getClasificacion()==Clasificaciones.st||getClasificacion()==Clasificaciones.parentesisIzquierdo)
            {
                ListaSimbolos();
            }
        }
        //ListaORs -> st (| ListaORs)?
        private void ListaORs()
        {
            //Generar else ifs y el ultimo simbolo debe de ser else
        }
        private void Cabecera()
        {
            lenguaje.WriteLine("using System;");
            lenguaje.WriteLine("using System.Collections.Generic;");
            lenguaje.WriteLine("using System.Text;");
            lenguaje.WriteLine("namespace Generador");
            lenguaje.WriteLine("{");
            lenguaje.WriteLine("    public class Lenguaje:Sintaxis");
            lenguaje.WriteLine("    {");
            lenguaje.WriteLine("        public Lenguaje()");
            lenguaje.WriteLine("        {");
            lenguaje.WriteLine("            Console.WriteLine(\"Iniciando analisis gramatical.\");");
            lenguaje.WriteLine("        }");
            lenguaje.WriteLine("        public Lenguaje(string nombre) : base(nombre)");
            lenguaje.WriteLine("        {");
            lenguaje.WriteLine("            Console.WriteLine(\"Iniciando analisis gramatical.\");");
            lenguaje.WriteLine("        }");
            
        }
    }
}