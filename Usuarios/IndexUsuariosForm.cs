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

namespace PagoAgilFrba.Usuarios
{
    public partial class IndexUsuariosForm : Form
    {
        public IndexUsuariosForm()
        {
            InitializeComponent();
            LlenarGrilla();
        }

        private void LlenarGrilla()
        {
            List<Usuario> usuarios = UsuariosRepository.GetAll();
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            int i=0;
            foreach (var item in usuarios)
            {
                dataGridView1.Rows.Add(item.Username, item.Activo);
                dataGridView1[2, i].Value = item.Activo ? "Inhabilitar" : "Habilitar";
                i++;

            }
        }

        private void IndexUsuariosForm_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Cambiar habilitacion
            if (e.ColumnIndex == 2)
            {
                string username = (string)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                UsuariosRepository.CambiarHabilitacion(username);
                MessageBox.Show("El usuario ha sido modificado");
                LlenarGrilla();

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
