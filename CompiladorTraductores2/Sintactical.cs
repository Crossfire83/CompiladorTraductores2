using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Collections;

namespace CompiladorTraductores2
{
    class Sintactical
    {
        public Lexical l;
        public DataGridView SymbolsTable;
        public Stack SyntacticalStack;
        public List<List<int>> LRTable;
        internal Rule[] Rules;

        public Sintactical(ref DataGridView SymbolsTable) {
            this.SymbolsTable = SymbolsTable;
            this.SymbolsTable.Rows.Clear();
            SyntacticalStack = new Stack();
        }

        internal void SetLRTable(string[] lines)
        {
            List<List<int>> table = new List<List<int>>();

            for (int index = 0; index < lines.Length; index++)
            {
                table.Add(new List<int>());
                lines[index] = lines[index].Trim();
                string[] cols = lines[index].Split('\t');
                int[] intcols = cols.Select(s => Int32.Parse(s)).ToArray();
                for (int index1 = 0; index1 < intcols.Length; index1++)
                {
                    table[index].Add(intcols[index1]);
                }
            }
            this.LRTable = table;
        }

        public void SetRules(string text) {
            Rule[] data = new Rule[51];
            string[] Lines = text.Split('\n');
            int index = 0;
            foreach (string line in Lines)
            {
                string temp = line.Trim();

                string[] cols = temp.Split('\t');

                Rule rule = new Rule() { Id = Int32.Parse(cols[0]), TotalProductions = Int32.Parse(cols[1]), RuleName = cols[2] };
                data[index] = rule;
                index++;
            }
            Rules = data;
        }

