using System.Collections.Generic;
using System.Windows.Forms;

namespace CompiladorTraductores2
{
    public partial class Form1 : Form
    {
        Lexical l;
        public Form1()
        {
            InitializeComponent();
            l = new Lexical();
        }

        private void btnParse_Click(object sender, System.EventArgs e)
        {
            l.Input(sourceCodeTxt.Text);
            List<Symbol> words = new List<Symbol>();
            while (!l.IsFinished()) {
                words.Add(l.NextSymbol());
            }
        }
    }
}
