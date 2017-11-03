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
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;

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
                    Cliente cliente = new Cliente();
                    cliente.Nombre = txtNombre.Text;
                    cliente.Apellido = txtApellido.Text;
                    cliente.DNI = decimal.Parse(txtDNI.Text);
                    cliente.FechaNacimiento = txtFecha.Value;
                    cliente.Direccion = txtDireccion.Text;
                    cliente.CodigoPostal = txtCodigoPostal.Text;
                    cliente.Telefono = txtTelefono.Text;
                    cliente.Mail = txtMail.Text;
                    ClientesRepository.AgregarCliente(cliente);		     
                    MessageBox.Show("El cliente ha sido agregado correctamente");
                    this.Hide();
                    var indexForm = new IndexClientesForm();
                    indexForm.Show();
	            }
	            catch (SqlException sqlexc)
	            {
				   //Violacion de primary key
                   if(sqlexc.Number == 2627)
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
        private void validarCamposNoUnicos(List<string> errores) 
        {
            //Valido el DNI
            if (txtDNI.Text == "" || !Regex.IsMatch(txtDNI.Text, regexSoloNumeros))
                errores.Add("Ingrese un DNI valido");      
            if (txtNombre.Text == "")
                errores.Add("Ingrese un nombre válido");
            if (txtApellido.Text == "")
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
        private bool ValidarMail(string mail) {
            try
            {
                MailAddress mailAdress = new MailAddress(txtMail.Text);
                return true;
            }
            catch (Exception e) {
                return false;
            }
        }
        private List<string> validarCamposCreateOrEdit()
        {
            List<string> errores = new List<string>();
                  

            //Valido el mail
            if (txtMail.Text == "" || !this.ValidarMail(txtMail.Text))
                errores.Add("Ingrese un email valido");
            else
            {
                int cantMailsIguales = ClientesRepository.GetCantidadClientesConEseMail(txtMail.Text);
                if (cantMailsIguales > 0)
                    errores.Add("Ya existe un cliente con ese mail");
            }
            this.validarCamposNoUnicos(errores);
            return errores;
        }

        private void CreateClienteForm_Load(object sender, EventArgs e)
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
