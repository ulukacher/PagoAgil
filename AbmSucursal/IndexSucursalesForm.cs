using PagoAgilFrba.AbmCliente;
using PagoAgilFrba.AbmEmpresa;
using PagoAgilFrba.AbmFactura;
using PagoAgilFrba.AbmSucursal;
using PagoAgilFrba.AbmRol;
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

namespace PagoAgilFrba.AbmSucursal
{
    public partial class IndexSucursalesForm : Form
    {
        public IndexSucursalesForm()
        {
            InitializeComponent();
            LlenarGrillaSucursales();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            //Filtros de busqueda
            try
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                var sucursales = SucursalesRepository.GetSucursalByNombreDireccionCodigoPostal(txtFiltroNombre.Text, txtFiltroDireccion.Text, txtFiltroCodPostal.Text);
                foreach (var item in sucursales)
                {
                    int index = dataGridView1.Rows.Add(item.Nombre, item.Direccion, item.CodigoPostal,  item.Activa);
                    dataGridView1.Rows[index].Cells[4].Value = "Editar";
                    dataGridView1.Rows[index].Cells[5].Value = "Eliminar";
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFiltroNombre.Text = "";
            txtFiltroDireccion.Text = "";
            txtFiltroCodPostal.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var createForm = new CreateSucursalForm();
            this.Hide();
            createForm.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Editar
            if (e.ColumnIndex == 4)
            {
                int codPostalAEditar = (int)dataGridView1.Rows[e.RowIndex].Cells[2].Value;
                Sucursal sucursal = SucursalesRepository.GetSucursalByCodigoPostal(codPostalAEditar);
                EditSucursalForm editForm = new EditSucursalForm(sucursal);
                this.Hide();
                editForm.Show();

            }
            //Eliminar
            if (e.ColumnIndex == 5)
            {
                int codPostalAEditar = (int)dataGridView1.Rows[e.RowIndex].Cells[2].Value;
                try
                {
                    SucursalesRepository.DarDeBajaSucursal(codPostalAEditar);
                    MessageBox.Show("La sucursal ha sido marcada como inactiva");
                    LlenarGrillaSucursales();
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Hubo un error al dar de baja a la sucursal");

                }
            }
        }

        private void LlenarGrillaSucursales()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            var sucursales = SucursalesRepository.GetAllSucursales();
            foreach (var item in sucursales)
            {
                int index = dataGridView1.Rows.Add(item.Nombre, item.Direccion, item.CodigoPostal, item.Activa);
                dataGridView1.Rows[index].Cells[4].Value = "Editar";
                dataGridView1.Rows[index].Cells[5].Value = "Eliminar";
            }
        }

        private void clientesToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var indexClientes = new IndexClientesForm();
            this.Hide();
            indexClientes.Show();
        }

        private void sucursalesToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var indexSucursales = new IndexSucursalesForm();
            this.Hide();
            indexSucursales.Show();
        }

        private void rolesToolStripMenuItem_Click(object sender, System.EventArgs e)
        {

        }

        private void empresasToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var indexEmpresas = new IndexEmpresasForm();
            this.Hide();
            indexEmpresas.Show();
        }

        private void facturasToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var indexFacturas = new IndexFacturasForm();
            this.Hide();
            indexFacturas.Show();
        }
    }
}
