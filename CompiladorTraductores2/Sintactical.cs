using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Sintactical(string text, ref DataGridView SymbolsTable) {
            l = new Lexical(text);
            this.SymbolsTable = SymbolsTable;
            this.SymbolsTable.Rows.Clear();
            SyntacticalStack = new Stack();
        }

        internal void SetLRTable(string[] lines)
        {
            List<List<int>> table = new List<List<int>>();

            for (int index = 0; index < lines.Length; index++)
            {
                lines[index] = lines[index].Trim();
                string[] cols = lines[index].Split('\t');
                int[] intcols = cols.Select(s => Int32.Parse(s)).ToArray();
                for (int index1 = 0; index1 < intcols.Length; index1++)
                {
                    table[index][index1] = intcols[index1];
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
                string l = line.Trim();

                string[] cols = l.Split('\t');

                Rule rule = new Rule() { Id = Int32.Parse(cols[0]), TotalProductions = Int32.Parse(cols[1]), RuleName = cols[2] };
                data[index] = rule;
                index++;
            }
            Rules = data;
        }

        public void Analiza() {
            int x = 0;  //Fila
            int y = 0;  //Columna
            int r = 0;  //Resultado (regla, desplazamiento o aceptacion)

            int pops = 0;   //Cantidad de elementos que produce la regla
            bool error = false; //Bandera que detiene el ciclo
            bool newSymbol = true;  //Decide si se necesita un nuevo simbolo del Lexico o no
            SyntacticalStack.Push(new State(0));
            StackElement Root = new StackElement();

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
