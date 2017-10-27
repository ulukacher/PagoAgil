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
using PagoAgilFrba.Classes;
using PagoAgilFrba.Repositories;

namespace PagoAgilFrba.AbmCliente
{
    public partial class IndexClientesForm : Form
    {
        SqlConnection conn;
        string connectionString = ConfigurationManager.ConnectionStrings["GDD"].ConnectionString;

     

        public IndexClientesForm()
        {

            InitializeComponent();     
            this.LlenarGrillaClientes();
        }
        private void LlenarGrillaClientes()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            var clientes = ClientesRepository.GetAllClientes();
            foreach (var item in clientes)
	        {
                int index = dataGridView1.Rows.Add(item.Nombre, item.Apellido, item.DNI, item.FechaNacimiento, item.Mail, item.Direccion, item.CodigoPostal,item.Telefono, item.Activo);
                dataGridView1.Rows[index].Cells[9].Value = "Editar";
                dataGridView1.Rows[index].Cells[10].Value = "Eliminar";
	        }       
                  
                
                     
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Editar
            if (e.ColumnIndex == 9)
            {
                decimal dniAEditar = (decimal)dataGridView1.Rows[e.RowIndex].Cells[2].Value;
                Cliente cliente = ClientesRepository.GetClienteByDNI(dniAEditar);
                EditClienteForm editForm = new EditClienteForm(cliente);
                this.Hide();
                editForm.Show();   

            }
            //Eliminar
            if (e.ColumnIndex == 10)
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

        private void button1_Click(object sender, EventArgs e)
        {
            var createForm = new CreateClienteForm();
            this.Hide();
            createForm.Show();
        }

        private void IndexClientesForm_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, System.EventArgs e)
        {

        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            //Filtros de busqueda
            try
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                var clientes = ClientesRepository.GetClientesByNombreApellidoDni(txtFiltroNombre.Text,txtFiltroApellido.Text,txtFiltroDni.Text);
                foreach (var item in clientes)
                {
                    int index = dataGridView1.Rows.Add(item.Nombre, item.Apellido, item.DNI, item.FechaNacimiento, item.Mail, item.Direccion, item.CodigoPostal, item.Telefono, item.Activo);
                    dataGridView1.Rows[index].Cells[9].Value = "Editar";
                    dataGridView1.Rows[index].Cells[10].Value = "Eliminar";
                } 
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            
        }

        private void txtFiltroNombre_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void btnLimpiar_Click(object sender, System.EventArgs e)
        {
            txtFiltroApellido.Text = "";
            txtFiltroNombre.Text = "";
            txtFiltroDni.Text = "";

        }
    }
}
