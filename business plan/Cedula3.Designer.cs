namespace business_plan
{
    partial class Cedula3
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
            System.Windows.Forms.TabPage tabPage1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Cedula3));
            this.dgvCed3 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.cbEstructura2 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.CbCategoria = new System.Windows.Forms.ComboBox();
            this.cbModificar = new System.Windows.Forms.ComboBox();
            this.chbModificar = new System.Windows.Forms.CheckBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.dtpFechafinal = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpFechainicial = new System.Windows.Forms.DateTimePicker();
            this.btnSimular = new System.Windows.Forms.Button();
            this.dtpEscenario = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.PanelHeader = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.progressBarCed1 = new System.Windows.Forms.ProgressBar();
            this.foot = new System.Windows.Forms.Panel();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvCed3Rep = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            tabPage1 = new System.Windows.Forms.TabPage();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCed3)).BeginInit();
            this.panel1.SuspendLayout();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.foot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCed3Rep)).BeginInit();
            this.SuspendLayout();
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(this.dgvCed3);
            tabPage1.Controls.Add(this.panel1);
            tabPage1.Location = new System.Drawing.Point(4, 22);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3);
            tabPage1.Size = new System.Drawing.Size(1241, 294);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Simulador";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvCed3
            // 
            this.dgvCed3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCed3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.dgvCed3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCed3.Location = new System.Drawing.Point(3, 103);
            this.dgvCed3.Name = "dgvCed3";
            this.dgvCed3.Size = new System.Drawing.Size(1235, 188);
            this.dgvCed3.TabIndex = 1;
            // 
            // Column1
            // 
            this.Column1.Frozen = true;
            this.Column1.HeaderText = "Estructura";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.cbEstructura2);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.btnNuevo);
            this.panel1.Controls.Add(this.CbCategoria);
            this.panel1.Controls.Add(this.cbModificar);
            this.panel1.Controls.Add(this.chbModificar);
            this.panel1.Controls.Add(this.btnGuardar);
            this.panel1.Controls.Add(this.dtpFechafinal);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dtpFechainicial);
            this.panel1.Controls.Add(this.btnSimular);
            this.panel1.Controls.Add(this.dtpEscenario);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1235, 100);
            this.panel1.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(240, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 13);
            this.label9.TabIndex = 34;
            this.label9.Text = "Estructura";
            // 
            // cbEstructura2
            // 
            this.cbEstructura2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEstructura2.FormattingEnabled = true;
            this.cbEstructura2.Items.AddRange(new object[] {
            "Division",
            "Departamento",
            "Familia",
            "Linea",
            "Linea 1",
            "Linea 2",
            "Linea 3",
            "Linea 4",
            "Linea 5",
            "Linea 6",
            "Marca"});
            this.cbEstructura2.Location = new System.Drawing.Point(207, 38);
            this.cbEstructura2.Name = "cbEstructura2";
            this.cbEstructura2.Size = new System.Drawing.Size(172, 21);
            this.cbEstructura2.TabIndex = 33;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(65, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 13);
            this.label8.TabIndex = 32;
            this.label8.Text = "Sucursales";
            // 
            // btnNuevo
            // 
            this.btnNuevo.Location = new System.Drawing.Point(1106, 10);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(110, 28);
            this.btnNuevo.TabIndex = 31;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.UseVisualStyleBackColor = true;
            // 
            // CbCategoria
            // 
            this.CbCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbCategoria.FormattingEnabled = true;
            this.CbCategoria.Items.AddRange(new object[] {
            "Total ",
            "Juarez",
            "Hidalgo",
            "Triana",
            "Matriz"});
            this.CbCategoria.Location = new System.Drawing.Point(18, 38);
            this.CbCategoria.Name = "CbCategoria";
            this.CbCategoria.Size = new System.Drawing.Size(172, 21);
            this.CbCategoria.TabIndex = 30;
            // 
            // cbModificar
            // 
            this.cbModificar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbModificar.FormattingEnabled = true;
            this.cbModificar.Location = new System.Drawing.Point(93, 70);
            this.cbModificar.Name = "cbModificar";
            this.cbModificar.Size = new System.Drawing.Size(212, 21);
            this.cbModificar.TabIndex = 29;
            this.cbModificar.Visible = false;
            // 
            // chbModificar
            // 
            this.chbModificar.AutoSize = true;
            this.chbModificar.Location = new System.Drawing.Point(18, 72);
            this.chbModificar.Name = "chbModificar";
            this.chbModificar.Size = new System.Drawing.Size(69, 17);
            this.chbModificar.TabIndex = 28;
            this.chbModificar.Text = "Modificar";
            this.chbModificar.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(1106, 51);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(110, 28);
            this.btnGuardar.TabIndex = 27;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            // 
            // dtpFechafinal
            // 
            this.dtpFechafinal.CustomFormat = "yyyy-MM-dd";
            this.dtpFechafinal.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFechafinal.Location = new System.Drawing.Point(909, 15);
            this.dtpFechafinal.Name = "dtpFechafinal";
            this.dtpFechafinal.Size = new System.Drawing.Size(78, 20);
            this.dtpFechafinal.TabIndex = 26;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(815, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Fecha inicial";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(353, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Fecha del escenario";
            // 
            // dtpFechainicial
            // 
            this.dtpFechainicial.CustomFormat = "yyyy-MM-dd";
            this.dtpFechainicial.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFechainicial.Location = new System.Drawing.Point(691, 15);
            this.dtpFechainicial.Name = "dtpFechainicial";
            this.dtpFechainicial.Size = new System.Drawing.Size(78, 20);
            this.dtpFechainicial.TabIndex = 24;
            // 
            // btnSimular
            // 
            this.btnSimular.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnSimular.Location = new System.Drawing.Point(980, 51);
            this.btnSimular.Name = "btnSimular";
            this.btnSimular.Size = new System.Drawing.Size(110, 28);
            this.btnSimular.TabIndex = 22;
            this.btnSimular.Text = "Simular";
            this.btnSimular.UseVisualStyleBackColor = false;
            // 
            // dtpEscenario
            // 
            this.dtpEscenario.CustomFormat = "yyyy-MM-dd";
            this.dtpEscenario.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEscenario.Location = new System.Drawing.Point(480, 15);
            this.dtpEscenario.Name = "dtpEscenario";
            this.dtpEscenario.Size = new System.Drawing.Size(78, 20);
            this.dtpEscenario.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(610, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Fecha inicial";
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.label6);
            this.PanelHeader.Controls.Add(this.label4);
            this.PanelHeader.Controls.Add(this.pictureBox2);
            this.PanelHeader.Controls.Add(this.pictureBox1);
            this.PanelHeader.Controls.Add(this.progressBarCed1);
            this.PanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelHeader.Location = new System.Drawing.Point(0, 0);
            this.PanelHeader.Name = "PanelHeader";
            this.PanelHeader.Size = new System.Drawing.Size(1249, 184);
            this.PanelHeader.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(0, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(363, 29);
            this.label6.TabIndex = 7;
            this.label6.Text = "Flujo de compra/venta/inventario";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(502, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(237, 39);
            this.label4.TabIndex = 5;
            this.label4.Text = "Business Plan";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(0, 135);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(1249, 49);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1249, 90);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // progressBarCed1
            // 
            this.progressBarCed1.BackColor = System.Drawing.Color.Gold;
            this.progressBarCed1.Location = new System.Drawing.Point(3, 38);
            this.progressBarCed1.Name = "progressBarCed1";
            this.progressBarCed1.Size = new System.Drawing.Size(262, 29);
            this.progressBarCed1.Step = 1000;
            this.progressBarCed1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarCed1.TabIndex = 2;
            this.progressBarCed1.Visible = false;
            // 
            // foot
            // 
            this.foot.Controls.Add(this.btnCerrar);
            this.foot.Controls.Add(this.pictureBox3);
            this.foot.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.foot.Location = new System.Drawing.Point(0, 504);
            this.foot.Name = "foot";
            this.foot.Size = new System.Drawing.Size(1249, 49);
            this.foot.TabIndex = 3;
            // 
            // btnCerrar
            // 
            this.btnCerrar.Location = new System.Drawing.Point(600, 3);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(75, 47);
            this.btnCerrar.TabIndex = 3;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(0, -5);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(1249, 54);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 2;
            this.pictureBox3.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 184);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1249, 320);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvCed3Rep);
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1241, 294);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Reporte";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvCed3Rep
            // 
            this.dgvCed3Rep.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCed3Rep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCed3Rep.Location = new System.Drawing.Point(3, 103);
            this.dgvCed3Rep.Name = "dgvCed3Rep";
            this.dgvCed3Rep.Size = new System.Drawing.Size(1235, 188);
            this.dgvCed3Rep.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1235, 100);
            this.panel2.TabIndex = 0;
            // 
            // Cedula3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1249, 553);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.foot);
            this.Controls.Add(this.PanelHeader);
            this.Name = "Cedula3";
            this.Text = "Cedula3";
            this.Load += new System.EventHandler(this.Cedula3_Load);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCed3)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.foot.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCed3Rep)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelHeader;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ProgressBar progressBarCed1;
        private System.Windows.Forms.Panel foot;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgvCed3;
        private System.Windows.Forms.DataGridView dgvCed3Rep;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbEstructura2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.ComboBox CbCategoria;
        private System.Windows.Forms.ComboBox cbModificar;
        private System.Windows.Forms.CheckBox chbModificar;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.DateTimePicker dtpFechafinal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpFechainicial;
        private System.Windows.Forms.Button btnSimular;
        private System.Windows.Forms.DateTimePicker dtpEscenario;
        private System.Windows.Forms.Label label2;

    }
}