namespace CompiladorTraductores2
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.sourceCodeTxt = new System.Windows.Forms.RichTextBox();
            this.btnParse = new System.Windows.Forms.Button();
            this.SymbolsTable = new System.Windows.Forms.DataGridView();
            this.TokenValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TokenType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Token = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SymbolsTable)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 79.375F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.625F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 359F));
            this.tableLayoutPanel1.Controls.Add(this.sourceCodeTxt, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnParse, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.SymbolsTable, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 106F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 89F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // sourceCodeTxt
            // 
            this.sourceCodeTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceCodeTxt.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sourceCodeTxt.Location = new System.Drawing.Point(3, 3);
            this.sourceCodeTxt.Name = "sourceCodeTxt";
            this.sourceCodeTxt.Size = new System.Drawing.Size(344, 147);
            this.sourceCodeTxt.TabIndex = 0;
            this.sourceCodeTxt.Text = "";
            // 
            // btnParse
            // 
            this.btnParse.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnParse.Location = new System.Drawing.Point(353, 3);
            this.btnParse.Name = "btnParse";
            this.btnParse.Size = new System.Drawing.Size(84, 23);
            this.btnParse.TabIndex = 1;
            this.btnParse.Text = "Parse";
            this.btnParse.UseVisualStyleBackColor = true;
            this.btnParse.Click += new System.EventHandler(this.btnParse_Click);
            // 
            // SymbolsTable
            // 
            this.SymbolsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SymbolsTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TokenValue,
            this.TokenType,
            this.Token});
            this.tableLayoutPanel1.SetColumnSpan(this.SymbolsTable, 3);
            this.SymbolsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SymbolsTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.SymbolsTable.Location = new System.Drawing.Point(3, 156);
            this.SymbolsTable.Name = "SymbolsTable";
            this.SymbolsTable.ReadOnly = true;
            this.SymbolsTable.Size = new System.Drawing.Size(794, 96);
            this.SymbolsTable.TabIndex = 2;
            // 
            // TokenValue
            // 
            this.TokenValue.HeaderText = "Value";
            this.TokenValue.Name = "TokenValue";
            this.TokenValue.ReadOnly = true;
            this.TokenValue.Width = 250;
            // 
            // TokenType
            // 
            this.TokenType.HeaderText = "Type";
            this.TokenType.Name = "TokenType";
            this.TokenType.ReadOnly = true;
            this.TokenType.Width = 250;
            // 
            // Token
            // 
            this.Token.HeaderText = "Token";
            this.Token.Name = "Token";
            this.Token.ReadOnly = true;
            this.Token.Width = 250;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SymbolsTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RichTextBox sourceCodeTxt;
        private System.Windows.Forms.Button btnParse;
        private System.Windows.Forms.DataGridView SymbolsTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn TokenValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn TokenType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Token;
    }
}

