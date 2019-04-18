using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;

namespace CompiladorTraductores2
{
    class Sintactical
    {
        public Lexical l;
        public DataGridView SymbolsTable;
        public Stack<StackElement> SyntacticalStack;
        public List<List<int>> LRTable;
        private List<Rule> Rules;
        public StringBuilder stack;
        public StackElement Root;

        public Sintactical(ref DataGridView SymbolsTable) {
            this.SymbolsTable = SymbolsTable;
        }

        internal void SetLRTable(string[] lines)
        {
            int numberOfRules;
            if (Int32.TryParse(lines[0], out numberOfRules))
            {
                Rules = new List<Rule>(numberOfRules);
                for (int i = 1; i <= numberOfRules; i++)
                {
                    string temp = lines[i].Trim();
                    string[] cols = temp.Split('\t');
                    Rule rule = new Rule() { Id = Int32.Parse(cols[0]), TotalProductions = Int32.Parse(cols[1]), RuleName = cols[2] };
                    Rules.Add(rule);
                }

                string dimensions = lines[numberOfRules + 1];
                string[] lineTemp = dimensions.Split('\t');
                int x, y;
                if (Int32.TryParse(lineTemp[0], out y)) {
                    if (Int32.TryParse(lineTemp[1], out x))
                    {
                        //Creacion de tabla LR
                        LRTable = new List<List<int>>();
                        for (int i = 0; i < y; i++) {
                            LRTable.Add(new List<int>());
                        }

                        //Agregar valores a la tabla LR
                        int lineZero = numberOfRules + 2;
                        for (int i = lineZero; i < lines.Length; i++) {
                            lines[i] = lines[i].Trim();
                            string[] cols = lines[i].Split('\t');
                            int[] intcols = cols.Select(s => Int32.Parse(s)).ToArray();
                            foreach (int value in intcols) {
                                LRTable[i - lineZero].Add(value);
                            }
                        }
                    }
                    else
                    {
                        throw new FormatException("El archivo no está en el formato aceptado.");
                    }
                }
                else
                {
                    throw new FormatException("El archivo no está en el formato aceptado.");
                }
            }
            else
            {
                throw new FormatException("El archivo no está en el formato aceptado.");
            }
        }

        public string Analiza(string text) {
            l = new Lexical(text);

            stack = new StringBuilder();
            SyntacticalStack = new Stack<StackElement>();
            int x = 0;  //Fila
            int y = 0;  //Columna
            int r = 0;  //Resultado (regla, desplazamiento o aceptacion)

            //int pops = 0;   //Cantidad de elementos que produce la regla
            bool error = false; //Bandera que detiene el ciclo
            bool newSymbol = true;  //Decide si se necesita un nuevo simbolo del Lexico o no
            SyntacticalStack.Push(new State(0));
            //StackElement Root = new StackElement()

            Symbol currentSymbol = new Symbol();

            while (!error) {
                if (newSymbol)
                {
                    currentSymbol = l.NextSymbol();
                    DataGridViewRow row = new DataGridViewRow();

                    row.CreateCells(SymbolsTable);
                    row.Cells[0].Value = currentSymbol.value;
                    row.Cells[1].Value = currentSymbol.name;
                    row.Cells[2].Value = ((int)currentSymbol.type).ToString();
                    if (((int)currentSymbol.type) == -1)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.BackColor = System.Drawing.Color.Red;
                            cell.Style.ForeColor = System.Drawing.Color.White;
                        }
                        error = true;
                    }
                    SymbolsTable.Rows.Add(row);
                }
                y = ((State)SyntacticalStack.Peek()).transicion;
                x = (int)currentSymbol.type;

                r = LRTable[y][x];

