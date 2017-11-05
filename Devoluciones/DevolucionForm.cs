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

namespace PagoAgilFrba.Devoluciones
{
    public partial class DevolucionForm : Form
    {
        const string regexSoloLetras = "^[a-zA-Z]+$";
        const string regexSoloNumeros = @"^\d+$";

        public static string ConnectionString { get { return ConfigurationManager.ConnectionStrings["GDD"].ConnectionString; } }
        SqlConnection conn = new SqlConnection(ConnectionString);

        List<Factura> listaFacturas = new List<Factura>();

        public DevolucionForm()
        {
            InitializeComponent();

            CargarCombo();
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var errores = this.validarCamposCreateOrEdit();

            if (errores.Count == 0)
            {
                try
                {
                    /*Pago pago = new Pago();

                    pago.Nro = PagosRepository.GetNumeroPagoMaximo() + 1;
                    pago.Sucursal = ((ComboboxItem)cboSucursal.SelectedItem).Value;
                    pago.FormaDePago = ((ComboboxItem)cboMedioDePago.SelectedItem).Value;
                    pago.ClienteDNI = ((ComboboxItem)cboClienteDNI.SelectedItem).Value;
                    pago.Fecha = txtFechaCobro.Value;
                    pago.Importe = importePago;*/

                    foreach (Factura factura in listBox1.Items)
                    {
                        listaFacturas.Add(factura);
                    }

                    DevolucionesRepository.DevolverFactura(listaFacturas, txtMotivo.Text);

                    MessageBox.Show("El pago fue registrado correctamente");

                    var index = new DevolucionForm();
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

            if (txtMotivo.Text == "")
            {
                errores.Add("Ingrese el motivo de la devolucion");
            }

            if (listBox1.Items.Count == 0)
                errores.Add("Ingrese al menos una factura");

            bool entro = false;
            bool entro2 = false;
            bool entro3 = false;

            foreach (Factura factura in listBox1.Items)
            {
                if (FacturasRepository.GetFacturaByNro(factura.Nro).EmpresaCuit == null && entro == false)
                {
                    errores.Add("No existe una factura con ese numero, debe crearla primero");

                    entro = true;
                }

                if (factura.Estado != (int)EstadoFactura.Pagada && entro2 == false)
                {
                    errores.Add("Una o mas facturas estan pendiente de pago o rendidas");

                    entro2 = true;
                }

                if (factura.ClienteDNI != ((ComboboxItem)cboClienteDNI.SelectedItem).Value && entro3 == false)
                {
                    errores.Add("Una o mas facturas no pertenecen al cliente");

                    entro3 = true;
                }

                if (EmpresasRepository.EmpresaEstaActiva(factura.EmpresaCuit) == false)
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
                if (this.facturaRepetida(decimal.Parse(txtNroFactura.Text)) == true)
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
                listBox1.Items.Remove(listBox1.SelectedItems[0]);
                listBox1.Refresh();

                txtNroFactura.Text = "";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var index = new IndexFacturasForm();
            this.Hide();
            index.Show();
        }
    }
}