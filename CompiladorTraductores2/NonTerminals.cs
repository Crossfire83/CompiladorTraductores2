using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompiladorTraductores2
{
    //Regla 1
    public class Programa : NonTerminal
    {
        public Definiciones defs { get; private set; }
        public Programa(ref Stack<StackElement> pila) : base(24)
        {
            pila.Pop();
            defs = pila.Pop() as Definiciones;
        }

        public override string ImprimeTipo()
        {
            return " programa ";
        }

        public override void ValidaTipos(ref List<ElementoTabla> SymbolTable, ref List<string> errores)
        {
            ambito = "global";
            defs.ambito = ambito;
            defs.ValidaTipos(ref SymbolTable, ref errores);
        }
    }

    //Regla 2 y 3
    public class Definiciones : NonTerminal
    {
        public Definicion def { get; private set; }
        public Definiciones defs { get; private set; }

        public Definiciones(ref Stack<StackElement> pila) : base(25)
        {
            pila.Pop();
            defs = pila.Pop() as Definiciones;
            pila.Pop();
            def = pila.Pop() as Definicion;
        }

        public Definiciones() : base(25) { def = null; defs = null; containsChildren = false; }

        public override string ImprimeTipo()
        {
            return " Definiciones ";
        }

        public override void ValidaTipos(ref List<ElementoTabla> SymbolTable, ref List<string> errores)
        {
            def.ambito = ambito;
            def.ValidaTipos(ref SymbolTable, ref errores);
            if (defs.containsChildren) {
                defs.ambito = ambito;
                defs.ValidaTipos(ref SymbolTable, ref errores);
            }
        }
    }

    //Regla 4 y 5
    public class Definicion : NonTerminal
    {
        public DefVar var { get; private set; }
        public DefFunc func { get; private set; }

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

        public NonTerminal GetChild() {
            if (var != null) {
                return var;
            }
            return func;
        }

        public override void ValidaTipos(ref List<ElementoTabla> SymbolTable, ref List<string> errores)
        {
            if (var != null)
            {
                var.ambito = ambito;
                var.ValidaTipos(ref SymbolTable, ref errores);
            }
            else
            {
                func.ambito = ambito;
                func.ValidaTipos(ref SymbolTable, ref errores);
            }
        }
    }

    //Regla 6
    public class DefVar : NonTerminal
    {
        public Symbol tipo { get; private set; }
        public Symbol id { get; private set; }
        public ListaVar lvar { get; private set; }

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
        
        private DefVar() : base(27) { }

        public override string ImprimeTipo()
        {
            return " DefVar ";
        }

        public override void ValidaTipos(ref List<ElementoTabla> tabsim, ref List<string> errores)
        {
            List<ElementoTabla> result = tabsim.Where(i => i.Id == id.value && i.Ambito == ambito).ToList();

            if (result.Count == 0)
            {
                tabsim.Add(new ElementoTabla(id.value, tipo.value, ambito));
            }
            else
            {
                errores.Add("La variable " + id.value + "ya fue declarada dentro de " + ambito);
            }
            ListaVar aux = lvar;
            if (aux.containsChildren) {
                DefVar temp = new DefVar
                {
                    tipo = tipo,
                    id = aux.id,
                    lvar = aux.lvar,
                    containsChildren = true
                };
                temp.ambito = ambito;
                temp.ValidaTipos(ref tabsim, ref errores);
            }
        }
    }

    //Regla 7 y 8
    public class ListaVar : NonTerminal
    {
        public Symbol id { get; private set; }
        public ListaVar lvar { get; private set; }

        public ListaVar(ref Stack<StackElement> pila) : base(28)
        {
            pila.Pop();
            lvar = pila.Pop() as ListaVar;
            pila.Pop();
            id = pila.Pop().symbol;
            pila.Pop();
            pila.Pop();  //quita coma
        }

        public ListaVar() : base(28) { id = null; lvar = null; containsChildren = false; }

        public override string ImprimeTipo()
        {
            return " ListaVar ";
        }

        //TODO:
    }

    //Regla 9
    public class DefFunc : NonTerminal
    {
        public Symbol tipo { get; private set; }
        public Symbol id { get; private set; }
        public Parametros parametros { get; private set; }
        public BloqFunc bloqueFunc { get; private set; }

        public string CadenaParam { get; private set; }

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

        public bool ExisteFunc(List<ElementoTabla> SymbolTable, string nombre, string cadenaParam) {
            ElementoTabla result = SymbolTable.FirstOrDefault(s => s.Id == nombre);

            if (result != null && result.stParametro == cadenaParam)
            {
                return true;
            }
            //TODO: errores.Add("los parametros de la funcion " + id + " son incorrectos");
            return false;
        }

        public override void ValidaTipos(ref List<ElementoTabla> SymbolTable, ref List<string> errores)
        {
            ambito = id.value;

            if (parametros.containsChildren)
            {
                parametros.ambito = ambito;
                parametros.ValidaTipos(ref SymbolTable, ref errores);
                CadenaParam = parametros.CadenaParam;
            }

            if (ExisteFunc(SymbolTable, id.value, CadenaParam))
            {
                errores.Add("La funcion " + id.value + " ya existe.");
            }
            else {
                SymbolTable.Add(new ElementoTabla(id.value, tipo.value, ambito, CadenaParam));
            }

            CadenaParam = "";

            //if (bloqueFunc != null) {
            bloqueFunc.ambito = ambito;
            bloqueFunc.ValidaTipos(ref SymbolTable, ref errores);
            //}
        }
        
    }

    //Regla 10 y 11
    public class Parametros : NonTerminal
    {
        public Symbol tipo { get; private set; }
        public Symbol id { get; private set; }
        public ListaParam listaParams { get; private set; }

        public string CadenaParam { get; private set; }

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
            containsChildren = false;
        }

        public override string ImprimeTipo()
        {
            return " Parametros ";
        }

        public bool Existe(List<ElementoTabla> SymbolTable, string id)
        {
            ElementoTabla r = SymbolTable.FirstOrDefault(s => s.Id == id);

            if (r != null) return true;

            return false;
        }

        public override void ValidaTipos(ref List<ElementoTabla> SymbolTable, ref List<string> errores)
        {
            if (Existe(SymbolTable, id.value))
            {
                errores.Add("La variable " + id.value + " ya fue declarada.");
            }
            else if(containsChildren) {
                SymbolTable.Add(new ElementoTabla(id.value, tipo.value, ambito));
            }
            CadenaParam += tipo.value;

            if (listaParams.containsChildren) {
                listaParams.ambito = ambito;
                Parametros aux = new Parametros()
                {
                    tipo = listaParams.tipo,
                    id = listaParams.id,
                    listaParams = listaParams.listaParams,
                    ambito = listaParams.ambito
                };
                aux.ValidaTipos(ref SymbolTable, ref errores);
                CadenaParam += aux.CadenaParam;
            }
        }
    }

    //Regla 12 y 13
    public class ListaParam : NonTerminal
    {
        public Symbol tipo { get; private set; }
        public Symbol id { get; private set; }
        public ListaParam listaParams { get; private set; }

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
            containsChildren = false;
        }

        public override string ImprimeTipo()
        {
            return " ListaParam ";
        }
    }

    //Regla 14
    public class BloqFunc : NonTerminal
    {
        public DefLocales locales { get; private set; }

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
            return " BloqFunc ";
        }

        public override void ValidaTipos(ref List<ElementoTabla> SymbolTable, ref List<string> errores)
        {
            locales.ambito = ambito;
            locales.ValidaTipos(ref SymbolTable, ref errores);
        }
    }

    //Regla 15 y 16
    public class DefLocales : NonTerminal
    {
        public DefLocal def { get; private set; }
        public DefLocales defs { get; private set; }

        public DefLocales(ref Stack<StackElement> pila) : base(33)
        {
            pila.Pop();
            defs = pila.Pop() as DefLocales;
            pila.Pop();
            def = pila.Pop() as DefLocal;
        }

        public DefLocales() : base(33) { def = null; defs = null; containsChildren = false; }

        public override string ImprimeTipo()
        {
            return " DefLocales ";
        }

        public override void ValidaTipos(ref List<ElementoTabla> SymbolTable, ref List<string> errores)
        {
            def.ambito = ambito;
            def.ValidaTipos(ref SymbolTable, ref errores);
            if (defs.containsChildren) {
                defs.ambito = ambito;
                defs.ValidaTipos(ref SymbolTable, ref errores);
            }
        }
    }

    //Regla 17 y 18
    public class DefLocal : NonTerminal
    {
        public DefVar defv { get; private set; }
        public Sentencia sent { get; private set; }

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

        public NonTerminal GetChild()
        {
            if (defv != null)
            {
                return defv;
            }
            return sent;
        }

        public override void ValidaTipos(ref List<ElementoTabla> SymbolTable, ref List<string> errores)
        {
            if (defv != null)
            {
                defv.ambito = ambito;
                defv.ValidaTipos(ref SymbolTable, ref errores);
            }
            else
            {
                sent.ambito = ambito;
                sent.ValidaTipos(ref SymbolTable, ref errores);
            }
        }
    }

    //Regla 19 y 20
    public class Sentencias : NonTerminal
    {
        public Sentencia sent { get; private set; }
        public Sentencias sents { get; private set; }

        public Sentencias(ref Stack<StackElement> pila) : base(35)
        {
            pila.Pop();
            sents = pila.Pop() as Sentencias;
            pila.Pop();
            sent = pila.Pop() as Sentencia;
        }

        public Sentencias() : base(35) { sent = null; sents = null; containsChildren = false; }

        public override string ImprimeTipo()
        {
            return " Sentencias ";
        }

        public override void ValidaTipos(ref List<ElementoTabla> SymbolTable, ref List<string> errores)
        {
            sent.ambito = ambito;
            sent.ValidaTipos(ref SymbolTable, ref errores);
            if (sents.containsChildren)
            {
                sents.ambito = ambito;
                sents.ValidaTipos(ref SymbolTable, ref errores);
            }
        }
    }

    #region Tipos de Sentencia
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
        public Symbol id { get; private set; }
        public Expresion expresion { get; private set; }
        public string leftDataType { get; private set; }

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

        public override void ValidaTipos(ref List<ElementoTabla> SymbolTable, ref List<string> errores)
        {
            List<ElementoTabla> resultado = SymbolTable
                .Where(elem => elem.Id == id.value && 
                        (elem.Ambito == ambito || 
                        elem.Ambito == "global"))
                .ToList();

            expresion.ambito = ambito;
            expresion.ValidaTipos(ref SymbolTable, ref errores);

            if (resultado.Count == 0)
            {
                errores.Add("La variable " + id.value + " no existe en el contexto actual " + ambito);
            }
            else
            {
                leftDataType = resultado[0].Tipo;
                if (leftDataType != expresion.tipoDato) {
                    leftDataType = "error";
                    errores.Add("El tipo de dato de " + id.value + " en la funcion " + ambito + " es diferente al de la expresion ");
                }

            }
        }
    }

    //Regla 22
    public class If : Sentencia
    {
        public Expresion expresion { get; private set; }
        public SentenciaBloque sentenciabloque { get; private set; }
        public Otro otro { get; private set; }

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

        public override void ValidaTipos(ref List<ElementoTabla> SymbolTable, ref List<string> errores)
        {
            expresion.ambito = ambito;
            expresion.ValidaTipos(ref SymbolTable, ref errores);

            if (expresion.tipoDato != "bool") { errores.Add("Se necesita una condición para evaluar"); }

            sentenciabloque.ambito = ambito;
            sentenciabloque.ValidaTipos(ref SymbolTable, ref errores);

            if (otro.containsChildren)
            {
                otro.ambito = ambito;
                otro.ValidaTipos(ref SymbolTable, ref errores);
            }
        }
    }

    //Regla 23
    public class While : Sentencia
    {
        public Expresion expresion { get; private set; }
        public Bloque bloque { get; private set; }

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

        public override void ValidaTipos(ref List<ElementoTabla> SymbolTable, ref List<string> errores)
        {
            expresion.ambito = ambito;
            expresion.ValidaTipos(ref SymbolTable, ref errores);

            if (expresion.tipoDato != "bool") { errores.Add("Se necesita una condición para evaluar"); }

            bloque.ambito = ambito;
            bloque.ValidaTipos(ref SymbolTable, ref errores);
        }
    }

    //Regla 24
    public class Return : Sentencia
    {
        public ValorRegresa exp { get; private set; }

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
        public LlamadaFunc llamadaFunc { get; private set; }

        public SentenciaLLama(ref Stack<StackElement> pila) : base()
        {
            pila.Pop();
            pila.Pop(); //quita ;
            pila.Pop();
            llamadaFunc = pila.Pop() as LlamadaFunc; //quita llamadaFunc
        }

        public override void ValidaTipos(ref List<ElementoTabla> SymbolTable, ref List<string> errores)
        {
            llamadaFunc.ambito = ambito;
            llamadaFunc.ValidaTipos(ref SymbolTable, ref errores);
        }
    }
    #endregion

    //Regla 26 y 27
    public class Otro : NonTerminal
    {
        public SentenciaBloque sentBloq { get; private set; }

        public Otro(ref Stack<StackElement> pila) : base(37)
        {
            pila.Pop();
            sentBloq = pila.Pop() as SentenciaBloque; //quita SentenciaBloque
            pila.Pop();
            pila.Pop(); //quita else
        }

        public Otro() : base(37) { sentBloq = null; containsChildren = false; }

        public override string ImprimeTipo()
        {
            return " Otro ";
        }

        public override void ValidaTipos(ref List<ElementoTabla> SymbolTable, ref List<string> errores)
        {
            sentBloq.ambito = ambito;
            sentBloq.ValidaTipos(ref SymbolTable, ref errores);
        }
    }

    //Regla 28
    public class Bloque : NonTerminal
    {
        public Sentencias sents { get; private set; }

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

        public override void ValidaTipos(ref List<ElementoTabla> SymbolTable, ref List<string> errores)
        {
            sents.ambito = ambito;
            sents.ValidaTipos(ref SymbolTable, ref errores);
        }
    }

    //Regla 29 y 30
    public class ValorRegresa : NonTerminal
    {
        public Expresion exp { get; private set; }

        public ValorRegresa(ref Stack<StackElement> pila) : base(39)
        {
            pila.Pop();
            exp = pila.Pop() as Expresion;
        }

        public ValorRegresa() : base(39) { exp = null; containsChildren = false; }

        public override string ImprimeTipo()
        {
            return " ValorRegresa ";
        }
    }

    //Regla 31 y 32
    public class Argumentos : NonTerminal
    {
        public Expresion expr { get; set; }
        public ListaArgumentos lArgs { get; set; }

        public Argumentos(ref Stack<StackElement> pila) : base(40)
        {
            pila.Pop();
            lArgs = pila.Pop() as ListaArgumentos;
            pila.Pop();
            expr = pila.Pop() as Expresion;
        }

        public Argumentos() : base(40) { lArgs = null; expr = null; containsChildren = false; }

        public override string ImprimeTipo()
        {
            return " Argumentos ";
        }
    }

    //Regla 33 y 34
    public class ListaArgumentos : NonTerminal
    {
        public Expresion expr { get; private set; }
        public ListaArgumentos lArgs { get; private set; }

        public ListaArgumentos(ref Stack<StackElement> pila) : base(41)
        {
            pila.Pop();
            lArgs = pila.Pop() as ListaArgumentos;
            pila.Pop();
            expr = pila.Pop() as Expresion;
            pila.Pop();
            pila.Pop(); //quita coma
        }

        public ListaArgumentos() : base(41) { lArgs = null; expr = null; containsChildren = false; }

        public override string ImprimeTipo()
        {
            return " ListaArgumentos ";
        }
    }

    //Reglas 35, 36, 37, 38 y 39
    public class Termino : NonTerminal
    {
        public Symbol symb { get; private set; }
        public LlamadaFunc lfunc { get; private set; }
        public string tipoDato { get; private set; }

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

        public object getChild() {
            if (symb != null)
                return symb;
            return lfunc;
        }

        public override void ValidaTipos(ref List<ElementoTabla> SymbolTable, ref List<string> errores)
        {
            if (symb != null)
            {
                if (symb.type == SymbolType.identificador)
                {
                    List<ElementoTabla> result = SymbolTable
                        .Where(i => i.Id == symb.value && 
                                (i.Ambito == ambito || 
                                i.Ambito == "global"))
                        .ToList();
                    if (result.Count > 0) { tipoDato = result[0].Tipo; }
                    else { errores.Add("No se encuentra la variable " + symb.value + " dentro de la funcion " + ambito); }
                }
                else {
                    tipoDato = symb.name;
                }
            }
            else
            {
                lfunc.ValidaTipos(ref SymbolTable, ref errores);
                //TODO: tipoDato = lfunc.tipoDato;
            }

        }
        //TODO: terminar mañana
    }

    //Regla 40
    public class LlamadaFunc : NonTerminal
    {
        public Symbol id { get; private set; }
        public Argumentos argumentos { get; private set; }

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

        private bool Existe(List<ElementoTabla> SymbolTable, string simbolo, string cadena)
        {
            List<ElementoTabla> result = SymbolTable.Where(i => i.Id == simbolo && i.stParametro.Length == cadena.Length).ToList();

            if (result.Count > 0) { return true; } else { return false; }
        }

        public override void ValidaTipos(ref List<ElementoTabla> SymbolTable, ref List<string> errores)
        {
            Argumentos aux = new Argumentos()
            {
                ambito = ambito,
                expr = argumentos.expr,
                lArgs = argumentos.lArgs,
                containsChildren = true
            };
            StringBuilder cadena = new StringBuilder();
            while (aux.containsChildren) {

            }
        }

        //TODO: terminar esta chingadera
    }

    //Regla 41 y 42
    public class SentenciaBloque : NonTerminal
    {
        public Sentencia sent { get; private set; }
        public Bloque bloq { get; private set; }

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

        public NonTerminal GetChild() {
            if (sent != null)
                return sent;
            return bloq;
        }

        public override void ValidaTipos(ref List<ElementoTabla> SymbolTable, ref List<string> errores)
        {
            if (sent != null)
            {
                sent.ambito = ambito;
                sent.ValidaTipos(ref SymbolTable, ref errores);
            }
            else
            {
                bloq.ambito = ambito;
                bloq.ValidaTipos(ref SymbolTable, ref errores);
            }
        }
    }

    //Regla 43 a 52
    public class Expresion : NonTerminal
    {
        public Expresion expr { get; private set; }
        public Termino ter { get; private set; }
        public string tipoDato { get; protected set; }

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

        public Expresion() : base(45) { expr = null; ter = null; containsChildren = false; }

        public override string ImprimeTipo()
        {
            return " Expresion ";
        }

        public NonTerminal GetChild() {
            if (expr != null)
                return expr;
            return ter;
        }

        public override void ValidaTipos(ref List<ElementoTabla> SymbolTable, ref List<string> errores)
        {
            if (expr != null)
            {
                expr.ambito = ambito;
                expr.ValidaTipos(ref SymbolTable, ref errores);
                tipoDato = expr.tipoDato;
            }
            else
            {
                ter.ambito = ambito;
                ter.ValidaTipos(ref SymbolTable, ref errores);
                tipoDato = ter.tipoDato;
            }
        }
    }

    //Regla 44 y 45
    public class Operacion1 : Expresion
    {
        public Expresion der { get; private set; }

        public Operacion1(ref Stack<StackElement> pila) : base()
        {
            pila.Pop();
            der = pila.Pop() as Expresion; //quita expresion
            pila.Pop();
            symbol = pila.Pop().symbol; //quita el operador
            containsChildren = true;
        }

        public override void ValidaTipos(ref List<ElementoTabla> SymbolTable, ref List<string> errores)
        {
            der.ambito = ambito;
            der.ValidaTipos(ref SymbolTable, ref errores);

            tipoDato = der.tipoDato;
        }
    }

    //Reglas 46 a 51
    public class Operacion2 : Expresion
    {
        public Expresion der { get; private set; }
        public Expresion izq { get; private set; }

        public Operacion2(ref Stack<StackElement> pila) : base()
        {
            pila.Pop();
            der = pila.Pop() as Expresion;//quita exprsion
            pila.Pop();
            symbol = pila.Pop().symbol;//quita el operador
            pila.Pop();
            izq = pila.Pop() as Expresion;//quita expresion
            containsChildren = true;
        }

        public override void ValidaTipos(ref List<ElementoTabla> SymbolTable, ref List<string> errores)
        {
            der.ambito = ambito;
            izq.ambito = ambito;

            der.ValidaTipos(ref SymbolTable, ref errores);
            izq.ValidaTipos(ref SymbolTable, ref errores);

            if (der.tipoDato == izq.tipoDato)
            {
                if (symbol.type == SymbolType.opRelac ||
                    symbol.type == SymbolType.opOr ||
                    symbol.type == SymbolType.opAnd ||
                    symbol.type == SymbolType.opIgualdad)
                {
                    tipoDato = "bool";
                }
                else
                {
                    tipoDato = der.tipoDato;
                }
            }
            else {
                tipoDato = "error";
                errores.Add("No se pueden realizar operaciones con tipos de datos distintos");
            }
        }
    }
}
