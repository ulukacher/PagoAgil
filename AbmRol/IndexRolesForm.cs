using PagoAgilFrba.AbmCliente;
using PagoAgilFrba.AbmEmpresa;
using PagoAgilFrba.AbmFactura;
using PagoAgilFrba.AbmSucursal;
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

namespace PagoAgilFrba.AbmRol
{
    public partial class IndexRolesForm : Form
    {
        public IndexRolesForm()
        {
            InitializeComponent();
            try
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                var roles = RolesRepository.GetAllRoles();
                foreach (var item in roles)
                {
                    int index = dataGridView1.Rows.Add(item.Nombre, item.Activo);
                    dataGridView1.Rows[index].Cells[3].Value = "Editar";
                    dataGridView1.Rows[index].Cells[4].Value = "Eliminar";
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
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

        private void button1_Click(object sender, EventArgs e)
        {
            var _CrearRolForm = new CrearRolForm();
            this.Hide();
            _CrearRolForm.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Editar
            if (e.ColumnIndex == 3)
            {
                decimal idAEditar = (decimal)dataGridView1.Rows[e.RowIndex].Cells[2].Value;

                var editForm = new Rol() {  };
                this.Hide();
                editForm.Show();

            }
            //Eliminar
            if (e.ColumnIndex == 4)
            {
                decimal dniAInhabilitar = (decimal)dataGridView1.Rows[e.RowIndex].Cells[2].Value;
                try
                {
                    ClientesRepository.DarDeBajaCliente(dniAInhabilitar);
                    MessageBox.Show("El cliente ha sido marcado como inactivo");
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Hubo un error al dar de baja al cliente");

                }
            }
        }

    }
}
