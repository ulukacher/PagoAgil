using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;
using PagoAgilFrba.AbmCliente;
using PagoAgilFrba.AbmEmpresa;
using PagoAgilFrba.AbmFactura;
using PagoAgilFrba.AbmSucursal;
using PagoAgilFrba.AbmRol;
using PagoAgilFrba.Classes;
using PagoAgilFrba.Repositories;

namespace PagoAgilFrba.AbmFactura
{
    public partial class IndexFacturasForm : Form
    {
        public IndexFacturasForm()
        {
            InitializeComponent();
            CargarCombo();

            facturasToolStripMenuItem.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var createForm = new CreateFacturaForm();
            this.Hide();
            createForm.Show();
        }

        private void CargarCombo()
        {
            ComboBoxItemStringValue itemEmpresa0 = new ComboBoxItemStringValue();
            itemEmpresa0.Value = "";
            itemEmpresa0.Text = "Todas";
            cboFiltroEmpresa.Items.Add(itemEmpresa0);

            var empresas = EmpresasRepository.GetAllEmpresas();

            foreach (var item in empresas)
            {
                ComboBoxItemStringValue cbItem = new ComboBoxItemStringValue();

                cbItem.Text = item.Nombre;
                cbItem.Value = item.Cuit;

                cboFiltroEmpresa.Items.Add(cbItem);
            }

            cboFiltroEmpresa.SelectedIndex = 0;

            ComboboxItem itemEstado0 = new ComboboxItem();
            itemEstado0.Value = -1;
            itemEstado0.Text = "Todos";
            cboFiltroEstado.Items.Add(itemEstado0);

            ComboboxItem itemEstado1 = new ComboboxItem();
            itemEstado1.Value = 0;
            itemEstado1.Text = "Pendiente de Pago";
            cboFiltroEstado.Items.Add(itemEstado1);

            ComboboxItem itemEstado2 = new ComboboxItem();
            itemEstado2.Value = 1;
            itemEstado2.Text = "Pagada";
            cboFiltroEstado.Items.Add(itemEstado2);

            ComboboxItem itemEstado3 = new ComboboxItem();
            itemEstado3.Value = 2;
            itemEstado3.Text = "Rendida";
            cboFiltroEstado.Items.Add(itemEstado3);

            cboFiltroEstado.SelectedIndex = 0;
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            //Editar
            if (e.ColumnIndex == 6)
            {
                decimal nroFacturaAEditar = (decimal)dataGridView1.Rows[e.RowIndex].Cells[0].Value;

                Factura factura = FacturasRepository.GetFacturaByNro(nroFacturaAEditar);

                if (factura.Estado == 0)
                {
                    EditFacturaForm editForm = new EditFacturaForm(factura);
                    this.Hide();
                    editForm.Show();
                }
                else
                {
                    MessageBox.Show("No se puede editar una factura que ya fue pagada o rendida");
                }
            }
            //Eliminar
            if (e.ColumnIndex == 7)
            {
                decimal nroFacturaAEliminar = (decimal)dataGridView1.Rows[e.RowIndex].Cells[0].Value;

                Factura factura = FacturasRepository.GetFacturaByNro(nroFacturaAEliminar);

                if (factura.Estado == 0)
                {
                    try
                    {
                        FacturasRepository.EliminarFactura(nroFacturaAEliminar);

                        MessageBox.Show("La factura fue eliminada");

                        dataGridView1.Refresh();
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("Hubo un error al eliminar la factura");

                    }
                }
                else
                {
                    MessageBox.Show("No se puede eliminar una factura que ya fue pagada o rendida");
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFiltroNumero.Text = "";
            txtFiltroCliente.Text = "";
            cboFiltroEmpresa.SelectedIndex = 0;
            cboFiltroEstado.SelectedIndex = 0;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            //Filtros de busqueda
            try
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();

                if (txtFiltroNumero.Text != "")
                {
                    int nro;

                    if (!int.TryParse(txtFiltroNumero.Text, out nro))
                    {
                        txtFiltroNumero.Text = "";

                        txtFiltroNumero.Focus();

                        throw new Exception("El numero de factura a buscar no debe contener letras");
                    }
                }

                if (txtFiltroCliente.Text != "")
                {
                    int dni;

                    if (!int.TryParse(txtFiltroCliente.Text, out dni))
                        throw new Exception("El DNI a buscar debe ser un numero, sin puntos ni comas");
                }

                var facturas = FacturasRepository.GetFacturasByFiltros(txtFiltroNumero.Text, ((ComboBoxItemStringValue)cboFiltroEmpresa.SelectedItem).Value, txtFiltroCliente.Text, ((ComboboxItem)cboFiltroEstado.SelectedItem).Value);

                foreach (var item in facturas)
                {
                    int index;
                    
                    switch(item.Estado)
                    {
                        case 0:
                            index = dataGridView1.Rows.Add(item.Nro, item.ClienteDNI, item.EmpresaCuit, item.Fecha, item.FechaVencimiento, "Pendiente de Pago");
                        break;

                        case 1:
                            index = dataGridView1.Rows.Add(item.Nro, item.ClienteDNI, item.EmpresaCuit, item.Fecha, item.FechaVencimiento, "Pagada");
                        break;

                        case 2:
                            index = dataGridView1.Rows.Add(item.Nro, item.ClienteDNI, item.EmpresaCuit, item.Fecha, item.FechaVencimiento, "Rendida");
                        break;

                        default:
                            index = dataGridView1.Rows.Add(item.Nro, item.ClienteDNI, item.EmpresaCuit, item.Fecha, item.FechaVencimiento, "Pendiente de Pago");
                        break;
                    }
                    
                    dataGridView1.Rows[index].Cells[6].Value = "Editar";
                    dataGridView1.Rows[index].Cells[7].Value = "Eliminar";
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void clientesToolStripMenuItem_Click_1(object sender, System.EventArgs e)
        {
            var indexClientes = new IndexClientesForm();
            this.Hide();
            indexClientes.Show();
        }

        private void sucursalesToolStripMenuItem_Click_1(object sender, System.EventArgs e)
        {
            var indexSucursales = new IndexSucursalesForm();
            this.Hide();
            indexSucursales.Show();
        }

        private void rolesToolStripMenuItem_Click_1(object sender, System.EventArgs e)
        {
            var indexRoles = new IndexRolesForm();
            this.Hide();
            indexRoles.Show();
        }

        private void empresasToolStripMenuItem_Click_1(object sender, System.EventArgs e)
        {
            var indexEmpresas = new IndexEmpresasForm();
            this.Hide();
            indexEmpresas.Show();
        }

        private void facturasToolStripMenuItem_Click_1(object sender, System.EventArgs e)
        {
            var indexFacturas = new IndexFacturasForm();
            this.Hide();
            indexFacturas.Show();
        }

        private void IndexFacturasForm_Load(object sender, System.EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, System.EventArgs e)
        {

        }
    }
}