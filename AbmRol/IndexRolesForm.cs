using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PagoAgilFrba.Repositories;

namespace PagoAgilFrba.AbmRol
{
    public partial class IndexRolesForm : Form
    {
        public IndexRolesForm()
        {
            InitializeComponent();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFiltroId.Clear();
            txtFiltroNombre.Clear();
            cboFiltroActivo.SelectedIndex = 0;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            //Filtros de busqueda
            try
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                var roles = RolesRepository.GetEmpresasByNombreCuitRubro(txtFiltroNombre.Text, txtFiltroCuit.Text, ((ComboboxItem)cboFiltroRubro.SelectedItem).Value);
                foreach (var item in roles)
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

    }
}
