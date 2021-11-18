using System;
using System.Collections.Generic;
using System.Text;
/*
    Requerimiento 1: Agregar comentarios de linea y multilinea a nivel lexico ☭
    Requerimiento 2: El proyecto se debe de llamar igual que el lenguaje ☭
    Requerimiento 3: Indentar el codigo generado tip: Escribe (int numeroTabs, string instruccion) ☭
    Requerimiento 4: En la cerradura epsilon considerar getClasificacion y getContenido ☭
    Requerimiento 5: Implementar el operador OR ☭
    Lenguaje -> lenguaje:identificador; ListaProducciones
    ListaProducciones -> snt flechita ListaSimbolos finProduccion 
    ListaSimbolos -> snt | st ListaSimbolos?
*/
namespace Generador
{
    public class Lenguaje : Sintaxis
    {
        string namesp;
        int numeroTabs;
        public Lenguaje()
        {
            numeroTabs=0;
            Console.WriteLine("Iniciando analisis gramatical.");
        }
        public Lenguaje(string nombre) : base(nombre)
        {
            numeroTabs=0;
            Console.WriteLine("Iniciando analisis gramatical.");
        }
        //Lenguaje -> lenguaje:identificador; ListaProducciones
        public void gramatica()
        {
            match("lenguaje");
            match(":");
            if (getClasificacion() == Clasificaciones.snt)
            {
                namesp=getContenido();
                match(Clasificaciones.snt);
            }
            else
            {
                namesp=getContenido();
                match(Clasificaciones.st);
            }
            match(";");
            Cabecera();
            match("{");
            ListaProducciones();
            match("}");
            numeroTabs--;
            Escribir("}");
            numeroTabs--;
            Escribir("}");
        }
        //ListaProducciones -> snt flechita ListaSimbolos finProduccion ListaProducciones?
        private void ListaProducciones()
        {
            Escribir("public void " + getContenido() + "()");
            match(Clasificaciones.snt);
            match(Clasificaciones.flechita);
            Escribir("{");
            numeroTabs++;
            ListaSimbolos();
            numeroTabs--;
            Escribir("}");
            match(Clasificaciones.finProduccion);
            if (getClasificacion() == Clasificaciones.snt)
            {
                ListaProducciones();
            }
        }
        //ListaSimbolos -> snt | st ListaSimbolos?
        private void ListaSimbolos()
        {
            if (getClasificacion() == Clasificaciones.snt)
            {
                Escribir(getContenido() + "();");
                match(Clasificaciones.snt);
            }
            else if (getClasificacion() == Clasificaciones.st)
            {
                if (esClasificacion(getContenido()))
                {
                    Escribir("match(clasificaciones." + getContenido() + ");");
                }
                else
                {
                    Escribir("match(\"" + getContenido() + "\");");
                }
                match(Clasificaciones.st);
            }
            else if (getClasificacion() == Clasificaciones.parentesisIzquierdo)
            {
                match(Clasificaciones.parentesisIzquierdo);
                if(esClasificacion(getContenido()))
                {
                    Escribir("if(getClasificacion() == clasificaciones." + getContenido() + ")");
                }
                else{
                    Escribir("if(getContenido() == \"" + getContenido() + "\")");
                }
                
                Escribir("{");
                numeroTabs++;
                if (esClasificacion(getContenido()))
                {
                    Escribir("match(clasificaciones." + getContenido() + ");");
                }
                else
                {
                    Escribir("match(\"" + getContenido() + "\");");
                }
                match(Clasificaciones.st);
                if (getClasificacion() == Clasificaciones.snt || getClasificacion() == Clasificaciones.st)
                {
                    ListaSimbolos();
                }
                match(Clasificaciones.parentesisDerecho);
                numeroTabs--;
                Escribir("}");
                match(Clasificaciones.cerraduraEpsilon);
            }
            else if (getClasificacion() == Clasificaciones.corcheteIzquierdo)
            {
                match(Clasificaciones.corcheteIzquierdo);
                if(esClasificacion(getContenido()))
                {
                    Escribir("if(getClasificacion() == clasificaciones." + getContenido() + ")");
                }
                else{
                    Escribir("if(getContenido() == \"" + getContenido() + "\")");
                }
                Escribir("{");
                numeroTabs++;
                if (esClasificacion(getContenido()))
                {
                    Escribir("match(clasificaciones." + getContenido() + ");");
                }
                else
                {
                    Escribir("match(\"" + getContenido() + "\");");
                }
                match(Clasificaciones.st);
                
                numeroTabs--;
                Escribir("}");
                if (getClasificacion() == Clasificaciones.or)
                {
                    ListaORs();
                }
                match(Clasificaciones.corcheteDerecho);
            }
            if (getClasificacion() == Clasificaciones.snt || getClasificacion() == Clasificaciones.st || getClasificacion() == Clasificaciones.parentesisIzquierdo||getClasificacion()==Clasificaciones.corcheteIzquierdo)
            {
                ListaSimbolos();
            }
        }
        //ListaORs -> st (| ListaORs)?
        private void ListaORs()
        {
            match(Clasificaciones.or);
            string or= getContenido();
            match(Clasificaciones.st);
            if(getClasificacion()==Clasificaciones.or){
                //"if(getContenido() == \"" + or + "\")");
                if(esClasificacion(or))
                {
                    Escribir("else if(getClasificacion() == clasificaciones." + or + ")");
                }
                else{
                    Escribir("else if(getContenido() == \"" + or + "\")");
                }
                Escribir("{");
                numeroTabs++;
                if (esClasificacion(or))
                {
                    Escribir("match(clasificaciones." + or + ");");
                }
                else
                {
                    Escribir("match(\"" + or + "\");");
                }
                numeroTabs--;
                Escribir("}");
                ListaORs();
            }
            else
            {
                Escribir("else");
                Escribir("{");
                numeroTabs++;
                if (esClasificacion(or))
                {
                    Escribir("match(clasificaciones." + or + ");");
                }
                else
                {
                    Escribir("match(\"" + or + "\");");
                }
                numeroTabs--;
                Escribir("}");
            }
            //Generar else ifs y el ultimo simbolo debe de ser else
        }
        private void Cabecera()
        {
            Escribir("using System;");
            Escribir("using System.Collections.Generic;");
            Escribir("using System.Text;");
            Escribir("namespace "+namesp);
            Escribir("{");
            numeroTabs++;
            Escribir("public class Lenguaje:Sintaxis");
            Escribir("{");
            numeroTabs++;
            Escribir("public Lenguaje()");
            Escribir("{");
            numeroTabs++;
            Escribir("Console.WriteLine(\"Iniciando analisis gramatical.\");");
            numeroTabs--;
            Escribir("}");
            Escribir("public Lenguaje(string nombre) : base(nombre)");
            Escribir("{");
            numeroTabs++;
            Escribir("Console.WriteLine(\"Iniciando analisis gramatical.\");");
            numeroTabs--;
            Escribir("}");
        }
        private void Escribir(string contenido){
            for(int i=numeroTabs;i>0;i--)
            {
                lenguaje.Write("    ");
            }
            lenguaje.WriteLine(contenido);
        }
    }
}