using System;
using System.Text;

namespace CompiladorTraductores2
{
    class Lexical
    {
        private string font;
        private int ind;
        private char c;
        public Symbol result;
        
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
            int state = 0;
            bool cont = true;
            result = new Symbol();
            StringBuilder temp = new StringBuilder();

            while (cont) {
                c = NextChar();
                switch (state) {
                    case 0:
                        if (IsDigit(c))
                        {
                            temp.Append(c);
                            result.name = "entero";
                            result.type = SymbolType.entero;
                            state = 1;
                        }
                        else if (c == '+' || c == '-')
                        {
                            temp.Append(c);
                            result.name = "Add Operator";
                            result.type = SymbolType.opSuma;
                            cont = false;
                        }
                        else if (c == '*' || c == '/')
                        {
                            temp.Append(c);
                            result.name = "Multiplication Operator";
                            result.type = SymbolType.opMul;
                            cont = false;
                        }
                        else if (c == '<' || c == '>')
                        {
                            temp.Append(c);
                            result.name = "Relational Operator";
                            result.type = SymbolType.opRelac;
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
                                result.type = SymbolType.opAnd;
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
                                result.type = SymbolType.opOr;
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
                        else if (c == ',')
                        {
                            temp.Append(c);
                            result.name = "Comma";
                            result.type = SymbolType.Comma;
                            cont = false;
                        }
                        else if (IsLetter(c))
                        {
                            temp.Append(c);
                            result.name = "identificador";
                            result.type = SymbolType.identificador;
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
                            result.name = "cadena";
                            result.type = SymbolType.cadena;
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
                            result.type = SymbolType.entero;
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
                            result.type = SymbolType.real;
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
                            result.type = SymbolType.opRelac;
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
                            result.type = SymbolType.opIgualdad;
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
                                result.type = SymbolType.opNot;
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
                                temp.ToString().ToLower() == "string" ||
                                temp.ToString().ToLower() == "void")
                            {
                                result.name = "tipo";
                                result.type = SymbolType.tipo;
                                cont = false;
                            }
                            else
                            {
                                result.name = "identificador";
                                result.type = SymbolType.identificador;
                            }
                        }
                        else if (IsDigit(c))
                        {
                            temp.Append(c);
                            result.name = "identificador";
                            result.type = SymbolType.identificador;
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