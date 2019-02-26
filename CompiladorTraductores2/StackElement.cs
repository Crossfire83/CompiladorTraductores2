using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorTraductores2
{
    public class StackElement
    {
        public StackElement Next;
        public Symbol symbol;
        public StackElement() {

        }
    }
    public class State : StackElement
    {
        public int transicion;
        public State(int e) { transicion = e; }
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
    }

    public class Programa : StackElement
    {
        public Programa(ref Stack pilaSintactica)
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
    }

    public class Tipo : StackElement
    {
        public Tipo(Symbol s)
        {
            symbol = s;
        }
    }

    public class DefVar : StackElement
    {
        public StackElement tipo;
        public StackElement id;
        public StackElement lvar;

        public DefVar(ref Stack pila)
        {
            tipo = new StackElement();
            id = new StackElement();
            lvar = new StackElement();
            pila.Pop();//quita estado
            pila.Pop(); //quita  ;
            pila.Pop(); //quita estado estado
            lvar = (StackElement)pila.Pop(); //quita ListaVar
            pila.Pop(); //quita estado
            id = new Identificador(((Terminal)pila.Pop()).symbol); //quita Id

            pila.Pop(); //quita estado
            tipo = new Tipo(((Terminal)pila.Pop()).symbol); //quita tipo
            id.symbol.name = tipo.symbol.name;
        }

        public DefVar()
        {
        }
    }

    public class DefFunc : StackElement
    {
        private StackElement tipo;
        private StackElement id;
        private StackElement parametros;
        private StackElement bloqueFunc;

        public DefFunc(ref Stack pila)
        {
            pila.Pop();//quita estado
            bloqueFunc = (StackElement)pila.Pop();//quita <bloqfunc> //aqui me quede
            pila.Pop();//quita estado
            pila.Pop();//quita )
            pila.Pop();//quita estado
            parametros = (StackElement)pila.Pop();//quita <parametros>
            pila.Pop();//quita estado
            pila.Pop();//quita (
            pila.Pop();//quita estado
            id = new Identificador(((Terminal)pila.Pop()).symbol);//quita id
            pila.Pop();//quita estado
            tipo = new Tipo(((Terminal)pila.Pop()).symbol);//quita el tipo
            id.symbol.name = tipo.symbol.name;
        }
    }

    public class Parametros : StackElement
    {
        private StackElement tipo;
        private StackElement id;
        private StackElement listaParams;
        public Parametros(ref Stack pila)
        {

            pila.Pop();//quita estado

            listaParams = (StackElement)pila.Pop();//quita la lista de aprametros
            pila.Pop();//quita estado
            id = new Identificador(((Terminal)pila.Pop()).symbol);//quita el id
            pila.Pop(); //quita estado

            tipo = new Tipo(((Terminal)pila.Pop()).symbol);//quita el tipo
            id.symbol.name = tipo.symbol.name;
        }
    }

    public class Asignacion : StackElement
    {
        StackElement id;
        StackElement expresion;
        public Asignacion(ref Stack pila) //<Sentencia> ::= id = <Expresion> ;
        {
            pila.Pop();
            pila.Pop(); //quita la ;
            pila.Pop();
            expresion = (StackElement)pila.Pop(); //quita expresion
            pila.Pop();
            pila.Pop(); //quita =
            pila.Pop();
            id = new Identificador(((Terminal)pila.Pop()).symbol); //quita id
        }
    }

    public class If : StackElement
    {
        StackElement expresion;
        StackElement sentenciabloque;
        StackElement otro;
        public If(ref Stack pila)
        {
            pila.Pop();
            otro = (StackElement)pila.Pop(); //quita otro
            pila.Pop();
            sentenciabloque = (StackElement)pila.Pop(); //quita sentencia bloque
            pila.Pop();
            pila.Pop(); //quita )
            pila.Pop();
            expresion = (StackElement)pila.Pop(); //quita expresion
            pila.Pop();
            pila.Pop(); //quita (
            pila.Pop();
            pila.Pop(); //quita if
        }
    }

    public class While : StackElement
    {
        StackElement expresion;
        StackElement bloque;

        public While(ref Stack pila)
        {
            pila.Pop();
            bloque = (StackElement)pila.Pop(); //quita bloque
            pila.Pop();
            pila.Pop(); //quita )
            pila.Pop();
            expresion = (StackElement)pila.Pop(); //quita expresion
            pila.Pop();
            pila.Pop(); //quita (
            pila.Pop();
            pila.Pop(); //quita while
        }
    }

    public class DoWhile : StackElement
    {
        StackElement bloque;
        StackElement expresion;

        public DoWhile(ref Stack pila)
        {
            pila.Pop();
            pila.Pop(); //quita ;
            pila.Pop();
            pila.Pop(); //quita )
            pila.Pop();
            expresion = (StackElement)pila.Pop(); //quita exprecion
            pila.Pop();
            pila.Pop(); //quita (
            pila.Pop();
            pila.Pop(); //quita el while
            pila.Pop();
            bloque = (StackElement)pila.Pop(); //quita bloque
            pila.Pop();
            pila.Pop(); //quita do
        }
    }

    public class For : StackElement
    {
        StackElement senbloque;
        StackElement expresion1;
        StackElement expresion2;
        StackElement expresion3;
        StackElement id;

        public For(ref Stack pila)
        {
            pila.Pop();
            senbloque = (StackElement)pila.Pop(); //quita senteciabloque
            pila.Pop();
            expresion3 = (StackElement)pila.Pop(); //quita expresion
            pila.Pop();
            pila.Pop(); //quita ;
            pila.Pop();
            expresion2 = (StackElement)pila.Pop(); //quita expresion
            pila.Pop();
            expresion1 = (StackElement)pila.Pop(); //quita expresion
            pila.Pop();
            pila.Pop(); //quita =
            pila.Pop();
            id = new Identificador(((Terminal)pila.Pop()).symbol); //quita id
            pila.Pop();
            pila.Pop(); //quita for
        }
    }

    public class Return : StackElement
    {
        StackElement expresion;
        public Return(ref Stack pila)
        {
            pila.Pop();
            pila.Pop(); //quita ;
            pila.Pop();
            expresion = (StackElement)pila.Pop(); //quita expresion
            pila.Pop();
            pila.Pop(); //quita return
        }
    }

    public class Constante : StackElement
    {
        public Constante(Symbol s)
        {
            symbol = s;
        }
    }

    public class LlamadaFunc : StackElement
    {
        StackElement id;
        StackElement argumentos;
        public LlamadaFunc(ref Stack pila)
        {
            pila.Pop();
            pila.Pop();//quita )
            pila.Pop();
            argumentos = (StackElement)pila.Pop();//quita exprecion
            pila.Pop();
            pila.Pop();//quita (
            pila.Pop();
            id = new Identificador(((Terminal)pila.Pop()).symbol);//quita id
        }
    }

    public class Operacion1 : StackElement
    {
        StackElement der;

        public Operacion1(ref Stack pila)
        {
            pila.Pop();
            der = (StackElement)pila.Pop();//quita exprsion
            pila.Pop();
            symbol = ((Terminal)pila.Pop()).symbol;//quita el operador
        }
    }

    public class Operacion2 : StackElement
    {
        StackElement der;
        StackElement izq;
        public Operacion2(ref Stack pila)
        {
            pila.Pop();
            der = (StackElement)pila.Pop();//quita exprsion
            pila.Pop();
            symbol = ((Terminal)pila.Pop()).symbol;//quita el operador
            pila.Pop();
            izq = (StackElement)pila.Pop();//quita expresion

        }
    }
}
