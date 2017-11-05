namespace PagoAgilFrba.Devoluciones
{
    partial class DevolucionForm
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
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtNroFactura = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboClienteDNI = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMotivo = new System.Windows.Forms.RichTextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(172, 359);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(81, 32);
            this.button3.TabIndex = 38;
            this.button3.Text = "Remover Item";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(46, 361);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(79, 30);
            this.button2.TabIndex = 37;
            this.button2.Text = "Agregar Item";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(43, 182);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 36;
            this.label7.Text = "Motivo";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(283, 283);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(96, 108);
            this.listBox1.TabIndex = 35;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(169, 443);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(83, 30);
            this.button1.TabIndex = 34;
            this.button1.Text = "Crear";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtNroFactura
            // 
            this.txtNroFactura.Location = new System.Drawing.Point(161, 280);
            this.txtNroFactura.Margin = new System.Windows.Forms.Padding(2);
            this.txtNroFactura.Name = "txtNroFactura";
            this.txtNroFactura.Size = new System.Drawing.Size(92, 20);
            this.txtNroFactura.TabIndex = 33;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 283);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Numero Factura";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(143, 66);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 24);
            this.label1.TabIndex = 39;
            this.label1.Text = "Devolver factura";
            // 
            // cboClienteDNI
            // 
            this.cboClienteDNI.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboClienteDNI.FormattingEnabled = true;
            this.cboClienteDNI.Location = new System.Drawing.Point(161, 131);
            this.cboClienteDNI.Margin = new System.Windows.Forms.Padding(2);
            this.cboClienteDNI.Name = "cboClienteDNI";
            this.cboClienteDNI.Size = new System.Drawing.Size(92, 21);
            this.cboClienteDNI.TabIndex = 41;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(43, 134);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 40;
            this.label4.Text = "DNI Cliente";
            // 
            // txtMotivo
            // 
            this.txtMotivo.Location = new System.Drawing.Point(161, 182);
            this.txtMotivo.Name = "txtMotivo";
            this.txtMotivo.Size = new System.Drawing.Size(218, 68);
            this.txtMotivo.TabIndex = 42;
            this.txtMotivo.Text = "";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 12);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 61;
            this.button4.Text = "Volver";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // DevolucionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 504);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.txtMotivo);
            this.Controls.Add(this.cboClienteDNI);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtNroFactura);
            this.Controls.Add(this.label2);
            this.Name = "DevolucionForm";
            this.Text = "DevolucionForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtNroFactura;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboClienteDNI;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox txtMotivo;
        private System.Windows.Forms.Button button4;
    }
}