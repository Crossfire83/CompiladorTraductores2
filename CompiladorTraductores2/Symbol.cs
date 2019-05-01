namespace CompiladorTraductores2
{
    public enum SymbolType
    {
        Error = -1,
        identificador = 0,
        entero = 1,
        real = 2,
        cadena = 3,
        tipo = 4,
        opSuma = 5,
        opMul = 6,
        opRelac = 7,
        opOr = 8,
        opAnd = 9,
        opNot = 10,
        opIgualdad = 11,
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

    public class Symbol
    {
        public SymbolType type;
        public string name;
        public string value;
        public int linea;

        public override string ToString()
        {
            return "Name: " + name + "; Value: " + value;
        }
    }
}
