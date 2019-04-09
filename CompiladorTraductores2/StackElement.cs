using System.Collections.Generic;

namespace CompiladorTraductores2
{
    public class StackElement
    {
        public StackElement Next;
        public Symbol symbol;
        public StackElement() {

        }

        public override string ToString()
        {
            return symbol.ToString();
        }

        public virtual string imprimeTipo() { return "Nodo"; }
    }
    public class State : StackElement
    {
        public int transicion;
        public State(int e) { transicion = e; }

        public override string ToString()
        {
            return "Y: " + transicion.ToString();
        }

        public override string imprimeTipo()
        {
            return transicion.ToString();
        }
    }

    public class Terminal : StackElement
    {
        public int columna;
        public StackElement element;

        public Terminal(Symbol c)
        {
            element = new StackElement
            {
                symbol = c
            };
        }

        public override string ToString()
        {
            return element.ToString();
        }

        public override string imprimeTipo()
        {
            return " " + element.symbol.name + " ";
        }
    }

    public class NonTerminal : StackElement
    {
        public int columna;
        public StackElement element;

        public NonTerminal(int c)
        {
            columna = c;
            element = new StackElement();
        }

        public override string ToString()
        {
            return "Columna: " + columna.ToString();
        }

        public override string imprimeTipo()
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

    public class Programa : StackElement
    {
        public Programa(ref Stack<StackElement> pilaSintactica)
        {
            pilaSintactica.Pop();
            Next = (NonTerminal)pilaSintactica.Pop();
        }
    }

    public class Identificador : StackElement
    {
        public Identificador(Symbol s)
        {
            symbol = s;
        }

        public override string imprimeTipo()
        {
            return "id";
        }
    }

    public class Tipo : StackElement
    {
        public Tipo(Symbol s)
        {
            symbol = s;
        }

        public override string imprimeTipo()
        {
            return "tipo";
        }
    }

    public class DefVar : StackElement
    {
        public StackElement tipo;
        public StackElement id;
        public StackElement lvar;

        public DefVar(ref Stack<StackElement> pila)
        {
            tipo = new StackElement();
            id = new StackElement();
            lvar = new StackElement();
            pila.Pop();//quita estado
            pila.Pop(); //quita  ;
            pila.Pop(); //quita estado estado
            lvar = pila.Pop(); //quita ListaVar
            pila.Pop(); //quita estado
            id = new Identificador(((Terminal)pila.Pop()).symbol); //quita Id

            pila.Pop(); //quita estado
            tipo = new Tipo(((Terminal)pila.Pop()).symbol); //quita tipo
            //id.symbol.name = tipo.symbol.name;
        }

        public DefVar()
        {
        }

        public override string imprimeTipo()
        {
            return "DefVar";
        }
    }

    public class DefFunc : StackElement
    {
        private StackElement tipo;
        private StackElement id;
        private StackElement parametros;
        private StackElement bloqueFunc;

        public DefFunc(ref Stack<StackElement> pila)
        {
            pila.Pop();//quita estado
            bloqueFunc = pila.Pop();//quita <bloqfunc> //aqui me quede
            pila.Pop();//quita estado
            pila.Pop();//quita )
            pila.Pop();//quita estado
            parametros = pila.Pop();//quita <parametros>
            pila.Pop();//quita estado
            pila.Pop();//quita (
            pila.Pop();//quita estado
            id = new Identificador(((Terminal)pila.Pop()).symbol);//quita id
            pila.Pop();//quita estado
            tipo = new Tipo(((Terminal)pila.Pop()).symbol);//quita el tipo
            //id.symbol.name = tipo.symbol.name;
        }

        public override string imprimeTipo()
        {
            return "DefFunc";
        }
    }

    public class Parametros : StackElement
    {
        private StackElement tipo;
        private StackElement id;
        private StackElement listaParams;
        public Parametros(ref Stack<StackElement> pila)
        {

            pila.Pop();//quita estado

            listaParams = pila.Pop();//quita la lista de aprametros
            pila.Pop();//quita estado
            id = new Identificador(((Terminal)pila.Pop()).symbol);//quita el id
            pila.Pop(); //quita estado

            tipo = new Tipo(((Terminal)pila.Pop()).symbol);//quita el tipo
            //id.symbol.name = tipo.symbol.name;
        }

        public override string imprimeTipo()
        {
            return "Parametros";
        }
    }

    public class Asignacion : StackElement
    {
        StackElement id;
        StackElement expresion;
        public Asignacion(ref Stack<StackElement> pila) //<Sentencia> ::= id = <Expresion> ;
        {
            pila.Pop();
            pila.Pop(); //quita la ;
            pila.Pop();
            expresion = pila.Pop(); //quita expresion
            pila.Pop();
            pila.Pop(); //quita =
            pila.Pop();
            id = new Identificador(((Terminal)pila.Pop()).symbol); //quita id
        }

