using PagoAgilFrba.Classes;
using PagoAgilFrba.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PagoAgilFrba.Rendicion
{
    public partial class RendicionForm : Form
    {
        public RendicionForm()
        {
            InitializeComponent();
            CargarCombos();
        }

        private void CargarCombos()
        {
            var empresas = EmpresasRepository.GetAllEmpresas();
            foreach (var item in empresas)
            {
                ComboBoxItemStringValue cbItem = new ComboBoxItemStringValue();
                cbItem.Text = item.Nombre;
                cbItem.Value = item.Cuit;
                comboEmpresas.Items.Add(cbItem);
            }
            comboEmpresas.SelectedIndex = 0;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void RendicionForm_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            List<Factura> facturas = FacturasRepository.GetFacturasPendientesDeRendicionByEmpresa(((ComboBoxItemStringValue)comboEmpresas.SelectedItem).Value);
            foreach (var item in facturas)
            {
                dataGridView1.Rows.Add(item.Nro, item.ClienteDNI, item.EmpresaCuit, item.Fecha, item.FechaVencimiento,item.Monto);
            }
            lblTotalEnFacturas.Text = "$" + facturas.Sum(x => x.Monto);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
