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

namespace PagoAgilFrba.AbmCliente
{
    public partial class IndexClientesForm : Form
    {
        SqlConnection conn;
        string connectionString = ConfigurationManager.ConnectionStrings["GDD"].ConnectionString;

     

        public IndexClientesForm()
        {

            InitializeComponent();
            conn = new SqlConnection(connectionString);
            conn.Open();
            string query = "Select * from dbo.Clientes";
            SqlCommand command = new SqlCommand(query, conn);
            this.LlenarGrillaClientes(command);
            conn.Close();
        }
        private void LlenarGrillaClientes(SqlCommand command)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {

                    int index = dataGridView1.Rows.Add(reader["clie_nombre"], reader["clie_apellido"], reader["clie_dni"], reader["clie_fechaNacimiento"], reader["clie_mail"], reader["clie_direccion"], reader["clie_codigoPostal"], reader["clie_telefono"], reader["clie_activo"]);
                    dataGridView1.Rows[index].Cells[9].Value = "Editar";
                    dataGridView1.Rows[index].Cells[10].Value = "Eliminar";
                }

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
                conn.Open();
                decimal dniAEditar = (decimal)dataGridView1.Rows[e.RowIndex].Cells[2].Value;
                string query = "select * from dbo.clientes where clie_dni = @dni";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@dni", dniAEditar);
                SqlDataReader reader =  command.ExecuteReader();
                if (reader.Read()) 
                {
                    Cliente cliente = new Cliente();
                    cliente.Nombre = (string)reader["clie_nombre"];
                    cliente.Apellido = (string)reader["clie_apellido"];
                    cliente.DNI = (decimal)reader["clie_dni"];
                    cliente.Direccion = (string)reader["clie_direccion"];
                    cliente.Mail = (string)reader["clie_mail"];
                    cliente.FechaNacimiento = (DateTime)reader["clie_fechaNacimiento"];
                    cliente.CodigoPostal = (string)reader["clie_codigoPostal"];
                    cliente.Telefono = (string)reader["clie_telefono"];

                    cliente.Activo = (bool)reader["clie_activo"];
                    EditClienteForm editForm = new EditClienteForm(cliente);
                    this.Hide();
                    editForm.Show();
                }

            }
            //Eliminar
            if (e.ColumnIndex == 10)
            {
                decimal dniAInhabilitar = (decimal)dataGridView1.Rows[e.RowIndex].Cells[2].Value;
                conn.Open();
                string query = "update dbo.clientes set clie_activo = 0 where clie_dni = @dni";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@dni", dniAInhabilitar);
                command.ExecuteNonQuery();
                MessageBox.Show("El cliente ha sido marcado como inactivo");
                conn.Close();
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
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = new SqlCommand();
                string query = "Select * from dbo.Clientes where clie_nombre like @nombre and clie_apellido like @apellido";
                SqlParameter nombre = new SqlParameter("@nombre", DbType.String);
                nombre.Value = "%" + txtFiltroNombre.Text + "%";
                command.Parameters.Add(nombre);

                SqlParameter apellido = new SqlParameter("@apellido", DbType.String);
                apellido.Value = "%" + txtFiltroApellido.Text + "%";
                command.Parameters.Add(apellido);

                if (txtFiltroDni.Text != "")
                {
                    query += " and clie_dni = @dni";
                    SqlParameter dniParameter = new SqlParameter("@dni", DbType.Int32);
                    int dni;
                    if (!int.TryParse(txtFiltroDni.Text,out dni))
                        throw new Exception("El DNI a buscar debe ser un numero, sin puntos ni comas");
                    dniParameter.Value = dni;
                    command.Parameters.Add(dniParameter);
                }
                command.Connection = conn;
                command.CommandText = query;
                this.LlenarGrillaClientes(command);
                conn.Close();
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