        public override string imprimeTipo()
        {
            return "Asignacion";
        }
    }

    public class If : StackElement
    {
        StackElement expresion;
        StackElement sentenciabloque;
        StackElement otro;
        public If(ref Stack<StackElement> pila)
        {
            pila.Pop();
            otro = pila.Pop(); //quita otro
            pila.Pop();
            sentenciabloque = pila.Pop(); //quita sentencia bloque
            pila.Pop();
            pila.Pop(); //quita )
            pila.Pop();
            expresion = pila.Pop(); //quita expresion
            pila.Pop();
            pila.Pop(); //quita (
            pila.Pop();
            pila.Pop(); //quita if
        }

        public override string imprimeTipo()
        {
            return "if";
        }
    }

    public class While : StackElement
    {
        StackElement expresion;
        StackElement bloque;

        public While(ref Stack<StackElement> pila)
        {
            pila.Pop();
            bloque = pila.Pop(); //quita bloque
            pila.Pop();
            pila.Pop(); //quita )
            pila.Pop();
            expresion = pila.Pop(); //quita expresion
            pila.Pop();
            pila.Pop(); //quita (
            pila.Pop();
            pila.Pop(); //quita while
        }

        public override string imprimeTipo()
        {
            return "while";
        }
    }

    public class DoWhile : StackElement
    {
        StackElement bloque;
        StackElement expresion;

        public DoWhile(ref Stack<StackElement> pila)
        {
            pila.Pop();
            pila.Pop(); //quita ;
            pila.Pop();
            pila.Pop(); //quita )
            pila.Pop();
            expresion = pila.Pop(); //quita exprecion
            pila.Pop();
            pila.Pop(); //quita (
            pila.Pop();
            pila.Pop(); //quita el while
            pila.Pop();
            bloque = pila.Pop(); //quita bloque
            pila.Pop();
            pila.Pop(); //quita do
        }

        public override string imprimeTipo()
        {
            return "DoWhile";
        }
    }

    public class For : StackElement
    {
        StackElement senbloque;
        StackElement expresion1;
        StackElement expresion2;
        StackElement expresion3;
        StackElement id;

        public For(ref Stack<StackElement> pila)
        {
            pila.Pop();
            senbloque = pila.Pop(); //quita senteciabloque
            pila.Pop();
            expresion3 = pila.Pop(); //quita expresion
            pila.Pop();
            pila.Pop(); //quita ;
            pila.Pop();
            expresion2 = pila.Pop(); //quita expresion
            pila.Pop();
            expresion1 = pila.Pop(); //quita expresion
            pila.Pop();
            pila.Pop(); //quita =
            pila.Pop();
            id = new Identificador(((Terminal)pila.Pop()).symbol); //quita id
            pila.Pop();
            pila.Pop(); //quita for
        }

        public override string imprimeTipo()
        {
            return "for";
        }
    }

    public class Return : StackElement
    {
        StackElement expresion;
        public Return(ref Stack<StackElement> pila)
        {
            pila.Pop();
            pila.Pop(); //quita ;
            pila.Pop();
            expresion = pila.Pop(); //quita expresion
            pila.Pop();
            pila.Pop(); //quita return
        }

        public override string imprimeTipo()
        {
            return "return";
        }
    }

    public class Constante : StackElement
    {
        public Constante(Symbol s)
        {
            symbol = s;
        }

        public override string imprimeTipo()
        {
            return "constante";
        }
    }

    public class LlamadaFunc : StackElement
    {
        StackElement id;
        StackElement argumentos;
        public LlamadaFunc(ref Stack<StackElement> pila)
        {
            pila.Pop();
            pila.Pop();//quita )
            pila.Pop();
            argumentos = pila.Pop();//quita exprecion
            pila.Pop();
            pila.Pop();//quita (
            pila.Pop();
            id = new Identificador(((Terminal)pila.Pop()).symbol);//quita id
        }

        public override string imprimeTipo()
        {
            return "LLamadaFunc";
        }
    }

    public class Operacion1 : StackElement
    {
        StackElement der;

        public Operacion1(ref Stack<StackElement> pila)
        {
            pila.Pop();
            der = pila.Pop();//quita exprsion
            pila.Pop();
            symbol = ((Terminal)pila.Pop()).symbol;//quita el operador
        }

        public override string imprimeTipo()
        {
            return "Op1";
        }
    }

    public class Operacion2 : StackElement
    {
        StackElement der;
        StackElement izq;
        public Operacion2(ref Stack<StackElement> pila)
        {
            pila.Pop();
            der = pila.Pop();//quita exprsion
            pila.Pop();
            symbol = ((Terminal)pila.Pop()).symbol;//quita el operador
            pila.Pop();
            izq = pila.Pop();//quita expresion

        }

        public override string imprimeTipo()
        {
            return "Op2";
        }
    }
}
