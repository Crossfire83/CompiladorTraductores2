using System;
using System.IO;
using System.Windows.Forms;

namespace CompiladorTraductores2
{
    public partial class Form1 : Form
    {
        Sintactical s;
        string TablePath;
        string RulesPath;
        public Form1()
        {
            InitializeComponent();
            TablePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\TablaLR.txt";
            RulesPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Producciones.txt";
            s = new Sintactical(ref SymbolsTable);
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            s.SetLRTable(File.ReadAllLines(TablePath));
            s.SetRules(File.ReadAllText(RulesPath));
            s.Analiza(sourceCodeTxt.Text);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string temp = ShowOpenFileDialog("Seleccione archivo con el codigo fuente", "Text files (*.txt)|*.txt");
            if (temp != String.Empty) {
                sourceCodeTxt.Text = File.ReadAllText(temp);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            string temp = ShowOpenFileDialog("Seleccione archivo con la tabla LR", "Text files (*.txt)|*.txt");
            if (temp != String.Empty) {
                TablePath = temp;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            string temp = ShowOpenFileDialog("Seleccione archivo con las Reglas de Produccion", "Text files (*.txt)|*.txt");
            if (temp != String.Empty)
            {
                RulesPath = temp;
            }
        }

        private string ShowOpenFileDialog(string title, string filter)
        {
            string file = String.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = title,
                Filter = filter,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    file = openFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("El archivo no se puede abrir", "Se ha producido un error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return file;
        }
    }
}
