using System.Collections.Generic;

namespace CompiladorTraductores2
{
    public abstract class StackElement
    {
        public Symbol symbol;
        public string ambito;
        protected StackElement() {

        }

        public override string ToString()
        {
            return symbol.ToString();
        }

        public abstract string ImprimeTipo();

        public virtual void ValidaTipos(ref List<ElementoTabla> SymbolTable, ref List<string> errores) { }
    }

    public class State : StackElement
    {
        public int transicion;
        public State(int e) { transicion = e; }

        public override string ToString()
        {
            return "Y: " + transicion.ToString();
        }

        public override string ImprimeTipo()
        {
            return transicion.ToString();
        }
    }

    public class Terminal : StackElement
    {
        public int columna;

        public Terminal(Symbol c, int col)
        {
            symbol = c;
            columna = col;
        }

        public Terminal(Symbol c)
        {
            symbol = c;
            columna = (int)symbol.type;
        }

        public override string ToString()
        {
            return symbol.ToString();
        }

        public override string ImprimeTipo()
        {
            return " " + symbol.name + " ";
        }
    }

    public abstract class NonTerminal : StackElement
    {
        public int columna;
        //public StackElement element;
        public bool containsChildren;

        protected NonTerminal(int c)
        {
            columna = c;
            containsChildren = true;
            //element = new StackElement();
        }

        public override string ToString()
        {
            return "Columna: " + columna.ToString();
        }

        public override string ImprimeTipo()
        {
            switch (columna) {
                case 24:
                    return " programa ";
                case 25:
                    return " Definiciones ";
                case 26:
                    return " Definicion ";
                case 27:
                    return " DefVar ";
                case 28:
                    return " ListaVar ";
                case 29:
                    return " DefFunc ";
                case 30:
                    return " Parametros ";
                case 31:
                    return " ListaParam ";
                case 32:
                    return " BloqFunc ";
                case 33:
                    return " DefLocales ";
                case 34:
                    return " DefLocal ";
                case 35:
                    return " Sentencias ";
                case 36:
                    return " Sentencia ";
                case 37:
                    return " Otro ";
                case 38:
                    return " Bloque ";
                case 39:
                    return " ValorRegresa ";
                case 40:
                    return " Argumentos ";
                case 41:
                    return " ListaArgumentos ";
                case 42:
                    return " Termino ";
                case 43:
                    return " LlamadaFunc ";
                case 44:
                    return " SentenciaBloque ";
                case 45:
                    return " Expresion ";
                default:
                    return " NoTerminal ";
            }
        }
    }

    //public class Identificador : StackElement
    //{
    //    public Identificador(Symbol s)
    //    {
    //        symbol = s;
    //    }

    //    public override string ImprimeTipo()
    //    {
    //        return " id ";
    //    }
    //}

    public class Tipo : StackElement
    {
        public Tipo(Symbol s)
        {
            symbol = s;
        }

        public override string ImprimeTipo()
        {
            return " tipo ";
        }
    }

    public class Constante : StackElement
    {
        public Constante(Symbol s)
        {
            symbol = s;
        }

        public override string ImprimeTipo()
        {
            return " constante ";
        }
    }

    

    


}