        public void Analiza(string text) {
            l = new Lexical(text);

            //while (!s.l.IsFinished())
            //{
            //    Symbol sym = s.l.NextSymbol();
            //    DataGridViewRow row = new DataGridViewRow();

            //    row.CreateCells(SymbolsTable);
            //    row.Cells[0].Value = sym.value;
            //    row.Cells[1].Value = sym.name;
            //    row.Cells[2].Value = ((int)sym.type).ToString();
            //    if (((int)sym.type) == -1)
            //    {
            //        foreach (DataGridViewCell cell in row.Cells)
            //        {
            //            cell.Style.BackColor = System.Drawing.Color.Red;
            //            cell.Style.ForeColor = System.Drawing.Color.White;
            //        }
            //    }
            //    SymbolsTable.Rows.Add(row);
            //}

            int x = 0;  //Fila
            int y = 0;  //Columna
            int r = 0;  //Resultado (regla, desplazamiento o aceptacion)

            int pops = 0;   //Cantidad de elementos que produce la regla
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
                x = ((State)SyntacticalStack.Peek()).transicion;
                y = (int)currentSymbol.type;

                r = LRTable[x][y];

                if (r > 0)
                {
                    //Desplazamiento
                    SyntacticalStack.Push(new Terminal(currentSymbol));
                    SyntacticalStack.Push(new State(r));
                    newSymbol = true;
                }
                else if (r < 0) //Regla
                {
                    r = Math.Abs(r) - 1;
                    if (r == 0)
                    {
                        //Cadena Aceptada
                        break;
                    }
                    StackElement element = null;
                    switch (r)
                    {
                        case 3: //<Definiciones> ::= <Definicion> <Definiciones>
                        case 16: //<DefLocales> ::= <DefLocal> <DefLocales> 	
                        case 20: //<Sentencias> ::= <Sentencia> <Sentencias>
                        case 32: //<Argumentos> ::= <Expresion> <ListaArgumentos> 
                            SyntacticalStack.Pop(); //quita estado
                            StackElement aux = (StackElement)SyntacticalStack.Pop(); //quita <definiciones>
                            SyntacticalStack.Pop(); //quita estado
                            element = (StackElement)SyntacticalStack.Pop(); //quita <definicion>
                            if (element != null)
                                element.Next = aux;
                            break;
                        case 1: //<programa> ::= <Definiciones> 
                        case 4: //<Definicion> ::= <DefVar>
                        case 5: //<Definicion> ::= <DefFunc> 
                        case 17: //<DefLocal> ::= <DefVar> 
                        case 18: //<DefLocal> ::= <Sentencia> 
                        case 35: //<Atomo> ::= <LlamadaFunc> 
                        case 39: //<SentenciaBloque> ::= <Sentencia> 
                        case 40: //<SentenciaBloque> ::= <Bloque> 
                        case 50: //<Expresion> ::= <Atomo>
                            SyntacticalStack.Pop();//quita estado
                            element = (StackElement)SyntacticalStack.Pop(); //quita defvar
                            break;

                        case 6: // <DefVar> ::= tipo id <ListaVar>
                            element = new DefVar(ref SyntacticalStack);
                            break;

                        case 8:  //<ListaVar> ::= , id <ListaVar>
                            SyntacticalStack.Pop(); //quita estado
                            StackElement lvar = ((StackElement)SyntacticalStack.Pop());
                            SyntacticalStack.Pop(); //quita estado
                            element = new Identificador(((Terminal)SyntacticalStack.Pop()).element.symbol)
                            {
                                Next = lvar
                            }; //quita id
                            SyntacticalStack.Pop(); //quita estado
                            SyntacticalStack.Pop(); //quita la coma
                            break;

                        case 9: //<DefFunc> ::= tipo id ( <Parametros> ) <BloqFunc>
                            element = new DefFunc(ref SyntacticalStack);
                            break;

                        case 11: //<Parametros> ::= tipo id <ListaParam>
                            element = new Parametros(ref SyntacticalStack);
                            break;

                        case 13: //<ListaParam> ::= , tipo id <ListaParam>
                            element = new Parametros(ref SyntacticalStack);
                            SyntacticalStack.Pop(); //quita estado
                            SyntacticalStack.Pop(); //quita la coma
                            break;

                        case 14: //<BloqFunc> ::= { <DefLocales> }
                        case 30: //<Bloque> ::= { <Sentencias> } 
                        case 41: //<Expresion> ::= ( <Expresion> ) 
                            SyntacticalStack.Pop(); //quita estado
                            SyntacticalStack.Pop(); //quita }
                            SyntacticalStack.Pop(); //quita estado
                            element = ((StackElement)SyntacticalStack.Pop()); //quita <deflocales> o <sentencias>
                            SyntacticalStack.Pop();
                            SyntacticalStack.Pop(); //quita la {
                            break;

                        case 21: //<Sentencia> ::= id = <Expresion>
                            element = new Asignacion(ref SyntacticalStack);
                            break;

                        case 22: //<Sentencia> ::= if ( <Expresion> ) <SentenciaBloque> <Otro>
                            element = new If(ref SyntacticalStack);
                            break;

                        case 23: //<Sentencia> ::= while ( <Expresion> ) <Bloque> 
                            element = new While(ref SyntacticalStack);
                            break;

                        case 24: //<Sentencia> ::= do <Bloque> while ( <Expresion> )
                            element = new DoWhile(ref SyntacticalStack);
                            break;

                        case 25: //<Sentencia> ::= for id = <Expresion> : <Expresion> : <Expresion> <SentenciaBloque>
                            element = new For(ref SyntacticalStack);
                            break;

                        case 26: //<Sentencia> ::= return <Expresion>
                            element = new Return(ref SyntacticalStack);
                            break;

                        case 27: //<Sentencia> ::= <LlamadaFunc>
                            SyntacticalStack.Pop();
                            SyntacticalStack.Pop(); //quita ;
                            SyntacticalStack.Pop();
                            element = ((StackElement)SyntacticalStack.Pop()); //quita llamadafunc
                            break;

                        case 29: //<Otro> ::= else <SentenciaBloque> 
                            SyntacticalStack.Pop();
                            element = ((StackElement)SyntacticalStack.Pop()); //quita sentencia bloque
                            SyntacticalStack.Pop();
                            SyntacticalStack.Pop(); //quita el else
                            break;

                        case 34: // <ListaArgumentos> ::= , <Expresion> <ListaArgumentos> 
                            SyntacticalStack.Pop();
                            aux = ((StackElement)SyntacticalStack.Pop()); //quita la lsta de argumentos
                            SyntacticalStack.Pop();
                            element = ((StackElement)SyntacticalStack.Pop()); //quita expresion
                            SyntacticalStack.Pop();
                            SyntacticalStack.Pop(); //quita la ,
                            element.Next = aux;
                            break;

                        case 36:
                            SyntacticalStack.Pop();
                            element = new Identificador(((Terminal)SyntacticalStack.Pop()).element.symbol);
                            break;

                        case 37:
                            SyntacticalStack.Pop();
                            element = new Constante(((Terminal)SyntacticalStack.Pop()).element.symbol);
                            break;
                        case 38:
                            element = new LlamadaFunc(ref SyntacticalStack);
                            break;

                        case 42: //<Expresion> ::= opSuma <Expresion>
                        case 43: //<Expresion> ::= opNot <Expresion>
                            element = new Operacion1(ref SyntacticalStack);
                            break;

                        case 44: //<Expresion> ::= <Expresion> opMul <Expresion>
                        case 45: //<Expresion> ::= <Expresion> opSuma <Expresion>
                        case 46: //<Expresion> ::= <Expresion> opRelac <Expresion>
                        case 47: //<Expresion> ::= <Expresion> opIgualdad <Expresion>
                        case 48: //<Expresion> ::= <Expresion> opAnd <Expresion>
                        case 49: //<Expresion> ::= <Expresion> opOr <Expresion>
                            element = new Operacion2(ref SyntacticalStack);
                            break;

                        //aqui cae R2,R7,R10,R12,R15,R19,R28,R31,R33,
                        default:
                            pops = Rules.ElementAt(r).TotalProductions;

                            if (pops > 0)
                            {
                                while (pops > 0)
                                {
                                    SyntacticalStack.Pop();
                                    SyntacticalStack.Pop();
                                    pops--;
                                }
                            }
                            break;
                    }
                    x = ((State)SyntacticalStack.Peek()).transicion;

                    y = Rules.ElementAt(r).Id;
                    //NonTerminal nt = new NonTerminal(y)
                    SyntacticalStack.Push(element);

                    r = LRTable[x][y];
                    SyntacticalStack.Push(new State(r));
                    newSymbol = false;
                    //Root = element
                }
                else
                    error = true;
                
            }
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
