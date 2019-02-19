using System;
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
}
