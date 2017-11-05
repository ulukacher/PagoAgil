using PagoAgilFrba.AbmFactura;
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

namespace PagoAgilFrba.Rendiciones
{
    public partial class RendicionForm : Form
    {
        List<Factura> facturas;
        public RendicionForm()
        {
            InitializeComponent();
            CargarCombos();
        }

        private void CargarCombos()
        {
            var empresas = EmpresasRepository.GetEmpresasActivas();
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
            facturas = FacturasRepository.GetFacturasPendientesDeRendicionByEmpresa(((ComboBoxItemStringValue)comboEmpresas.SelectedItem).Value);
            foreach (var item in facturas)
            {
                dataGridView1.Rows.Add(item.Nro, item.ClienteDNI, item.EmpresaCuit, item.Fecha, item.FechaVencimiento,item.Monto);
            }
            lblTotalEnFacturas.Text = "$" + facturas.Sum(x => x.Monto);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime fechaRendicion = DateTime.Now;
            //Valido si ya hubo una rendicion en este periodo para esta empresa
            string empresaCuit = ((ComboBoxItemStringValue)comboEmpresas.SelectedItem).Value;
            decimal porcentajeComision;
            if (this.facturas.Count > 0)
            {
                if (decimal.TryParse(txtPorcentaje.Text, out porcentajeComision) && porcentajeComision >= 0 && porcentajeComision <= 100)
                {
                    bool yaHuboRendicion = RendicionesRepository.ExisteRendicionParaEmpresaYPeriodo(empresaCuit, fechaRendicion);
                    if (!yaHuboRendicion)
                    {
                        Rendicion rend = new Rendicion(empresaCuit, facturas, fechaRendicion, porcentajeComision);
                        try
                        {
                            RendicionesRepository.AgregarRendicion(rend);
                            MessageBox.Show("Se ha realizado la rendición correctamente");
                            this.Hide();
                            var indexFacturasForm = new IndexFacturasForm();
                            indexFacturasForm.Show();
                        }
                        catch (Exception exce)
                        {
                            MessageBox.Show("Hubo un error al realizar la rendicion. Intente nuevamente");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ya se realizó una rendición para esta empresa en este periodo");
                    }
                }
                else
                {
                    MessageBox.Show("Ingrese un valor valido para el porcentaje");
                }

            }
            else
            {
                MessageBox.Show("No se puede realizar una rendicion sin facturas asociadas");
            }
            
         
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var index = new IndexForm();
            this.Hide();
            index.Show();
        }
    }
}
