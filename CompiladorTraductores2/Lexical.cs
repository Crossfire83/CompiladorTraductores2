using System;

namespace CompiladorTraductores2
{
    public enum SymbolType {
        Error = -1,
        Identifier = 0,
        AddOp = 1,
        MultOp = 2,
        Currency = 3,
        Int = 4
    }

    class Lexical
    {
        private string font;
        private int ind;
        private bool cont;
        private char c;
        private int state;
        public string symbol;
        public int type;

        private char NextChar() { throw new NotImplementedException(); }

        private void NextState() { throw new NotImplementedException(); }

        private void Acceptance() { throw new NotImplementedException(); }

        private bool IsLetter(char c) { throw new NotImplementedException(); }

        private bool IsDigit(char c) { throw new NotImplementedException(); }

        private bool IsSpace(char c) { throw new NotImplementedException(); }

        //TODO: cambiar el nombre a ingles
        private void retroceso() { throw new NotImplementedException(); }

        public Lexical() {
            ind = 0;
        }

        public Lexical(string font) : this() {
            this.font = font;
        }

        public void Input(string font) {
            ind = 0;
            this.font = font;
        }

        public string TypeToString(int type) { throw new NotImplementedException(); }

        public int NextSymbol() {
            state = 0;
            cont = true;
            symbol = "";


            while (cont) {
                c = NextChar();
                switch (state) {

                }
            }

            return 0;
        }

        public bool IsFinished() { throw new NotImplementedException(); }
    }
}