using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PagoAgilFrba.AbmCliente
{
    public partial class CreateClienteForm : Form
    {
        const string regexSoloLetras = "^[a-zA-Z]+$";        
        const string regexSoloNumeros = @"^\d+$";
        public static string ConnectionString { get { return  ConfigurationManager.ConnectionStrings["GDD"].ConnectionString;} }
        SqlConnection conn = new SqlConnection(ConnectionString);

        public CreateClienteForm()
        {
            InitializeComponent();
            //DateTimePicker datePicker = new DateTimePicker();
            //datePicker.ShowUpDown = true;
            //datePicker.CustomFormat = "dd/MM/yyyy hh:mm";
            //datePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            //textBox8.Controls.Add(datePicker);
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Valido que esten todos los datos y que el tipo de dato sea correcto
            var errores = this.validarCamposCreateOrEdit();
            if (errores.Count == 0)
            { 
                try 
	            {	        
		            conn.Open();
                    string query = "insert into dbo.Clientes values (@dni,@nombre,@apellido,@fechaNacimiento,@mail,@direccion,@codigoPostal,@telefono,1)";
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@nombre", txtNombre.Text);
                    command.Parameters.AddWithValue("@apellido", txtApellido.Text);
                    command.Parameters.AddWithValue("@dni", txtDNI.Text);
                    command.Parameters.AddWithValue("@mail", txtMail.Text);
                    command.Parameters.AddWithValue("@telefono", txtTelefono.Text);
                    command.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                    command.Parameters.AddWithValue("@codigoPostal", txtCodigoPostal.Text);
                    command.Parameters.AddWithValue("@fechaNacimiento", txtFecha.Value);

                    command.ExecuteNonQuery();
                    MessageBox.Show("El cliente ha sido agregado correctamente");
                    conn.Close();
                    this.Hide();
                    var indexForm = new IndexClientesForm();
                    indexForm.Show();
	            }
	            catch (SqlException sqlexc)
	            {
				   //Violacion de primary key
                   if(sqlexc.Number == 2627)
                       MessageBox.Show("Ya existe un cliente con ese DNI");
	            }
               
            }
            else
            {
                string errorMessage = string.Join("\n", errores.ToArray());
                MessageBox.Show(errorMessage);
            }

        }
        private void validarCamposNoUnicos(List<string> errores) 
        {
            //Valido el DNI
            if (txtDNI.Text == "" || !Regex.IsMatch(txtDNI.Text, regexSoloNumeros))
                errores.Add("Ingrese un DNI valido");      
            if (txtNombre.Text == "" || !Regex.IsMatch(txtNombre.Text, regexSoloLetras))
                errores.Add("Ingrese un nombre válido");
            if (txtApellido.Text == "" || !Regex.IsMatch(txtApellido.Text, regexSoloLetras))
                errores.Add("Ingrese un apellido válido");
            if (txtDireccion.Text == "")
                errores.Add("Ingrese una direccion valida");
            if (txtTelefono.Text == "" || !Regex.IsMatch(txtTelefono.Text, regexSoloNumeros))
                errores.Add("Ingrese un teléfono valido");
            if (txtCodigoPostal.Text == "" || !Regex.IsMatch(txtCodigoPostal.Text, regexSoloNumeros))
                errores.Add("Ingrese un codigo postal valido");
            if (txtFecha.Text == "" || txtFecha.Value >= DateTime.Now)
                errores.Add("Ingrese una fecha de nacimiento válida");
        }
        private List<string> validarCamposCreateOrEdit()
        {
            List<string> errores = new List<string>();
                  

            //Valido el mail
            if (txtMail.Text == "" || !txtMail.Text.Contains("@"))
                errores.Add("Ingrese un email valido");
            else
            {
                conn.Open();
                var queryMailUnico = "select count(*) from dbo.Clientes where clie_mail = @mailIngresado";
                SqlCommand command = new SqlCommand(queryMailUnico, conn);
                command.Parameters.AddWithValue("@mailIngresado", txtMail.Text);
                int cantMailsIguales = (int)command.ExecuteScalar();
                conn.Close();
                if (cantMailsIguales > 0)
                    errores.Add("Ya existe un cliente con ese mail");
            }
            this.validarCamposNoUnicos(errores);
            return errores;
        }

        private void CreateClienteForm_Load(object sender, EventArgs e)
        {

        }
    }
}
