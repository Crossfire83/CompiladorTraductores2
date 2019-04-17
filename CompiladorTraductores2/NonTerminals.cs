using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorTraductores2
{
    //Regla 1
    public class Programa : NonTerminal
    {
        Definiciones defs;
        public Programa(ref Stack<StackElement> pila) : base(24)
        {
            pila.Pop();
            defs = pila.Pop() as Definiciones;
        }

        public override string ImprimeTipo()
        {
            return " programa ";
        }
    }

    //Regla 2 y 3
    public class Definiciones : NonTerminal
    {
        Definicion def;
        Definiciones defs;

        public Definiciones(ref Stack<StackElement> pila) : base(25)
        {
            pila.Pop();
            defs = pila.Pop() as Definiciones;
            pila.Pop();
            def = pila.Pop() as Definicion;
        }

        public Definiciones() : base(25) { def = null; defs = null; }

        public override string ImprimeTipo()
        {
            return " Definiciones ";
        }
    }

    //Regla 4 y 5
    public class Definicion : NonTerminal
    {
        DefVar var;
        DefFunc func;

        public Definicion(ref Stack<StackElement> pila) : base(26)
        {
            var = null;
            func = null;
            pila.Pop();
            StackElement elem = pila.Pop();
            if (elem is DefVar)
            {
                var = elem as DefVar;
            }
            else if (elem is DefFunc)
            {
                func = elem as DefFunc;
            }
        }

        public override string ImprimeTipo()
        {
            return " Definicion ";
        }
    }

    //Regla 6
    public class DefVar : NonTerminal
    {
        public Symbol tipo;
        public Symbol id;
        public ListaVar lvar;

        public DefVar(ref Stack<StackElement> pila) : base(27)
        {
            pila.Pop();//quita estado
            pila.Pop(); //quita ;
            pila.Pop(); //quita estado estado
            lvar = pila.Pop() as ListaVar; //quita ListaVar
            pila.Pop(); //quita estado
            id = pila.Pop().symbol; //quita Id
            pila.Pop(); //quita estado
            tipo = pila.Pop().symbol;
        }

        public override string ImprimeTipo()
        {
            return " DefVar ";
        }
    }

    //Regla 7 y 8
    public class ListaVar : NonTerminal
    {
        Symbol id;
        ListaVar lvar;

        public ListaVar(ref Stack<StackElement> pila) : base(28)
        {
            pila.Pop();
            lvar = pila.Pop() as ListaVar;
            pila.Pop();
            id = pila.Pop().symbol;
            pila.Pop();
            pila.Pop();  //quita coma
        }

        public ListaVar() : base(28) { id = null; lvar = null; }

        public override string ImprimeTipo()
        {
            return " ListaVar ";
        }

    }

    //Regla 9
    public class DefFunc : NonTerminal
    {
        private Symbol tipo;
        private Symbol id;
        private Parametros parametros;
        private BloqFunc bloqueFunc;

        public DefFunc(ref Stack<StackElement> pila) : base(29)
        {
            pila.Pop();//quita estado
            bloqueFunc = pila.Pop() as BloqFunc;//quita <bloqfunc> //aqui me quede
            pila.Pop();//quita estado
            pila.Pop();//quita )
            pila.Pop();//quita estado
            parametros = pila.Pop() as Parametros;
            pila.Pop();//quita estado
            pila.Pop();//quita (
            pila.Pop();//quita estado
            id = pila.Pop().symbol;//quita id
            pila.Pop();//quita estado
            tipo = pila.Pop().symbol;//quita el tipo
        }

        public override string ImprimeTipo()
        {
            return " DefFunc ";
        }
    }

    //Regla 10 y 11
    public class Parametros : NonTerminal
    {
        private Symbol tipo;
        private Symbol id;
        private ListaParam listaParams;

        public Parametros(ref Stack<StackElement> pila) : base(30)
        {
            pila.Pop();//quita estado
            listaParams = pila.Pop() as ListaParam;
            pila.Pop();//quita estado
            id = pila.Pop().symbol;
            pila.Pop(); //quita estado
            tipo = pila.Pop().symbol;//quita el tipo
        }

        public Parametros() : base(30)
        {
            tipo = null;
            id = null;
            listaParams = null;
        }

        public override string ImprimeTipo()
        {
            return " Parametros ";
        }
    }

    //Regla 12 y 13
    public class ListaParam : NonTerminal
    {
        private Symbol tipo;
        private Symbol id;
        private ListaParam listaParams;

        public ListaParam(ref Stack<StackElement> pila) : base(31)
        {

            pila.Pop();//quita estado
            listaParams = pila.Pop() as ListaParam;
            pila.Pop();//quita estado
            id = pila.Pop().symbol;
            pila.Pop(); //quita estado
            tipo = pila.Pop().symbol;//quita el tipo
            pila.Pop(); //quita estado
            pila.Pop(); //quita coma
        }

        public ListaParam() : base(31)
        {
            tipo = null;
            id = null;
            listaParams = null;
        }

        public override string ImprimeTipo()
        {
            return " ListaParam ";
        }
    }

    //Regla 14
    public class BloqFunc : NonTerminal
    {
        DefLocales locales;

        public BloqFunc(ref Stack<StackElement> pila) : base(32)
        {
            pila.Pop();
            pila.Pop(); //quita }
            pila.Pop();
            locales = pila.Pop() as DefLocales;
            pila.Pop();
            pila.Pop();  //quita {
        }

        public override string ImprimeTipo()
        {
            return " DefLocales ";
        }
    }

    //Regla 15 y 16
    public class DefLocales : NonTerminal
    {
        DefLocal def;
        DefLocales defs;

        public DefLocales(ref Stack<StackElement> pila) : base(33)
        {
            pila.Pop();
            defs = pila.Pop() as DefLocales;
            pila.Pop();
            def = pila.Pop() as DefLocal;
        }

        public DefLocales() : base(33) { def = null; defs = null; }

        public override string ImprimeTipo()
        {
            return " DefLocales ";
        }
    }

    //Regla 17 y 18
    public class DefLocal : NonTerminal
    {
        DefVar defv;
        Sentencia sent;

        public DefLocal(ref Stack<StackElement> pila) : base(34)
        {
            defv = null;
            sent = null;
            pila.Pop();
            StackElement elem = pila.Pop();
            if (elem is DefVar)
            {
                defv = elem as DefVar;
            }
            else if (elem is Sentencia){
                sent = elem as Sentencia;
            }
        }

        public override string ImprimeTipo()
        {
            return " DefLocal ";
        }
    }

    //Regla 19 y 20
    public class Sentencias : NonTerminal
    {
        Sentencia sent;
        Sentencias sents;

        public Sentencias(ref Stack<StackElement> pila) : base(35)
        {
            pila.Pop();
            sents = pila.Pop() as Sentencias;
            pila.Pop();
            sent = pila.Pop() as Sentencia;
        }

        public Sentencias() : base(35) { sent = null; sents = null; }

        public override string ImprimeTipo()
        {
            return " Sentencias ";
        }
    }

    //Reglas 21, 22, 23, 24 y 25
    public abstract class Sentencia : NonTerminal {

        protected Sentencia() : base(36) { }

        public override string ImprimeTipo()
        {
            return " Sentencia ";
        }
    }

    //Regla 21
    public class Asignacion : Sentencia
    {
        Symbol id;
        Expresion expresion;

        public Asignacion(ref Stack<StackElement> pila) : base() //<Sentencia> ::= id = <Expresion> ;
        {
            pila.Pop();
            pila.Pop(); //quita la ;
            pila.Pop();
            expresion = pila.Pop() as Expresion; //quita expresion
            pila.Pop();
            pila.Pop(); //quita =
            pila.Pop();
            id = pila.Pop().symbol; //quita id
        }
    }

    //Regla 22
    public class If : Sentencia
    {
        Expresion expresion;
        SentenciaBloque sentenciabloque;
        Otro otro;

        public If(ref Stack<StackElement> pila) : base()
        {
            pila.Pop();
            otro = pila.Pop() as Otro; //quita otro
            pila.Pop();
            sentenciabloque = pila.Pop() as SentenciaBloque; //quita sentencia bloque
            pila.Pop();
            pila.Pop(); //quita )
            pila.Pop();
            expresion = pila.Pop() as Expresion; //quita expresion
            pila.Pop();
            pila.Pop(); //quita (
            pila.Pop();
            pila.Pop(); //quita if
        }
    }

    //Regla 23
    public class While : Sentencia
    {
        Expresion expresion;
        Bloque bloque;

        public While(ref Stack<StackElement> pila) : base()
        {
            pila.Pop();
            bloque = pila.Pop() as Bloque; //quita bloque
            pila.Pop();
            pila.Pop(); //quita )
            pila.Pop();
            expresion = pila.Pop() as Expresion; //quita expresion
            pila.Pop();
            pila.Pop(); //quita (
            pila.Pop();
            pila.Pop(); //quita while
        }
    }

    //Regla 24
    public class Return : Sentencia
    {
        ValorRegresa exp;

        public Return(ref Stack<StackElement> pila) : base()
        {
            pila.Pop();
            pila.Pop(); //quita ;
            pila.Pop();
            exp = pila.Pop() as ValorRegresa; //quita expresion
            pila.Pop();
            pila.Pop(); //quita return
        }
    }

    //Regla 25
    public class SentenciaLLama : Sentencia
    {
        LlamadaFunc llamadaFunc;

        public SentenciaLLama(ref Stack<StackElement> pila) : base()
        {
            pila.Pop();
            pila.Pop(); //quita ;
            pila.Pop();
            llamadaFunc = pila.Pop() as LlamadaFunc; //quita llamadaFunc
        }
    }

    //Regla 26 y 27
    public class Otro : NonTerminal
    {
        public SentenciaBloque sentBloq;

        public Otro(ref Stack<StackElement> pila) : base(37)
        {
            pila.Pop();
            sentBloq = pila.Pop() as SentenciaBloque; //quita SentenciaBloque
            pila.Pop();
            pila.Pop(); //quita else
        }

        public Otro() : base(37) { sentBloq = null; }

        public override string ImprimeTipo()
        {
            return " Otro ";
        }
    }

    //Regla 28
    public class Bloque : NonTerminal
    {
        Sentencias sents;

        public Bloque(ref Stack<StackElement> pila) : base(38)
        {
            pila.Pop();
            pila.Pop(); //quita }
            pila.Pop();
            sents = pila.Pop() as Sentencias; //quita SentenciaBloque
            pila.Pop();
            pila.Pop(); //quita {
        }

        public override string ImprimeTipo()
        {
            return " Bloque ";
        }
    }

    //Regla 29 y 30
    public class ValorRegresa : NonTerminal
    {
        Expresion exp;

        public ValorRegresa(ref Stack<StackElement> pila) : base(39)
        {
            pila.Pop();
            exp = pila.Pop() as Expresion;
        }

        public ValorRegresa() : base(39) { exp = null; }

        public override string ImprimeTipo()
        {
            return " ValorRegresa ";
        }
    }

    //Regla 31 y 32
    public class Argumentos : NonTerminal
    {
        Expresion expr;
        ListaArgumentos lArgs;

        public Argumentos(ref Stack<StackElement> pila) : base(40)
        {
            pila.Pop();
            lArgs = pila.Pop() as ListaArgumentos;
            pila.Pop();
            expr = pila.Pop() as Expresion;
        }

        public Argumentos() : base(40) { lArgs = null; expr = null; }

        public override string ImprimeTipo()
        {
            return " Argumentos ";
        }
    }

    //Regla 33 y 34
    public class ListaArgumentos : NonTerminal
    {
        Expresion expr;
        ListaArgumentos lArgs;

        public ListaArgumentos(ref Stack<StackElement> pila) : base(41)
        {
            pila.Pop();
            lArgs = pila.Pop() as ListaArgumentos;
            pila.Pop();
            expr = pila.Pop() as Expresion;
            pila.Pop();
            pila.Pop(); //quita coma
        }

        public ListaArgumentos() : base(41) { lArgs = null; expr = null; }

        public override string ImprimeTipo()
        {
            return " ListaArgumentos ";
        }
    }

    //Reglas 35, 36, 37, 38 y 39
    public class Termino : NonTerminal
    {
        Symbol symb;
        LlamadaFunc lfunc;

        public Termino(ref Stack<StackElement> pila) : base(42)
        {
            symb = null;
            lfunc = null;
            pila.Pop();
            StackElement elem = pila.Pop();
            if (elem is Terminal)
            {
                symb = elem.symbol;
            }
            else if (elem is LlamadaFunc){
                lfunc = elem as LlamadaFunc;
            }

        }

        public override string ImprimeTipo()
        {
            return " Termino ";
        }
    }

    //Regla 40
    public class LlamadaFunc : NonTerminal
    {
        Symbol id;
        Argumentos argumentos;

        public LlamadaFunc(ref Stack<StackElement> pila) : base(43)
        {
            pila.Pop();
            pila.Pop(); //quita )
            pila.Pop();
            argumentos = pila.Pop() as Argumentos; //quita expresion
            pila.Pop();
            pila.Pop(); //quita (
            pila.Pop();
            id = pila.Pop().symbol; //quita id
        }

        public override string ImprimeTipo()
        {
            return " LLamadaFunc ";
        }
    }

    //Regla 41 y 42
    public class SentenciaBloque : NonTerminal
    {
        Sentencia sent;
        Bloque bloq;

        public SentenciaBloque(ref Stack<StackElement> pila) : base(44)
        {
            sent = null;
            bloq = null;
            pila.Pop();
            StackElement elem = pila.Pop();
            if (elem is Sentencia)
            {
                sent = elem as Sentencia;
            }
            else if (elem is Bloque) {
                bloq = elem as Bloque;
            }
        }

        public override string ImprimeTipo()
        {
            return " SentenciaBloque ";
        }
    }

    //Regla 43 a 52
    public class Expresion : NonTerminal
    {
        Expresion expr;
        Termino ter;

        //Regla 43 y 52
        public Expresion(ref Stack<StackElement> pila) : base(45)
        {
            expr = null;
            ter = null;
            pila.Pop();
            StackElement elem = pila.Pop(); //quita termino o )
            if (elem is Termino)
            {
                ter = elem as Termino;
            }
            else if (elem is Terminal) {
                pila.Pop();
                expr = pila.Pop() as Expresion;
                pila.Pop();
                pila.Pop(); //quita (
            }
        }

        public Expresion() : base(45) { expr = null; ter = null; }

        public override string ImprimeTipo()
        {
            return " Expresion ";
        }
    }

    //Regla 44 y 45
    public class Operacion1 : Expresion
    {
        Expresion der;

        public Operacion1(ref Stack<StackElement> pila) : base()
        {
            pila.Pop();
            der = pila.Pop() as Expresion; //quita expresion
            pila.Pop();
            symbol = pila.Pop().symbol; //quita el operador
        }

    }

    //Reglas 46 a 51
    public class Operacion2 : Expresion
    {
        Expresion der;
        Expresion izq;
        public Operacion2(ref Stack<StackElement> pila) : base()
        {
            pila.Pop();
            der = pila.Pop() as Expresion;//quita exprsion
            pila.Pop();
            symbol = pila.Pop().symbol;//quita el operador
            pila.Pop();
            izq = pila.Pop() as Expresion;//quita expresion

        }
    }
}
