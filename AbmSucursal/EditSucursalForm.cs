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
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PagoAgilFrba.AbmSucursal
{
    public partial class EditSucursalForm : Form
    {
        const string regexSoloNumeros = @"^\d+$";
        const string regexLetrasEspaciosONumeros = @"^[a-zA-Z\s\d]+$";

        public EditSucursalForm(Sucursal _sucursal)
        {
            this.sucursal = _sucursal;
            InitializeComponent();

            txtNombre.Text = sucursal.Nombre;
            txtDireccion.Text = sucursal.Direccion;
            txtCodigoPostal.Text = sucursal.CodigoPostal.ToString();
            chkActiva.Checked= sucursal.Activa;
        }
        public EditSucursalForm()
        {
            InitializeComponent();
        }

        public Sucursal sucursal{ get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            //Valido que esten todos los datos y que el tipo de dato sea correcto
            var errores = this.validarCamposCreateOrEdit();
            if (errores.Count == 0)
            {
                try
                {
                    Sucursal sucursalAEditar= new Sucursal();
                    sucursalAEditar.Nombre = txtNombre.Text;
                    sucursalAEditar.Direccion = txtDireccion.Text;
                    sucursalAEditar.CodigoPostal = int.Parse(txtCodigoPostal.Text);
                    sucursalAEditar.Activa = chkActiva.Checked;

                    SucursalesRepository.EditarSucursal(sucursalAEditar, sucursal.CodigoPostal);
                    MessageBox.Show("La sucursal ha sido modificada correctamente");
                    this.Hide();
                    var indexForm = new IndexSucursalesForm();
                    indexForm.Show();
                }
                catch (SqlException exc)
                {
                    //Violacion de primary key
                    if (exc.Number == 2627)
                        MessageBox.Show("Ya existe una sucursal con ese código postal");
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
            //Valido el Codigo Postal
            if (txtCodigoPostal.Text == "" || !Regex.IsMatch(txtCodigoPostal.Text, regexSoloNumeros))
                errores.Add("Ingrese un código postal válido");
            if (txtNombre.Text == "" || !Regex.IsMatch(txtNombre.Text, regexLetrasEspaciosONumeros))
                errores.Add("Ingrese un nombre válido");
            if (txtDireccion.Text == "")
                errores.Add("Ingrese una direccion válida");
        }

        private List<string> validarCamposCreateOrEdit()
        {
            List<string> errores = new List<string>();
            this.validarCamposNoUnicos(errores);
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
    }
}
