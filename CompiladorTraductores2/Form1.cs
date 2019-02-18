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
                DataGridViewRow row = new DataGridViewRow();

                row.CreateCells(dataGridView1);
                row.Cells[0].Value = s.value;
                row.Cells[1].Value = s.name;
                row.Cells[2].Value = ((int)s.type).ToString();
                if (((int)s.type) == -1) {
                    foreach (DataGridViewCell cell in row.Cells) {
                        cell.Style.BackColor = System.Drawing.Color.Red;
                        cell.Style.ForeColor = System.Drawing.Color.White;
                    }
                }
                dataGridView1.Rows.Add(row);
            }
            
        }
    }
}
