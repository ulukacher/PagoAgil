using PagoAgilFrba.AbmCliente;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PagoAgilFrba
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var viewIndexClientes = new IndexClientesForm();
            this.Hide();
            viewIndexClientes.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var indexForm = new IndexForm();
            this.Hide();
            indexForm.Show();
        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            
        }
    }
}
