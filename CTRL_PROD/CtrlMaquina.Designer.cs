namespace CTRL_PROD
{
    partial class CtrlMaquina
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlMaquina));
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvPoints = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblTempoComm = new System.Windows.Forms.TextBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblNumPontosNC = new System.Windows.Forms.TextBox();
            this.dgvHistLastPoints = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label10 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblMin = new System.Windows.Forms.TextBox();
            this.lblMax = new System.Windows.Forms.TextBox();
            this.lblMedia = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.limparLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportarPFicheiroToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtClassificacao = new System.Windows.Forms.TextBox();
            this.txtDiametro = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ledPLC = new System.Windows.Forms.PictureBox();
            this.ledDB = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNomeMaquina = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPoints)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistLastPoints)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ledPLC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ledDB)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.txtClassificacao);
            this.panel1.Controls.Add(this.txtDiametro);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.ledPLC);
            this.panel1.Controls.Add(this.ledDB);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtNomeMaquina);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(430, 350);
            this.panel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(11, 63);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(410, 181);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.dgvPoints);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(402, 155);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Monitorização Pontos NC";
            // 
            // dgvPoints
            // 
            this.dgvPoints.AllowUserToAddRows = false;
            this.dgvPoints.AllowUserToDeleteRows = false;
            this.dgvPoints.AllowUserToResizeRows = false;
            this.dgvPoints.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPoints.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPoints.ColumnHeadersHeight = 21;
            this.dgvPoints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPoints.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn4});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPoints.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPoints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPoints.Location = new System.Drawing.Point(0, 0);
            this.dgvPoints.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgvPoints.MultiSelect = false;
            this.dgvPoints.Name = "dgvPoints";
            this.dgvPoints.ReadOnly = true;
            this.dgvPoints.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvPoints.RowHeadersVisible = false;
            this.dgvPoints.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            this.dgvPoints.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvPoints.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.dgvPoints.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvPoints.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPoints.ShowCellErrors = false;
            this.dgvPoints.ShowCellToolTips = false;
            this.dgvPoints.ShowEditingIcon = false;
            this.dgvPoints.ShowRowErrors = false;
            this.dgvPoints.Size = new System.Drawing.Size(402, 155);
            this.dgvPoints.TabIndex = 11;
            this.dgvPoints.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvPoints_KeyPress);
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn2.HeaderText = "Diâmetro";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 70;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn5.HeaderText = "Datahora";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 140;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn4.HeaderText = "Classificação";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 165;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.lblTempoComm);
            this.tabPage2.Controls.Add(this.btnReset);
            this.tabPage2.Controls.Add(this.lblNumPontosNC);
            this.tabPage2.Controls.Add(this.dgvHistLastPoints);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.lblMin);
            this.tabPage2.Controls.Add(this.lblMax);
            this.tabPage2.Controls.Add(this.lblMedia);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(402, 155);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Últimos Pontos";
            // 
            // lblTempoComm
            // 
            this.lblTempoComm.BackColor = System.Drawing.Color.Black;
            this.lblTempoComm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTempoComm.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblTempoComm.Location = new System.Drawing.Point(325, 102);
            this.lblTempoComm.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lblTempoComm.Name = "lblTempoComm";
            this.lblTempoComm.Size = new System.Drawing.Size(73, 21);
            this.lblTempoComm.TabIndex = 104;
            this.lblTempoComm.Text = "---";
            this.lblTempoComm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnReset
            // 
            this.btnReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Location = new System.Drawing.Point(224, 74);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(174, 23);
            this.btnReset.TabIndex = 103;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // lblNumPontosNC
            // 
            this.lblNumPontosNC.BackColor = System.Drawing.Color.Black;
            this.lblNumPontosNC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNumPontosNC.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblNumPontosNC.Location = new System.Drawing.Point(325, 125);
            this.lblNumPontosNC.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lblNumPontosNC.Name = "lblNumPontosNC";
            this.lblNumPontosNC.Size = new System.Drawing.Size(73, 21);
            this.lblNumPontosNC.TabIndex = 105;
            this.lblNumPontosNC.Text = "---";
            this.lblNumPontosNC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dgvHistLastPoints
            // 
            this.dgvHistLastPoints.AllowUserToAddRows = false;
            this.dgvHistLastPoints.AllowUserToDeleteRows = false;
            this.dgvHistLastPoints.AllowUserToResizeRows = false;
            this.dgvHistLastPoints.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHistLastPoints.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvHistLastPoints.ColumnHeadersHeight = 21;
            this.dgvHistLastPoints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvHistLastPoints.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn3});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvHistLastPoints.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvHistLastPoints.Dock = System.Windows.Forms.DockStyle.Left;
            this.dgvHistLastPoints.Location = new System.Drawing.Point(0, 0);
            this.dgvHistLastPoints.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgvHistLastPoints.MultiSelect = false;
            this.dgvHistLastPoints.Name = "dgvHistLastPoints";
            this.dgvHistLastPoints.ReadOnly = true;
            this.dgvHistLastPoints.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvHistLastPoints.RowHeadersVisible = false;
            this.dgvHistLastPoints.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            this.dgvHistLastPoints.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvHistLastPoints.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.dgvHistLastPoints.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvHistLastPoints.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHistLastPoints.ShowCellErrors = false;
            this.dgvHistLastPoints.ShowCellToolTips = false;
            this.dgvHistLastPoints.ShowEditingIcon = false;
            this.dgvHistLastPoints.ShowRowErrors = false;
            this.dgvHistLastPoints.Size = new System.Drawing.Size(218, 155);
            this.dgvHistLastPoints.TabIndex = 12;
            this.dgvHistLastPoints.SelectionChanged += new System.EventHandler(this.dgvHistLastPoints_SelectionChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn1.HeaderText = "Diâmetro";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 75;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn3.HeaderText = "Datahora";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 140;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(221, 128);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Pontos NC:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(221, 104);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Tempo Com.:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(221, 52);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Mínimo:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(221, 29);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Máximo:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(221, 6);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Média:";
            // 
            // lblMin
            // 
            this.lblMin.BackColor = System.Drawing.Color.Black;
            this.lblMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMin.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblMin.Location = new System.Drawing.Point(325, 49);
            this.lblMin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(73, 21);
            this.lblMin.TabIndex = 102;
            this.lblMin.Text = "---";
            this.lblMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblMax
            // 
            this.lblMax.BackColor = System.Drawing.Color.Black;
            this.lblMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMax.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblMax.Location = new System.Drawing.Point(325, 26);
            this.lblMax.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lblMax.Name = "lblMax";
            this.lblMax.Size = new System.Drawing.Size(73, 21);
            this.lblMax.TabIndex = 101;
            this.lblMax.Text = "---";
            this.lblMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblMedia
            // 
            this.lblMedia.BackColor = System.Drawing.Color.Black;
            this.lblMedia.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMedia.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblMedia.Location = new System.Drawing.Point(325, 3);
            this.lblMedia.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lblMedia.Name = "lblMedia";
            this.lblMedia.Size = new System.Drawing.Size(73, 21);
            this.lblMedia.TabIndex = 100;
            this.lblMedia.Text = "---";
            this.lblMedia.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtLog);
            this.groupBox2.Location = new System.Drawing.Point(11, 246);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Size = new System.Drawing.Size(410, 95);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Logs";
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.Black;
            this.txtLog.ContextMenuStrip = this.contextMenuStrip1;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.ForeColor = System.Drawing.Color.Lime;
            this.txtLog.Location = new System.Drawing.Point(4, 17);
            this.txtLog.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.txtLog.MaxLength = 0;
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(402, 75);
            this.txtLog.TabIndex = 12;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.limparLogsToolStripMenuItem,
            this.exportarPFicheiroToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(209, 48);
            // 
            // limparLogsToolStripMenuItem
            // 
            this.limparLogsToolStripMenuItem.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.limparLogsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("limparLogsToolStripMenuItem.Image")));
            this.limparLogsToolStripMenuItem.Name = "limparLogsToolStripMenuItem";
            this.limparLogsToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.limparLogsToolStripMenuItem.Text = "Limpar Logs";
            this.limparLogsToolStripMenuItem.Click += new System.EventHandler(this.limparLogsToolStripMenuItem_Click);
            // 
            // exportarPFicheiroToolStripMenuItem
            // 
            this.exportarPFicheiroToolStripMenuItem.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.exportarPFicheiroToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportarPFicheiroToolStripMenuItem.Image")));
            this.exportarPFicheiroToolStripMenuItem.Name = "exportarPFicheiroToolStripMenuItem";
            this.exportarPFicheiroToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.exportarPFicheiroToolStripMenuItem.Text = "Exportar p/ Ficheiro";
            this.exportarPFicheiroToolStripMenuItem.Click += new System.EventHandler(this.exportarPFicheiroToolStripMenuItem_Click);
            // 
            // txtClassificacao
            // 
            this.txtClassificacao.BackColor = System.Drawing.Color.Black;
            this.txtClassificacao.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtClassificacao.ForeColor = System.Drawing.Color.LimeGreen;
            this.txtClassificacao.Location = new System.Drawing.Point(255, 38);
            this.txtClassificacao.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtClassificacao.Name = "txtClassificacao";
            this.txtClassificacao.Size = new System.Drawing.Size(166, 21);
            this.txtClassificacao.TabIndex = 6;
            this.txtClassificacao.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtDiametro
            // 
            this.txtDiametro.BackColor = System.Drawing.Color.Black;
            this.txtDiametro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDiametro.ForeColor = System.Drawing.Color.LimeGreen;
            this.txtDiametro.Location = new System.Drawing.Point(84, 38);
            this.txtDiametro.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtDiametro.Name = "txtDiametro";
            this.txtDiametro.Size = new System.Drawing.Size(73, 21);
            this.txtDiametro.TabIndex = 6;
            this.txtDiametro.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 42);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Diâmetro:";
            // 
            // ledPLC
            // 
            this.ledPLC.Image = global::CTRL_PROD.Properties.Resources.ledRed;
            this.ledPLC.Location = new System.Drawing.Point(332, 6);
            this.ledPLC.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ledPLC.Name = "ledPLC";
            this.ledPLC.Size = new System.Drawing.Size(24, 24);
            this.ledPLC.TabIndex = 4;
            this.ledPLC.TabStop = false;
            // 
            // ledDB
            // 
            this.ledDB.Image = global::CTRL_PROD.Properties.Resources.ledRed;
            this.ledDB.Location = new System.Drawing.Point(397, 6);
            this.ledDB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ledDB.Name = "ledDB";
            this.ledDB.Size = new System.Drawing.Size(24, 24);
            this.ledDB.TabIndex = 4;
            this.ledDB.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(159, 42);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Classificação:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(270, 11);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Leitura:";
            // 
            // txtNomeMaquina
            // 
            this.txtNomeMaquina.BackColor = System.Drawing.Color.Black;
            this.txtNomeMaquina.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNomeMaquina.ForeColor = System.Drawing.Color.White;
            this.txtNomeMaquina.Location = new System.Drawing.Point(84, 7);
            this.txtNomeMaquina.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtNomeMaquina.Name = "txtNomeMaquina";
            this.txtNomeMaquina.ReadOnly = true;
            this.txtNomeMaquina.Size = new System.Drawing.Size(182, 21);
            this.txtNomeMaquina.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(365, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "DB:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Máquina:";
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // CtrlMaquina
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "CtrlMaquina";
            this.Size = new System.Drawing.Size(430, 350);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPoints)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistLastPoints)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ledPLC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ledDB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox ledPLC;
        private System.Windows.Forms.PictureBox ledDB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtNomeMaquina;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtClassificacao;
        private System.Windows.Forms.TextBox txtDiametro;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgvPoints;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem limparLogsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportarPFicheiroToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.TextBox lblNumPontosNC;
        private System.Windows.Forms.DataGridView dgvHistLastPoints;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox lblMin;
        private System.Windows.Forms.TextBox lblMax;
        private System.Windows.Forms.TextBox lblMedia;
        private System.Windows.Forms.TextBox lblTempoComm;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    }
}
