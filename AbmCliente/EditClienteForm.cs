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

            txtFecha.Value = ConfiguracionFecha.FechaSistema;
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
                     Cliente clienteEditar = new Cliente();
                     clienteEditar.Nombre = txtNombre.Text;
                     clienteEditar.Apellido = txtApellido.Text;
                     clienteEditar.DNI = decimal.Parse(txtDNI.Text);
                     clienteEditar.FechaNacimiento = txtFecha.Value;
                     clienteEditar.Direccion = txtDireccion.Text;
                     clienteEditar.CodigoPostal = txtCodigoPostal.Text;
                     clienteEditar.Telefono = txtTelefono.Text;
                     clienteEditar.Mail = txtMail.Text;
                     clienteEditar.Activo = cbkActivo.Checked;

                     ClientesRepository.EditarCliente(clienteEditar, cliente.DNI);                   
                     MessageBox.Show("El cliente ha sido modificado correctamente");
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
        private bool ValidarMail(string mail)
        {
            try
            {
                MailAddress mailAdress = new MailAddress(txtMail.Text);
                return true;
            }
            catch (Exception e)
            {
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
                int cantMailsIguales = ClientesRepository.GetCantClientesDistintosConEseMail(txtMail.Text, cliente.DNI);    
                if (cantMailsIguales > 0)
                    errores.Add("Ya existe un cliente con ese mail");
            }
            //Valido el DNI
            if (txtDNI.Text == "" || !Regex.IsMatch(txtDNI.Text, Program.regexSoloNumeros))
                errores.Add("Ingrese un DNI valido");
            if (txtNombre.Text == "")
                errores.Add("Ingrese un nombre válido");
            if (txtApellido.Text == "" )
                errores.Add("Ingrese un apellido válido");
            if (txtDireccion.Text == "")
                errores.Add("Ingrese una direccion valida");
            if (txtTelefono.Text == "" || !Regex.IsMatch(txtTelefono.Text, Program.regexSoloNumeros))
                errores.Add("Ingrese un teléfono valido");
            if (txtCodigoPostal.Text == "" || !Regex.IsMatch(txtCodigoPostal.Text, Program.regexSoloNumeros))
                errores.Add("Ingrese un codigo postal valido");
            if (txtFecha.Text == "" || txtFecha.Value >= ConfiguracionFecha.FechaSistema)
                errores.Add("Ingrese una fecha de nacimiento válida");
            return errores;
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

        private void button4_Click(object sender, EventArgs e)
        {
            var index = new IndexClientesForm();
            this.Hide();
            index.Show();
        }
    }
}
