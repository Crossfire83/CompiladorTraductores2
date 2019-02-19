using System;
using System.Text;

namespace CompiladorTraductores2
{
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
            if (IsFinished()) return '\0';
            return font[ind++];
        }

        private void NextState(int state) { this.state = state; }

        private void Acceptance(int state) {
            this.state = state;
            cont = false;
        }

        private bool IsLetter(char c) { return char.IsLetter(c); }

        private bool IsDigit(char c) { return char.IsDigit(c); }

        private bool IsSpace(char c) { return char.IsWhiteSpace(c) || c == '\r' || c == '\n'; }

        private void LeapBack() {
            if (c != '$') ind--;
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
                        if (IsDigit(c)) //TODO: revisar si float o int pueden empezar en 0
                        {
                            temp.Append(c);
                            result.name = "Integer";
                            result.type = SymbolType.Integer;
                            state = 1;
                        }
                        else if (c == '+' || c == '-')
                        {
                            temp.Append(c);
                            result.name = "Add Operator";
                            result.type = SymbolType.AddOp;
                            cont = false;
                        }
                        else if (c == '*' || c == '/')
                        {
                            temp.Append(c);
                            result.name = "Multiplication Operator";
                            result.type = SymbolType.MultOp;
                            cont = false;
                        }
                        else if (c == '<' || c == '>')
                        {
                            temp.Append(c);
                            result.name = "Relational Operator";
                            result.type = SymbolType.RelatOp;
                            state = 3;
                        }
                        else if (c == '!' || c == '=')
                        {
                            temp.Append(c);
                            state = 4;
                        }
                        else if (c == '&')
                        {
                            temp.Append(c);
                            c = NextChar();
                            if (c == '&')
                            {
                                temp.Append(c);
                                result.name = "And Operator";
                                result.type = SymbolType.AndOp;
                            }
                            else
                            {
                                LeapBack();
                                result.name = "Error";
                                result.type = SymbolType.Error;
                            }
                            cont = false;
                        }
                        else if (c == '|')
                        {
                            temp.Append(c);
                            c = NextChar();
                            if (c == '|')
                            {
                                temp.Append(c);
                                result.name = "Or Operator";
                                result.type = SymbolType.OrOp;
                            }
                            else
                            {
                                LeapBack();
                                result.name = "Error";
                                result.type = SymbolType.Error;
                            }
                            cont = false;
                        }
                        else if (c == '(')
                        {
                            temp.Append(c);
                            result.name = "Opening Parenthesis";
                            result.type = SymbolType.OpenParenthesis;
                            cont = false;
                        }
                        else if (c == ')')
                        {
                            temp.Append(c);
                            result.name = "Closing Parenthesis";
                            result.type = SymbolType.CloseParenthesis;
                            cont = false;
                        }
                        else if (c == '{')
                        {
                            temp.Append(c);
                            result.name = "Opening Bracket";
                            result.type = SymbolType.OpenBracket;
                            cont = false;
                        }
                        else if (c == '}')
                        {
                            temp.Append(c);
                            result.name = "Closing Bracket";
                            result.type = SymbolType.CloseBracket;
                            cont = false;
                        }
                        else if (c == ';')
                        {
                            temp.Append(c);
                            result.name = "SemiColon";
                            result.type = SymbolType.SemiColon;
                            cont = false;
                        }
                        else if (IsLetter(c))
                        {
                            temp.Append(c);
                            result.name = "Identifier";
                            result.type = SymbolType.Identifier;
                            state = 5;
                        }
                        else if (c == '$')
                        {
                            temp.Append(c);
                            result.name = "Currency";
                            result.type = SymbolType.Currency;
                            cont = false;
                        }
                        else if (IsSpace(c))
                        {
                            state = 0;
                            cont = true;
                        }
                        else if (c == '"')
                        {
                            temp.Append(c);
                            result.name = "String";
                            result.type = SymbolType.String;
                            state = 7;
                        }
                        else {
                            temp.Append(c);
                            result.name = "Error";
                            result.type = SymbolType.Error;
                            cont = false;
                        } // error
                        break;
                    case 1:
                        if (IsDigit(c))
                        {
                            temp.Append(c);

                            result.name = "int";
                            result.type = SymbolType.Integer;
                        }
                        else if (c == '.')
                        {
                            state = 2;
                            temp.Append(c);
                        }
                        else {
                            cont = false;
                            LeapBack();
                        }
                        break;
                    case 2:
                        if (IsDigit(c))
                        {
                            temp.Append(c);
                            result.name = "float";
                            result.type = SymbolType.Real;
                            state = 6;
                        }
                        else {
                            result.name = "Error";
                            result.type = SymbolType.Error;
                            cont = false;
                            LeapBack();
                        }
                        break;
                    case 3:
                        if (c == '=')
                        {
                            temp.Append(c);
                            result.name = "Relational Operator";
                            result.type = SymbolType.RelatOp;
                        }
                        else
                        {
                            LeapBack();
                            cont = false;
                        }
                        break;
                    case 4:
                        if (c == '=')
                        {
                            temp.Append(c);
                            cont = false;
                            result.name = "Equality Operator";
                            result.type = SymbolType.EqualOp;
                        }
                        else
                        {
                            if (temp.ToString() == "=")
                            {
                                result.name = "Assignation Operator";
                                result.type = SymbolType.Assignation;
                            }
                            else if (temp.ToString() == "!")
                            {
                                result.name = "Not Operator";
                                result.type = SymbolType.NotOp;
                            }
                            cont = false;
                            LeapBack();
                        }
                        break;
                    case 5:
                        if (IsLetter(c))
                        {
                            temp.Append(c);
                            if (temp.ToString().ToLower() == "if")
                            {
                                result.name = "If";
                                result.type = SymbolType.If;
                                cont = false;
                            }
                            else if (temp.ToString().ToLower() == "else")
                            {
                                result.name = "Else";
                                result.type = SymbolType.Else;
                                cont = false;
                            }
                            else if (temp.ToString().ToLower() == "while")
                            {
                                result.name = "While";
                                result.type = SymbolType.While;
                                cont = false;
                            }
                            else if (temp.ToString().ToLower() == "return")
                            {
                                result.name = "Return";
                                result.type = SymbolType.Return;
                                cont = false;
                            }
                            else if (temp.ToString().ToLower() == "int" || 
                                temp.ToString().ToLower() == "float" ||
                                temp.ToString().ToLower() == "string")
                            {
                                result.name = "Type";
                                result.type = SymbolType.Type;
                                cont = false;
                            }
                            else
                            {
                                result.name = "Identifier";
                                result.type = SymbolType.Identifier;
                            }
                        }
                        else if (IsDigit(c))
                        {
                            temp.Append(c);
                            result.name = "Identifier";
                            result.type = SymbolType.Identifier;
                        }
                        else {
                            LeapBack();
                            cont = false;
                        }
                        break;
                    case 6:
                        if (IsDigit(c))
                        {
                            temp.Append(c); //tipo ya fue asignado en estado 2
                        }
                        else {
                            cont = false;
                        }
                        break;
                    case 7:
                        if (c == '"')
                        {
                            if (temp.ToString()[temp.Length - 1] == '\\')
                            {
                                temp.Append(c);
                            }
                            else
                            {
                                temp.Append(c);
                                cont = false;
                            }
                        }
                        else if (c != '\r' && c != '\n' && c != '\0')
                        {
                            temp.Append(c);
                        }
                        else {
                            temp.Append(c);
                            result.name = "Error";
                            result.type = SymbolType.Error;
                            cont = false;
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