using System;
using System.Text;

namespace CompiladorTraductores2
{
    public enum SymbolType {
        Error = -1,
        Identifier = 0,
        Integer = 1,
        Real = 2,
        String = 3,
        Type = 4,
        AddOp = 5,
        MultOp = 6,
        RelatOp = 7,
        OrOp = 8,
        AndOp = 9,
        NotOp = 10,
        EqualOp = 11,
        SemiColon = 12,
        Comma = 13,
        OpenParenthesis = 14,
        CloseParenthesis = 15,
        OpenBracket = 16,
        CloseBracket = 17,
        Assignation = 18,
        If = 19,
        While = 20,
        Return = 21,
        Else = 22,
        Currency = 23
    }

    class Lexical
    {
        private string font;
        private int ind;
        private bool cont;
        private char c;
        private int state;
        public Symbol result;
        public int type;

        public Lexical()
        {
            ind = 0;
        }

        public Lexical(string font) : this()
        {
            this.font = font;
        }

        private char NextChar() {
            if (IsFinished()) return '$';
            return font[ind++];
        }

        private void NextState(int state) { this.state = state; }

        private void Acceptance(int state) {
            this.state = state;
            cont = false;
        }

        private bool IsLetter(char c) { return char.IsLetter(c); }

        private bool IsDigit(char c) { return char.IsDigit(c); }

        private bool IsSpace(char c) { return char.IsWhiteSpace(c); }

        //TODO: cambiar el nombre a ingles
        private void retroceso() {
            if (c != '$') ind--;
            cont = false;
        }
        
        public void Input(string font) {
            ind = 0;
            this.font = font;
        }

        public string TypeToString(int type) { throw new NotImplementedException(); }

        public Symbol NextSymbol() {
            state = 0;
            cont = true;
            result = new Symbol();
            StringBuilder temp = new StringBuilder();

            while (cont) {
                c = NextChar();
                switch (state) {
                    case 0:
                        if (c >= '1' && c <= '9') //TODO: revisar si float o int pueden empezar en 0
                        {
                            temp.Append(c);
                            state = 1;
                        }
                        else if (c == '+' || c == '-')
                        {
                            temp.Append(c);
                            Acceptance(4);
                            result.name = "Add Operator";
                            result.type = SymbolType.AddOp;
                        }
                        else if (c == '*' || c == '/')
                        {
                            temp.Append(c);
                            Acceptance(4);
                            result.name = "Multiplication Operator";
                            result.type = SymbolType.MultOp;
                        }
                        else if (c == '<' || c == '>')
                        {
                            temp.Append(c);
                            Acceptance(5);
                            result.name = "Relational Operator";
                            result.type = SymbolType.RelatOp;
                        }
                        else if (c == '!' || c == '=') {
                            temp.Append(c);
                            Acceptance(6);
                        }

                        else throw new NotImplementedException(); // otro tipo o error
                        break;
                    case 1:
                        if (c >= '0' && c <= '9')
                        {
                            temp.Append(c);
                            Acceptance(1);
                            result.name = "int";
                            result.type = SymbolType.Integer;
                        }
                        else if (c == '.') {
                            state = 2;
                            temp.Append(c);
                        }
                        else
                            throw new NotImplementedException(); // otro simbolo o error

                        break;
                    case 2:
                        if (c >= '0' && c <= '9')
                        {
                            temp.Append(c);
                            Acceptance(3);
                            result.name = "float";
                            result.type = SymbolType.Real;
                        }
                        else {
                            throw new NotImplementedException(); //error
                        }
                        break;
                    case 3:
                        if (c >= '0' && c <= '9')
                        {
                            temp.Append(c);
                            state = 3;
                        }
                        else
                            throw new NotImplementedException(); // otro simbolo o error
                        break;
                    case 4:

                        break;
                    case 5:
                        if (c == '=')
                        {
                            temp.Append(c);
                            Acceptance(4);
                            result.name = "Relational Operator";
                            result.type = SymbolType.RelatOp;
                        }
                        else
                            throw new NotImplementedException(); // otro simbolo o error
                        break;
                    case 6:
                        if (c == '=')
                        {
                            temp.Append(c);
                            Acceptance(4);
                            result.name = "Equality Operator";
                            result.type = SymbolType.EqualOp;
                        }
                        else if (temp.ToString() == "=")
                        {
                            Acceptance(4);
                            result.name = "Assignation Operator";
                            result.type = SymbolType.Assignation;
                        }
                        else if (temp.ToString() == "!") {
                            Acceptance(4);
                            result.name = "Not Operator";
                            result.type = SymbolType.NotOp;
                        }
                            
                        break;

                }
            }
            result.value = temp.ToString();
            return result;
        }

        public bool IsFinished() {
            return ind >= font.Length;
        }
    }
}