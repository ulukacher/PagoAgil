using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;
using PagoAgilFrba.AbmCliente;
using PagoAgilFrba.AbmEmpresa;
using PagoAgilFrba.AbmFactura;
using PagoAgilFrba.AbmSucursal;
using PagoAgilFrba.AbmRol;
using PagoAgilFrba.Classes;
using System.Text.RegularExpressions;
using PagoAgilFrba.Repositories;

namespace PagoAgilFrba.AbmEmpresa
{
    public partial class IndexEmpresasForm : Form
    {
        public IndexEmpresasForm()
        {
            InitializeComponent();
            this.LlenarGrillaEmpresas();
            CargarCombo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var createForm = new CreateEmpresaForm();
            this.Hide();
            createForm.Show();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFiltroNombre.Text = "";
            txtFiltroCuit.Text = "";
            cboFiltroRubro.SelectedIndex = 0;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            //Filtros de busqueda
            try
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                var empresas = EmpresasRepository.GetEmpresasByNombreCuitRubro(txtFiltroNombre.Text, txtFiltroCuit.Text, ((ComboboxItem)cboFiltroRubro.SelectedItem).Value);
                foreach (var item in empresas)
                {
                    int index = dataGridView1.Rows.Add(item.Cuit, item.Nombre, item.Direccion, item.Rubro, item.Activa);
                    dataGridView1.Rows[index].Cells[5].Value = "Editar";
                    dataGridView1.Rows[index].Cells[6].Value = "Eliminar";
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void LlenarGrillaEmpresas()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            var empresas = EmpresasRepository.GetAllEmpresas();
            foreach (var item in empresas)
            {
                int index = dataGridView1.Rows.Add(item.Cuit, item.Nombre, item.Direccion, item.Rubro, item.Activa);
                dataGridView1.Rows[index].Cells[5].Value = "Editar";
                dataGridView1.Rows[index].Cells[6].Value = "Eliminar";
            }       
        }

        private void CargarCombo()
        {

            ComboboxItem itemInicial = new ComboboxItem();
            itemInicial.Value = 0;
            itemInicial.Text = "Todos";
            cboFiltroRubro.Items.Add(itemInicial);
            var rubros = RubrosRepository.GetAllRubros();
            foreach (var item in rubros)
            {
                ComboboxItem cbItem = new ComboboxItem();
                cbItem.Text = item.rubr_descripcion;
                cbItem.Value = item.rubr_id;
                cboFiltroRubro.Items.Add(cbItem);
            }    
            cboFiltroRubro.SelectedIndex = 0;
        }

        private void IndexEmpresasForm_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Editar
            if (e.ColumnIndex == 5)
            {
                string cuitAEditar = (string)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                Empresa empresa = EmpresasRepository.GetEmpresaByCUIT(cuitAEditar);
                EditEmpresaForm editForm = new EditEmpresaForm(empresa);
                this.Hide();
                editForm.Show();

            }
            //Eliminar
            if (e.ColumnIndex == 6)
            {
                string cuitAInhabilitar = (string)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                if (EmpresasRepository.PuedeDarDeBaja(cuitAInhabilitar))
                {
                    try
                    {
                        EmpresasRepository.DarDeBajaEmpresa(cuitAInhabilitar);
                        MessageBox.Show("La empresa ha sido marcada como inactiva");
                        LlenarGrillaEmpresas();
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("Hubo un error al dar de baja a la empresa");

                    }
                }
                else
                    MessageBox.Show("La empresa no puede ser dada de baja debido a que no todas sus facturas fueron rendidas.");
            }
        }

        private void cboFiltroRubro_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

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
            var indexRoles = new IndexRolesForm();
            this.Hide();
            indexRoles.Show();
        }

        private void empresasToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var indexEmpresas = new IndexEmpresasForm();
            this.Hide();
            indexEmpresas.Show();
        }

        private void facturasToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var indexFacturas = new IndexFacturasForm();
            this.Hide();
            indexFacturas.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var index = new IndexForm();
            this.Hide();
            index.Show();
        }
    }
}
