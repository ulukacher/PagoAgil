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

namespace PagoAgilFrba.RegistroPago
{
    public partial class PagoForm : Form
    {
        const string regexSoloLetras = "^[a-zA-Z]+$";
        const string regexSoloNumeros = @"^\d+$";

        public static string ConnectionString { get { return ConfigurationManager.ConnectionStrings["GDD"].ConnectionString; } }
        SqlConnection conn = new SqlConnection(ConnectionString);

        List<Factura> listaFacturas = new List<Factura>();

        public PagoForm()
        {
            InitializeComponent();
            CargarCombo();

            this.ActiveControl = txtNroFactura;

            txtFechaCobro.Enabled = false;
        }

        private void CargarCombo()
        {
            var empresas = EmpresasRepository.GetAllEmpresas();

            foreach (var item in empresas)
            {
                ComboBoxItemStringValue cbItem = new ComboBoxItemStringValue();

                cbItem.Text = item.Nombre;
                cbItem.Value = item.Cuit;

                cboEmpresa.Items.Add(cbItem);
            }

            cboEmpresa.SelectedIndex = 0;

            var clientes = ClientesRepository.GetAllClientes();

            foreach (var item in clientes)
            {
                ComboboxItem cbItem = new ComboboxItem();

                cbItem.Text = item.DNI.ToString();
                cbItem.Value = Convert.ToInt32(item.DNI); ;

                cboClienteDNI.Items.Add(cbItem);
            }

            cboClienteDNI.SelectedIndex = 0;

            var sucursales = SucursalesRepository.GetAllSucursales();

            foreach (var item in sucursales)
            {
                ComboboxItem cbItem = new ComboboxItem();

                cbItem.Text = item.Nombre;
                cbItem.Value = item.CodigoPostal;

                cboSucursal.Items.Add(cbItem);
            }

            cboSucursal.SelectedIndex = 0;

            var formasDePago = FormasDePagoRepository.GetAllFormasDePago();

            foreach (var item in formasDePago)
            {
                ComboboxItem cbItem = new ComboboxItem();

                cbItem.Text = item.Descripcion;
                cbItem.Value = item.Id;

                cboMedioDePago.Items.Add(cbItem);
            }

            cboMedioDePago.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var errores = this.validarCamposCreateOrEdit();

            if (errores.Count == 0)
            {
                try
                {
                    Pago pago = new Pago();

                    pago.Nro = PagosRepository.GetNumeroPagoMaximo() + 1;
                    pago.Sucursal = ((ComboboxItem)cboSucursal.SelectedItem).Value;
                    pago.FormaDePago = ((ComboboxItem)cboMedioDePago.SelectedItem).Value;
                    pago.ClienteDNI = ((ComboboxItem)cboClienteDNI.SelectedItem).Value;
                    pago.Fecha = txtFechaCobro.Value;
                    pago.Importe = decimal.Parse(txtImporte.Text);

                    foreach (Factura factura in listBox1.Items)
                    {
                        listaFacturas.Add(factura);
                    }

                    PagosRepository.AgregarPago(pago, listaFacturas);

                    MessageBox.Show("El pago fue registrado correctamente");

                    var index = new PagoForm();
                    this.Hide();
                    index.Show();
                }
                catch (SqlException sqlexc)
                {
                    //Violacion de primary key
                    if (sqlexc.Number == 2627)
                        MessageBox.Show("Ya existe una factura con ese numero");

                    if (sqlexc.Number == 547)
                        MessageBox.Show("No hay ningun cliente registrado con ese DNI");

                    conn.Close();
                }

            }
            else
            {
                string errorMessage = string.Join("\n", errores.ToArray());
                MessageBox.Show(errorMessage);
            }

            listaFacturas.Clear();
        }

        private List<string> validarCamposCreateOrEdit()
        {
            List<string> errores = new List<string>();

            if (txtNroFactura.Text != "")
            {
                if (!Regex.IsMatch(txtNroFactura.Text, regexSoloNumeros))
                {
                    errores.Add("Ingrese un numero de factura valido");
                }
            }

            if (txtImporte.Text == "" || !Regex.IsMatch(txtImporte.Text, regexSoloNumeros))
                errores.Add("Ingrese un importe valido");

            if (txtFechaVencimiento.Text == "" || txtFechaVencimiento.Value > DateTime.Now)
                errores.Add("Ingrese una fecha de vencimiento válida");

            if (listBox1.Items.Count == 0)
                errores.Add("Ingrese al menos una factura");

            bool entro = false;

            foreach (Factura factura in listBox1.Items)
            {
                if (FacturasRepository.GetFacturaByNro(factura.Nro).EmpresaCuit == null && entro == false)
                {
                    errores.Add("No existe una factura con ese numero, debe crearla primero");

                    entro = true;
                }

                if(FacturasRepository.FacturaEstaPagaORendida(factura.Nro) == true)
                {
                    errores.Add("Esa factura ya fue pagada o rendida");

                    entro = true;
                }
            }

            if (EmpresasRepository.EmpresaEstaActiva(((ComboBoxItemStringValue)cboEmpresa.SelectedItem).Value) == false)
            {
                errores.Add("La empresa seleccionada esta inactiva");
            }

            return errores;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(txtNroFactura.Text, regexSoloNumeros))
            {
                MessageBox.Show("Ingrese un numero de factura valido");
            }
            else
            {
                Factura factura = new Factura();

                int a;

                if (!int.TryParse(txtNroFactura.Text, out a))
                {
                    MessageBox.Show("La factura debe ser un numero");
                }
                else
                {
                    factura.Nro = a;

                    listBox1.Items.Add(factura);
                }
            }

            txtNroFactura.Text = "";

            txtNroFactura.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count > 0)
            {
                listBox1.Items.Remove(listBox1.SelectedItems[0]);
                listBox1.Refresh();

                txtNroFactura.Text = "";
            }
        }
    }
}
