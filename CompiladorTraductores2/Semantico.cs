using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorTraductores2
{
    public class Semantico
    {
        public List<ElementoTabla> Simbolos;
        public List<string> errores;

        public Semantico() {
            Simbolos = new List<ElementoTabla>();
            errores = new List<string>();
        }
    }

    public class ElementoTabla
    {
        public string Id { get; set; }
        public string Tipo { get; set; }
        public string Ambito { get; set; }
        public string stParametro { get; set; }

        public ElementoTabla(string _id, string _tipo, string _ambito, string _stpara)
        {
            Id = _id;
            Tipo = _tipo;
            Ambito = _ambito;
            stParametro = _stpara;
        }
        public ElementoTabla(string _id, string _tipo, string _ambito)
        {
            Id = _id;
            Tipo = _tipo;
            Ambito = _ambito;
            stParametro = "";
        }
    }
}
