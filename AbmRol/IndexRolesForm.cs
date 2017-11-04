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
        public List<Rol> Roles;
        public IndexRolesForm()
        {
            InitializeComponent();
            try
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                Roles = RolesRepository.GetAllRoles();
                foreach (var item in Roles)
                {
                    int index = dataGridView1.Rows.Add(item.Id, item.Nombre, item.Activo);
                    dataGridView1.Rows[index].Cells[3].Value = "Editar";
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
            var id = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            try
            {
                //Editar
                if (e.ColumnIndex == 3)
                {
                    var rol = Roles.Find(r => r.Id == id);
                    var editForm = new CrearRolForm(rol);
                    this.Hide();
                    editForm.Show();
                }

                if (e.ColumnIndex == 2)
                {

                    var activo = (bool)dataGridView1.Rows[e.RowIndex].Cells[2].Value;
                    if (activo) //deshabilitar
                    {
                        var res = MessageBox.Show("¿Esta seguro de que desea deshabilitar este rol?", "", MessageBoxButtons.OKCancel);
                        if (res == System.Windows.Forms.DialogResult.OK)
                        {
                            RolesRepository.DesHabilitarRol(id);
                            MessageBox.Show("El rol ha sido marcado como inactivo");
                            dataGridView1.Rows[e.RowIndex].Cells[2].Value = false;
                            /*
                             if(rolActual.Id == id)
                              {
                                logOut();
                              }
                            */
                        }
                    }
                    else //habilitar
                    {
                        RolesRepository.HabilitarRol(id);
                        MessageBox.Show("El rol ha sido marcado como activo");
                        dataGridView1.Rows[e.RowIndex].Cells[2].Value = true;
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Hubo un error. Detalles: " + exc.Message);
            }
        }

    }
}
