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
            dataGridView1.Rows.Clear();
            l.Input(sourceCodeTxt.Text);
            List<Symbol> words = new List<Symbol>();
            while (!l.IsFinished()) {
                Symbol s = l.NextSymbol();
                words.Add(s);
                dataGridView1.Rows.Add(s.value, s.name, ((int)s.type).ToString());
            }
            
        }
    }
}
