using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorTraductores2
{
    public enum SymbolType
    {
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

    public class Symbol
    {
        public SymbolType type;
        public string name;
        public string value;
    }
}
