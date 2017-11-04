namespace PagoAgilFrba.AbmCliente
{
    partial class EditClienteForm
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
            this.txtFecha = new System.Windows.Forms.DateTimePicker();
            this.txtCodigoPostal = new System.Windows.Forms.TextBox();
            this.txtDireccion = new System.Windows.Forms.TextBox();
            this.txtTelefono = new System.Windows.Forms.TextBox();
            this.txtMail = new System.Windows.Forms.TextBox();
            this.txtDNI = new System.Windows.Forms.TextBox();
            this.txtApellido = new System.Windows.Forms.TextBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cbkActivo = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.clientesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sucursalesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rolesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.empresasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.facturasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFecha
            // 
            this.txtFecha.Location = new System.Drawing.Point(232, 407);
            this.txtFecha.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtFecha.Name = "txtFecha";
            this.txtFecha.Size = new System.Drawing.Size(309, 22);
            this.txtFecha.TabIndex = 34;
            // 
            // txtCodigoPostal
            // 
            this.txtCodigoPostal.Location = new System.Drawing.Point(232, 366);
            this.txtCodigoPostal.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCodigoPostal.Name = "txtCodigoPostal";
            this.txtCodigoPostal.Size = new System.Drawing.Size(312, 22);
            this.txtCodigoPostal.TabIndex = 33;
            // 
            // txtDireccion
            // 
            this.txtDireccion.Location = new System.Drawing.Point(232, 327);
            this.txtDireccion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtDireccion.Name = "txtDireccion";
            this.txtDireccion.Size = new System.Drawing.Size(312, 22);
            this.txtDireccion.TabIndex = 32;
            // 
            // txtTelefono
            // 
            this.txtTelefono.Location = new System.Drawing.Point(232, 287);
            this.txtTelefono.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTelefono.Name = "txtTelefono";
            this.txtTelefono.Size = new System.Drawing.Size(312, 22);
            this.txtTelefono.TabIndex = 31;
            // 
            // txtMail
            // 
            this.txtMail.Location = new System.Drawing.Point(232, 250);
            this.txtMail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtMail.Name = "txtMail";
            this.txtMail.Size = new System.Drawing.Size(312, 22);
            this.txtMail.TabIndex = 30;
            // 
            // txtDNI
            // 
            this.txtDNI.Enabled = false;
            this.txtDNI.Location = new System.Drawing.Point(232, 209);
            this.txtDNI.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtDNI.Name = "txtDNI";
            this.txtDNI.Size = new System.Drawing.Size(312, 22);
            this.txtDNI.TabIndex = 29;
            // 
            // txtApellido
            // 
            this.txtApellido.Location = new System.Drawing.Point(232, 174);
            this.txtApellido.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtApellido.Name = "txtApellido";
            this.txtApellido.Size = new System.Drawing.Size(312, 22);
            this.txtApellido.TabIndex = 28;
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(232, 133);
            this.txtNombre.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(312, 22);
            this.txtNombre.TabIndex = 27;
            this.txtNombre.TextChanged += new System.EventHandler(this.txtNombre_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(56, 407);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(139, 17);
            this.label9.TabIndex = 26;
            this.label9.Text = "Fecha de nacimiento";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(56, 370);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 17);
            this.label8.TabIndex = 25;
            this.label8.Text = "Codigo postal";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(315, 519);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 30);
            this.button1.TabIndex = 24;
            this.button1.Text = "Guardar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(56, 332);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 17);
            this.label7.TabIndex = 23;
            this.label7.Text = "Dirección";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(56, 290);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 17);
            this.label6.TabIndex = 22;
            this.label6.Text = "Teléfono";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(56, 254);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 17);
            this.label5.TabIndex = 21;
            this.label5.Text = "Mail";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(56, 213);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 17);
            this.label4.TabIndex = 20;
            this.label4.Text = "DNI";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(56, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 17);
            this.label3.TabIndex = 19;
            this.label3.Text = "Apellido";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 17);
            this.label2.TabIndex = 18;
            this.label2.Text = "Nombre";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(281, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 29);
            this.label1.TabIndex = 35;
            this.label1.Text = "Editar cliente";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(59, 453);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(46, 17);
            this.label10.TabIndex = 36;
            this.label10.Text = "Activo";
            // 
            // cbkActivo
            // 
            this.cbkActivo.AutoSize = true;
            this.cbkActivo.Location = new System.Drawing.Point(232, 453);
            this.cbkActivo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbkActivo.Name = "cbkActivo";
            this.cbkActivo.Size = new System.Drawing.Size(18, 17);
            this.cbkActivo.TabIndex = 37;
            this.cbkActivo.UseVisualStyleBackColor = true;
            this.cbkActivo.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clientesToolStripMenuItem,
            this.sucursalesToolStripMenuItem,
            this.rolesToolStripMenuItem,
            this.empresasToolStripMenuItem,
            this.facturasToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(740, 28);
            this.menuStrip1.TabIndex = 38;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // clientesToolStripMenuItem
            // 
            this.clientesToolStripMenuItem.Name = "clientesToolStripMenuItem";
            this.clientesToolStripMenuItem.Size = new System.Drawing.Size(73, 24);
            this.clientesToolStripMenuItem.Text = "Clientes";
            this.clientesToolStripMenuItem.Click += new System.EventHandler(this.clientesToolStripMenuItem_Click);
            // 
            // sucursalesToolStripMenuItem
            // 
            this.sucursalesToolStripMenuItem.Name = "sucursalesToolStripMenuItem";
            this.sucursalesToolStripMenuItem.Size = new System.Drawing.Size(89, 24);
            this.sucursalesToolStripMenuItem.Text = "Sucursales";
            this.sucursalesToolStripMenuItem.Click += new System.EventHandler(this.sucursalesToolStripMenuItem_Click);
            // 
            // rolesToolStripMenuItem
            // 
            this.rolesToolStripMenuItem.Name = "rolesToolStripMenuItem";
            this.rolesToolStripMenuItem.Size = new System.Drawing.Size(57, 24);
            this.rolesToolStripMenuItem.Text = "Roles";
            this.rolesToolStripMenuItem.Click += new System.EventHandler(this.rolesToolStripMenuItem_Click);
            // 
            // empresasToolStripMenuItem
            // 
            this.empresasToolStripMenuItem.Name = "empresasToolStripMenuItem";
            this.empresasToolStripMenuItem.Size = new System.Drawing.Size(84, 24);
            this.empresasToolStripMenuItem.Text = "Empresas";
            this.empresasToolStripMenuItem.Click += new System.EventHandler(this.empresasToolStripMenuItem_Click);
            // 
            // facturasToolStripMenuItem
            // 
            this.facturasToolStripMenuItem.Name = "facturasToolStripMenuItem";
            this.facturasToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.facturasToolStripMenuItem.Text = "Facturas";
            this.facturasToolStripMenuItem.Click += new System.EventHandler(this.facturasToolStripMenuItem_Click);
            // 
            // EditClienteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 562);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.cbkActivo);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFecha);
            this.Controls.Add(this.txtCodigoPostal);
            this.Controls.Add(this.txtDireccion);
            this.Controls.Add(this.txtTelefono);
            this.Controls.Add(this.txtMail);
            this.Controls.Add(this.txtDNI);
            this.Controls.Add(this.txtApellido);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "EditClienteForm";
            this.Text = "EditClienteForm";
            this.Load += new System.EventHandler(this.EditClienteForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker txtFecha;
        private System.Windows.Forms.TextBox txtCodigoPostal;
        private System.Windows.Forms.TextBox txtDireccion;
        private System.Windows.Forms.TextBox txtTelefono;
        private System.Windows.Forms.TextBox txtMail;
        private System.Windows.Forms.TextBox txtDNI;
        private System.Windows.Forms.TextBox txtApellido;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox cbkActivo;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clientesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sucursalesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rolesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem empresasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem facturasToolStripMenuItem;
    }
}