namespace PagoAgilFrba.ListadoEstadistico
{
    partial class ListaPorcentajeFacturasCobradasPorEmpresaForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFechaHasta = new System.Windows.Forms.DateTimePicker();
            this.txtFechaDesde = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(167, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(544, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Porcentaje de facturas cobradas por empresa";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Fecha desde";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Fecha hasta";
            // 
            // txtFechaHasta
            // 
            this.txtFechaHasta.Location = new System.Drawing.Point(217, 150);
            this.txtFechaHasta.Name = "txtFechaHasta";
            this.txtFechaHasta.Size = new System.Drawing.Size(289, 22);
            this.txtFechaHasta.TabIndex = 4;
            // 
            // txtFechaDesde
            // 
            this.txtFechaDesde.Location = new System.Drawing.Point(217, 108);
            this.txtFechaDesde.Name = "txtFechaDesde";
            this.txtFechaDesde.Size = new System.Drawing.Size(289, 22);
            this.txtFechaDesde.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(49, 194);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(143, 39);
            this.button1.TabIndex = 6;
            this.button1.Text = "Generar reporte";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dataGridView1.Location = new System.Drawing.Point(49, 252);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(662, 219);
            this.dataGridView1.TabIndex = 7;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Empresa";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Porcentaje de facturas cobradas";
            this.Column2.Name = "Column2";
            // 
            // ListaPorcentajeFacturasCobradasPorEmpresaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 483);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtFechaDesde);
            this.Controls.Add(this.txtFechaHasta);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ListaPorcentajeFacturasCobradasPorEmpresaForm";
            this.Text = "ListaPorcentajeFacturasCobradasPorEmpresaForm";
            this.Load += new System.EventHandler(this.ListaPorcentajeFacturasCobradasPorEmpresaForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker txtFechaHasta;
        private System.Windows.Forms.DateTimePicker txtFechaDesde;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
    }
}