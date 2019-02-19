using System;
using System.Collections.Generic;
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
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            s = new Sintactical(sourceCodeTxt.Text, ref SymbolsTable);
            s.SetLRTable(File.ReadAllLines(TablePath));
            s.SetRules(File.ReadAllText(RulesPath));

            while (!s.l.IsFinished()) {
                Symbol sym = s.l.NextSymbol();
                DataGridViewRow row = new DataGridViewRow();

                row.CreateCells(SymbolsTable);
                row.Cells[0].Value = sym.value;
                row.Cells[1].Value = sym.name;
                row.Cells[2].Value = ((int)sym.type).ToString();
                if (((int)sym.type) == -1) {
                    foreach (DataGridViewCell cell in row.Cells) {
                        cell.Style.BackColor = System.Drawing.Color.Red;
                        cell.Style.ForeColor = System.Drawing.Color.White;
                    }
                }
                SymbolsTable.Rows.Add(row);
            }
            
        }
    }
}