                if (r > 0)
                {
                    //Desplazamiento
                    SyntacticalStack.Push(new Terminal(currentSymbol));
                    SyntacticalStack.Push(new State(r));
                    newSymbol = true;
                    GeneratePrintedStack();
                }
                else if (r < 0) //Reglas
                {
                    r = Math.Abs(r) - 1;
                    if (r == 0)
                    {
                        //Cadena Aceptada
                        break;
                    }
                    NonTerminal element = null;
                    switch (r)
                    {
                        case 1: //<programa> ::= <Definiciones> 
                            element = new Programa(ref SyntacticalStack);
                            break;

                        case 2: //<Definiciones> ::= \e 
                            element = new Definiciones();
                            break;

                        case 3: //<Definiciones> ::= <Definicion> <Definiciones>
                            element = new Definiciones(ref SyntacticalStack);
                            break;

                        case 4: //<Definicion> ::= <DefVar>
                        case 5: //<Definicion> ::= <DefFunc> 
                            element = new Definicion(ref SyntacticalStack);
                            break;

                        case 6: //<DefVar> ::= tipo identificador <ListaVar> ;
                            element = new DefVar(ref SyntacticalStack);
                            break;

                        case 7: //<ListaVar> ::= \e
                            element = new ListaVar();
                            break;

                        case 8:  //<ListaVar> ::= , id <ListaVar>
                            element = new ListaVar(ref SyntacticalStack);
                            break;

                        case 9: //<DefFunc> ::= tipo id ( <Parametros> ) <BloqFunc>
                            element = new DefFunc(ref SyntacticalStack);
                            break;

                        case 10: //<Parametros> ::= \e
                            element = new Parametros();
                            break;

                        case 11: //<Parametros> ::= tipo id <ListaParam>
                            element = new Parametros(ref SyntacticalStack);
                            break;

                        case 12: //R12 <ListaParam> ::= \e
                            element = new ListaParam();
                            break;

                        case 13: //<ListaParam> ::= , tipo id <ListaParam>
                            element = new Parametros(ref SyntacticalStack);
                            break;

                        case 14: //<BloqFunc> ::= { <DefLocales> }
                            element = new BloqFunc(ref SyntacticalStack);
                            break;

                        case 15: //<DefLocales> ::= \e
                            element = new DefLocales();
                            break;

                        case 16: //<DefLocales> ::= <DefLocal> <DefLocales> 	
                            element = new DefLocales(ref SyntacticalStack);
                            break;

                        case 17: //<DefLocal> ::= <DefVar> 
                            element = new DefLocal(ref SyntacticalStack);
                            break;

                        case 18: //<DefLocal> ::= <Sentencia> 
                            element = new DefLocal(ref SyntacticalStack);
                            break;

                        case 19: //<Sentencias> ::= \e
                            element = new Sentencias();
                            break;

                        case 20: //<Sentencias> ::= <Sentencia> <Sentencias>
                            element = new Sentencias(ref SyntacticalStack);
                            break;

                        case 21: //<Sentencia> ::= identificador = <Expresion> ;
                            element = new Asignacion(ref SyntacticalStack);
                            break;

                        case 22: //<Sentencia> ::= if ( <Expresion> ) <SentenciaBloque> <Otro>
                            element = new If(ref SyntacticalStack);
                            break;

                        case 23: //<Sentencia> ::= while ( <Expresion> ) <Bloque> 
                            element = new While(ref SyntacticalStack);
                            break;

                        case 24: //<Sentencia> ::= return <ValorRegresa> ;
                            element = new Return(ref SyntacticalStack);
                            break;

                        case 25: //<Sentencia> ::= <LlamadaFunc> ;
                            element = new SentenciaLLama(ref SyntacticalStack);
                            break;

                        case 26: //<Otro> ::= \e
                            element = new Otro();
                            break;

                        case 27: //<Otro> ::= else <SentenciaBloque>
                            element = new Otro(ref SyntacticalStack);
                            break;

                        case 28: //<Bloque> ::= { <Sentencias> }
                            element = new Bloque(ref SyntacticalStack);
                            break;

                        case 29: //<ValorRegresa> ::= \e
                            element = new ValorRegresa();
                            break;

                        case 30: //<ValorRegresa> ::= <Expresion>
                            element = new ValorRegresa(ref SyntacticalStack);
                            break;

                        case 31: //<Argumentos> ::= \e
                            element = new Argumentos();
                            break;

                        case 32: //<Argumentos> ::= <Expresion> <ListaArgumentos> 
                            element = new Argumentos(ref SyntacticalStack);
                            break;

                        case 33: //<ListaArgumentos> ::= \e
                            element = new ListaArgumentos();
                            break;

                        case 34: //<ListaArgumentos> ::= , <Expresion> <ListaArgumentos> 
                            element = new ListaArgumentos(ref SyntacticalStack);
                            break;

                        case 35: //<Termino> ::= <LlamadaFunc>
                        case 36: //<Termino> ::= identificador
                        case 37: //<Termino> ::= entero
                        case 38: //<Termino> ::= real
                        case 39: //<Termino> ::= cadena
                            element = new Termino(ref SyntacticalStack);
                            break;

                        case 40: //<LlamadaFunc> ::= identificador ( <Argumentos> )
                            element = new LlamadaFunc(ref SyntacticalStack);
                            break;

                        case 41: //<SentenciaBloque> ::= <Sentencia> 
                        case 42: //<SentenciaBloque> ::= <Bloque>
                            element = new SentenciaBloque(ref SyntacticalStack);
                            break;

                        case 43: //<Expresion> ::= ( <Expresion> )
                            element = new Expresion(ref SyntacticalStack);
                            break;

                        case 44: //<Expresion> ::= opSuma <Expresion>
                        case 45: //<Expresion> ::= opNot <Expresion>
                            element = new Operacion1(ref SyntacticalStack);
                            break;

                        case 46: //<Expresion> ::= <Expresion> opMul <Expresion>
                        case 47: //<Expresion> ::= <Expresion> opSuma <Expresion>
                        case 48: //<Expresion> ::= <Expresion> opRelac <Expresion>
                        case 49: //<Expresion> ::= <Expresion> opIgualdad <Expresion>
                        case 50: //<Expresion> ::= <Expresion> opAnd <Expresion>
                        case 51: //<Expresion> ::= <Expresion> opOr <Expresion>
                            element = new Operacion2(ref SyntacticalStack);
                            break;

                        case 52: //<Expresion> ::= <Termino>
                            element = new Expresion(ref SyntacticalStack);
                            break;

                        default:
                            //pops = Rules.ElementAt(r - 1).TotalProductions;

                            //if (pops > 0)
                            //{
                            //    while (pops > 0)
                            //    {
                            //        SyntacticalStack.Pop();
                            //        SyntacticalStack.Pop();
                            //        pops--;
                            //    }
                            //}
                            
                            break;
                    }
                    y = ((State)SyntacticalStack.Peek()).transicion;

                    x = element.columna;
                    SyntacticalStack.Push(element);

                    r = LRTable[y][x];
                    SyntacticalStack.Push(new State(r));
                    newSymbol = false;
                    GeneratePrintedStack();
                    Root = element;
                }
                else
                    error = true;
                
            }

            StringBuilder result = new StringBuilder();
            if (error) {
                result.Append("Error en símbolo: " + currentSymbol.value + "\r\n");
            }
            return result.ToString();
        }

        private void GeneratePrintedStack() {
            Stack<StackElement> temp = new Stack<StackElement>();
            foreach (StackElement em in SyntacticalStack)
            {
                temp.Push(em);
            }
            foreach (StackElement em in temp)
            {
                stack.Append(em.ImprimeTipo());
            }
            stack.Append(Environment.NewLine);
        }
    }

    class Rule
    {
        public int Id;
        public int TotalProductions;
        public string RuleName;

        public Rule()
        {
            RuleName = string.Empty;
        }
    }
}
