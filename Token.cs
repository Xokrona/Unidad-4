namespace Generador
{
    public class Token : Error
    {
      public enum Clasificaciones
        {
            snt, st, flechita, finProduccion, cerraduraEpsilon, 
            parentesisIzquierdo, parentesisDerecho, or, 
            corcheteIzquierdo, corcheteDerecho
            //snt->L+
            //st->L+ | clasificaciones.tipo | caracter
            //flechita -> ->
            //cerraduraEpsilon -> \?
            //parentesisIzquierdo -> \(
            //parentesisDerecho ->\)
            //or -> \|
            //corcheteIzquierdo -> \[
            //corcheteDerecho -> \]
        }
        private string Contenido;
        private Clasificaciones Clasificacion;
        protected bool esClasificacion(string clasificacion){
            switch(clasificacion){
                case "identificador":
                case "numero":
                case "asignacion":
                case "inicializacion":
                case "finSentencia,":
                case "operadorLogico":
                case "operadorRelacional":
                case "operadorTermino":
                case "operadorFactor":
                case "incrementoTermino":
                case "incrementoFactor":
                case "cadena":
                case "ternario":
                case "caracter":
                case "tipoDato":
                case "zona":
                case "condicion":
                case "ciclo":
                case "inicioBloque":
                case "finBloque":
                case "flujoEntrada":
                case "flujoSalida":
                    return true;
            }
            return false;
        }
        public void setContenido(string Contenido)
        {
            this.Contenido = Contenido;
        }
        public void setClasificacion(Clasificaciones Clasificacion)
        {
            this.Clasificacion = Clasificacion;
        }
        public string getContenido()
        {
            return Contenido;
        }
        public Clasificaciones getClasificacion()
        {
            return Clasificacion;
        }
    }
}