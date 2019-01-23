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

        }
    }
}
