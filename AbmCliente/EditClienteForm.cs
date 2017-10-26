using PagoAgilFrba.Classes;
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
    public partial class EditClienteForm : Form
    {
        public static string ConnectionString { get { return ConfigurationManager.ConnectionStrings["GDD"].ConnectionString; } }
        SqlConnection conn = new SqlConnection(ConnectionString);
        public EditClienteForm(Cliente _cliente)
        {
            this.cliente = _cliente;
            InitializeComponent();
            txtApellido.Text = cliente.Apellido;
            txtNombre.Text = cliente.Nombre;
            txtDNI.Text = cliente.DNI.ToString();
            txtDireccion.Text = cliente.Direccion;
            txtCodigoPostal.Text = cliente.CodigoPostal.ToString();
            txtMail.Text = cliente.Mail;
            txtTelefono.Text = cliente.Telefono;
            txtFecha.Value = cliente.FechaNacimiento;
            cbkActivo.Checked = cliente.Activo;
        }
        public EditClienteForm()
        {
            InitializeComponent();
        }
        public Cliente cliente { get; set; }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void EditClienteForm_Load(object sender, EventArgs e)
        {

        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
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
                     var query = "update dbo.clientes set clie_nombre=@nombre,clie_apellido = @apellido,clie_dni=@dni,"
                         + "clie_direccion=@direccion,clie_mail=@mail,clie_codigoPostal = @codigoPostal,"
                         + "clie_telefono=@telefono,clie_fechaNacimiento=@fechaNacimiento,clie_activo=@activo where clie_dni = @dniOriginal";
                     SqlCommand command = new SqlCommand(query, conn);
                     command.Parameters.AddWithValue("@nombre", txtNombre.Text);
                     command.Parameters.AddWithValue("@dniOriginal", cliente.DNI);

                     command.Parameters.AddWithValue("@apellido", txtApellido.Text);
                     command.Parameters.AddWithValue("@dni", txtDNI.Text);
                     command.Parameters.AddWithValue("@mail", txtMail.Text);
                     command.Parameters.AddWithValue("@telefono", txtTelefono.Text);
                     command.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                     command.Parameters.AddWithValue("@codigoPostal", txtCodigoPostal.Text);
                     command.Parameters.AddWithValue("@fechaNacimiento", txtFecha.Value);
                     command.Parameters.AddWithValue("@activo", cbkActivo.Checked);

                     command.ExecuteNonQuery();
                     MessageBox.Show("El cliente ha sido modificado correctamente");
                     conn.Close();
                     this.Hide();
                     var indexForm = new IndexClientesForm();
                     indexForm.Show();
                 }
                 catch (SqlException exc)
                 {
                     //Violacion de primary key
                     if (exc.Number == 2627)
                         MessageBox.Show("Ya existe un cliente con ese DNI");
                     conn.Close();
                 }
             }
             else
             {
                 string errorMessage = string.Join("\n", errores.ToArray());
                 MessageBox.Show(errorMessage);
             }
               
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
                var queryMailUnico = "select count(*) from dbo.Clientes where clie_mail = @mailIngresado and clie_dni <> @dniOriginal";
                SqlCommand command = new SqlCommand(queryMailUnico, conn);
                command.Parameters.AddWithValue("@mailIngresado", txtMail.Text);
                command.Parameters.AddWithValue("@dniOriginal", cliente.DNI);

                int cantMailsIguales = (int)command.ExecuteScalar();
                conn.Close();
                if (cantMailsIguales > 0)
                    errores.Add("Ya existe un cliente con ese mail");
            }
            //Valido el DNI
            if (txtDNI.Text == "" || !Regex.IsMatch(txtDNI.Text, Program.regexSoloNumeros))
                errores.Add("Ingrese un DNI valido");
            if (txtNombre.Text == "" || !Regex.IsMatch(txtNombre.Text, Program.regexSoloLetras))
                errores.Add("Ingrese un nombre válido");
            if (txtApellido.Text == "" || !Regex.IsMatch(txtApellido.Text, Program.regexSoloLetras))
                errores.Add("Ingrese un apellido válido");
            if (txtDireccion.Text == "")
                errores.Add("Ingrese una direccion valida");
            if (txtTelefono.Text == "" || !Regex.IsMatch(txtTelefono.Text, Program.regexSoloNumeros))
                errores.Add("Ingrese un teléfono valido");
            if (txtCodigoPostal.Text == "" || !Regex.IsMatch(txtCodigoPostal.Text, Program.regexSoloNumeros))
                errores.Add("Ingrese un codigo postal valido");
            if (txtFecha.Text == "" || txtFecha.Value >= DateTime.Now)
                errores.Add("Ingrese una fecha de nacimiento válida");
            return errores;
        }
    }
}
