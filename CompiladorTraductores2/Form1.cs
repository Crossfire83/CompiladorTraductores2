using System;
using System.IO;
using System.Text;
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
            StringBuilder result = new StringBuilder();
            treeView1.Nodes.Clear();
            if (String.IsNullOrWhiteSpace(TablePath)) {
                MessageBox.Show("Por favor cargar archivo con reglas de producción");
                return;
            }
            try
            {
                s.SetLRTable(File.ReadAllLines(TablePath));
                result.Append(s.Analiza(sourceCodeTxt.Text));
                if (result.Length == 0/*String.IsNullOrWhiteSpace(result)*/)
                {
                    result.Append("Análisis lexico y sintactico terminado.");
                }
                StackTextBox.Text = s.stack.ToString();
                PopulateTreeView();
            }
            catch (Exception ex) {
                result.Append(Environment.NewLine);
                result.Append(ex.Message);
                //MessageBox.Show(ex.ToString());
            }
            ResultTextBox.Text = result.ToString();
        }

        private void PopulateTreeView()
        {
            TreeNode rootNode;

            rootNode = new TreeNode(s.Root.ImprimeTipo());
            GetSubNodes(rootNode, ((Programa)s.Root).defs);
            treeView1.Nodes.Add(rootNode);
        }

        private void GetSubNodes(TreeNode nodeToAddTo, NonTerminal subnode)
        {
            string typeName = subnode.ImprimeTipo().Trim();
            TreeNode childNode = new TreeNode(typeName);
            if (subnode.containsChildren) {
                switch (typeName) {
                    case "Definiciones":
                        GetSubNodes(childNode, ((Definiciones)subnode).def);
                        GetSubNodes(childNode, ((Definiciones)subnode).defs);
                        break;
                    case "Definicion":
                        GetSubNodes(childNode, ((Definicion)subnode).GetChild());
                        break;
                    case "DefVar":
                        childNode.Nodes.Add(((DefVar)subnode).tipo.value);
                        childNode.Nodes.Add(((DefVar)subnode).id.value);
                        GetSubNodes(childNode, ((DefVar)subnode).lvar);
                        break;
                    case "ListaVar":
                        childNode.Nodes.Add(((ListaVar)subnode).id.value);
                        GetSubNodes(childNode, ((ListaVar)subnode).lvar);
                        break;
                    case "DefFunc":
                        childNode.Nodes.Add(((DefFunc)subnode).tipo.value);
                        childNode.Nodes.Add(((DefFunc)subnode).id.value);
                        GetSubNodes(childNode, ((DefFunc)subnode).parametros);
                        GetSubNodes(childNode, ((DefFunc)subnode).bloqueFunc);
                        break;
                    case "Parametros":
                        childNode.Nodes.Add(((Parametros)subnode).tipo.value);
                        childNode.Nodes.Add(((Parametros)subnode).id.value);
                        GetSubNodes(childNode, ((Parametros)subnode).listaParams);
                        break;
                    case "ListaParam":
                        childNode.Nodes.Add(((ListaParam)subnode).tipo.value);
                        childNode.Nodes.Add(((ListaParam)subnode).id.value);
                        GetSubNodes(childNode, ((ListaParam)subnode).listaParams);
                        break;
                    case "BloqFunc":
                        GetSubNodes(childNode, ((BloqFunc)subnode).locales);
                        break;
                    case "DefLocales":
                        GetSubNodes(childNode, ((DefLocales)subnode).def);
                        GetSubNodes(childNode, ((DefLocales)subnode).defs);
                        break;
                    case "DefLocal":
                        GetSubNodes(childNode, ((DefLocal)subnode).GetChild());
                        break;
                    case "Sentencias":
                        GetSubNodes(childNode, ((Sentencias)subnode).sent);
                        GetSubNodes(childNode, ((Sentencias)subnode).sents);
                        break;
                    case "Sentencia":
                        if (subnode is Asignacion)
                        {
                            childNode.Nodes.Add(((Asignacion)subnode).id.value);
                            GetSubNodes(childNode, ((Asignacion)subnode).expresion);
                        }
                        else if (subnode is If)
                        {
                            GetSubNodes(childNode, ((If)subnode).expresion);
                            GetSubNodes(childNode, ((If)subnode).sentenciabloque);
                            GetSubNodes(childNode, ((If)subnode).otro);
                        }
                        else if (subnode is While)
                        {
                            GetSubNodes(childNode, ((While)subnode).expresion);
                            GetSubNodes(childNode, ((While)subnode).bloque);
                        }
                        else if (subnode is Return)
                        {
                            GetSubNodes(childNode, ((Return)subnode).exp);
                        }
                        else if (subnode is SentenciaLLama)
                        {
                            GetSubNodes(childNode, ((SentenciaLLama)subnode).llamadaFunc);
                        }
                        break;
                    case "Otro":
                        GetSubNodes(childNode, ((Otro)subnode).sentBloq);
                        break;
                    case "Bloque":
                        GetSubNodes(childNode, ((Bloque)subnode).sents);
                        break;
                    case "ValorRegresa":
                        GetSubNodes(childNode, ((ValorRegresa)subnode).exp);
                        break;
                    case "Argumentos":
                        GetSubNodes(childNode, ((Argumentos)subnode).expr);
                        GetSubNodes(childNode, ((Argumentos)subnode).lArgs);
                        break;
                    case "ListaArgumentos":
                        GetSubNodes(childNode, ((ListaArgumentos)subnode).expr);
                        GetSubNodes(childNode, ((ListaArgumentos)subnode).lArgs);
                        break;
                    case "Termino":
                        object temp = ((Termino)subnode).getChild();
                        if (temp is Symbol)
                        {
                            childNode.Nodes.Add(((Symbol)temp).value);
                        }
                        else {
                            GetSubNodes(childNode, (LlamadaFunc)temp);
                        }
                        break;
                    case "LlamadaFunc":
                        childNode.Nodes.Add(((LlamadaFunc)subnode).id.value);
                        GetSubNodes(childNode, ((LlamadaFunc)subnode).argumentos);
                        break;
                    case "SentenciaBloque":
                        GetSubNodes(childNode, ((SentenciaBloque)subnode).GetChild());
                        break;
                    case "Expresion":
                        if (subnode is Operacion1) {
                            childNode.Nodes.Add(((Operacion1)subnode).symbol.value);
                            GetSubNodes(childNode, ((Operacion1)subnode).der);
                        }
                        else if (subnode is Operacion2) {
                            GetSubNodes(childNode, ((Operacion2)subnode).izq);
                            childNode.Nodes.Add(((Operacion2)subnode).symbol.value);
                            GetSubNodes(childNode, ((Operacion2)subnode).der);
                        }
                        else {
                            GetSubNodes(childNode, ((Expresion)subnode).GetChild());
                        }
                        break;
                    default:
                        break;
                }
            }
            nodeToAddTo.Nodes.Add(childNode);
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
            
            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    file = openFileDialog.FileName;
                }
            }
            catch (Exception ex)
            {
            MessageBox.Show("El archivo no se puede abrir", "Se ha producido un error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return file;
        }
    }
}
