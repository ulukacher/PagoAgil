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

        decimal importePago = 0;

        public PagoForm()
        {
            InitializeComponent();
            CargarCombo();

            this.ActiveControl = txtNroFactura;

            txtFechaCobro.Enabled = false;

            lblImporte.Text = "$" + importePago;
        }

        private void CargarCombo()
        {
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
                    pago.Importe = importePago;

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

            if (listBox1.Items.Count == 0)
                errores.Add("Ingrese al menos una factura");

            if(Sucursal.SucursalActual != null && Sucursal.SucursalActual.CodigoPostal != ((ComboboxItem)cboSucursal.SelectedItem).Value)
                errores.Add("El usuario no pertenece a la sucursal seleccionada");

            bool entro = false;
            bool entro2 = false;
            bool entro3 = false;

            foreach (Factura factura in listBox1.Items)
            {
                if (factura.FechaVencimiento < DateTime.Now)
                    errores.Add("Una o mas facturas ya vencieron");

                if (FacturasRepository.GetFacturaByNro(factura.Nro).EmpresaCuit == null && entro == false)
                {
                    errores.Add("No existe una factura con ese numero, debe crearla primero");

                    entro = true;
                }

                if(FacturasRepository.FacturaEstaPagaORendida(factura.Nro) == true && entro2 == false)
                {
                    errores.Add("Una o mas facturas ya fueron pagadas o rendidas");

                    entro2 = true;
                }

                if (FacturasRepository.FacturaEsDeCliente(factura.Nro, ((ComboboxItem)cboClienteDNI.SelectedItem).Value) == false && entro3 == false)
                {
                    errores.Add("Una o mas facturas no pertenecen al cliente");

                    entro3 = true;
                }

                if (EmpresasRepository.EmpresaEstaActiva(FacturasRepository.GetFacturaByNro(factura.Nro).EmpresaCuit) == false)
                {
                    errores.Add("Una o mas facturas pertenecen a una empresa que esta inactiva");
                }
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
                if(this.facturaRepetida(decimal.Parse(txtNroFactura.Text)) == true)
                {
                    MessageBox.Show("La factura ya esta en la lista");
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
                        if (FacturasRepository.GetFacturaByNro(decimal.Parse(txtNroFactura.Text)).EmpresaCuit == null)
                        {
                            MessageBox.Show("No existe una factura con ese numero, debe crearla primero");
                        }
                        else
                        {
                            factura = FacturasRepository.GetFacturaByNro(a);

                            importePago += FacturasRepository.ImporteFactura(decimal.Parse(txtNroFactura.Text));

                            lblImporte.Text = "$" + importePago;

                            listBox1.Items.Add(factura);
                        }
                    }
                }
                
            }

            txtNroFactura.Text = "";

            txtNroFactura.Focus();
        }

        private bool facturaRepetida(decimal nroFactura)
        {
            foreach (Factura factura in listBox1.Items)
            {
                if (factura.Nro == nroFactura)
                {
                    return true;
                }
            }

            return false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count > 0)
            {
                Factura f = new Factura();

                f = (Factura)listBox1.SelectedItems[0];

                importePago -= FacturasRepository.ImporteFactura(f.Nro);

                listBox1.Items.Remove(listBox1.SelectedItems[0]);
                listBox1.Refresh();

                lblImporte.Text = "$" + importePago;

                txtNroFactura.Text = "";
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
