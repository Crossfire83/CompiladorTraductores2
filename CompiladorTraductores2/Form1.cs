using System;
using System.IO;
using System.Windows.Forms;

namespace CompiladorTraductores2
{
    public partial class Form1 : Form
    {
        Sintactical s;
        string TablePath;
        public Form1()
        {
            InitializeComponent();
            TablePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\compilador.lr";
            s = new Sintactical(ref SymbolsTable);
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            SymbolsTable.Rows.Clear();
            StackTextBox.Text = String.Empty;
            ResultTextBox.Text = String.Empty;
            if (String.IsNullOrWhiteSpace(TablePath)) {
                MessageBox.Show("Por favor cargar archivo con reglas de producción");
                return;
            }
            try
            {
                s.SetLRTable(File.ReadAllLines(TablePath));
                string result = s.Analiza(sourceCodeTxt.Text);
                if (String.IsNullOrWhiteSpace(result))
                {
                    ResultTextBox.Text = "Análisis lexico y sintactico terminado.";
                }
                else {
                    ResultTextBox.Text = result;
                }
                StackTextBox.Text = s.stack.ToString();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
            
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
            string temp = ShowOpenFileDialog("Seleccione archivo con la tabla LR y Reglas de Produccion", "LR files (*.lr)|*.lr");
            if (temp != String.Empty) {
                TablePath = temp;
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
