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

namespace PagoAgilFrba.AbmFactura
{
    public partial class EditFacturaForm : Form
    {
        const string regexSoloLetras = "^[a-zA-Z]+$";
        const string regexSoloNumeros = @"^\d+$";

        public static string ConnectionString { get { return ConfigurationManager.ConnectionStrings["GDD"].ConnectionString; } }
        SqlConnection conn = new SqlConnection(ConnectionString);

        List<ItemFactura> listaItems = new List<ItemFactura>();

        decimal nroFacturaOriginal;

        decimal importePago = 0;

        public EditFacturaForm(Factura factura)
        {
            InitializeComponent();
            CargarCombo(factura.EmpresaCuit);

            nroFacturaOriginal = factura.Nro;

            txtNroFactura.Text = factura.Nro.ToString();
            txtDNI.Text = factura.ClienteDNI.ToString();
            txtFecha.Text = factura.Fecha.ToString();
            txtFechaVencimiento.Text = factura.FechaVencimiento.ToString();

            var items = FacturasRepository.GetAllItemsFactura(factura.Nro);

            foreach (ItemFactura item in items)
            {
                listBox1.Items.Add(item);

                importePago += item.Cantidad * item.Monto;
            }

            lblImporte.Text = "$" + importePago;
        }

        private void CargarCombo(string empresaCuit)
        {
            var empresas = EmpresasRepository.GetAllEmpresas();

            int i = -1;
            bool encontroEmpresa = false;

            foreach (var item in empresas)
            {
                ComboBoxItemStringValue cbItem = new ComboBoxItemStringValue();

                cbItem.Text = item.Nombre;
                cbItem.Value = item.Cuit;

                cboEmpresa.Items.Add(cbItem);

                if (item.Cuit == empresaCuit && encontroEmpresa == false)
                {
                    i++;

                    encontroEmpresa = true;
                }
                else if (item.Cuit != empresaCuit && encontroEmpresa == false)
                {
                    i++;
                }
            }

            cboEmpresa.SelectedIndex = i;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var errores = this.validarCamposCreateOrEdit();

            if (errores.Count == 0)
            {
                try
                {
                    Factura factura = new Factura();

                    factura.Nro = decimal.Parse(txtNroFactura.Text);
                    factura.EmpresaCuit = ((ComboBoxItemStringValue)cboEmpresa.SelectedItem).Value;
                    factura.ClienteDNI = decimal.Parse(txtDNI.Text);
                    factura.Fecha = txtFecha.Value;
                    factura.Estado = 0;
                    factura.FechaVencimiento = txtFechaVencimiento.Value;

                    int i = 1;

                    foreach (ItemFactura itemFactura in listBox1.Items)
                    {
                        itemFactura.NroFactura = decimal.Parse(txtNroFactura.Text);
                        itemFactura.NroItem = i;

                        listaItems.Add(itemFactura);

                        i++;
                    }

                    FacturasRepository.EditarFactura(factura, listaItems, nroFacturaOriginal);

                    MessageBox.Show("La factura fue modificada correctamente");

                    var index = new IndexFacturasForm();
                    this.Hide();
                    index.Show();
                }
                catch (SqlException sqlexc)
                {
                    //Violacion de primary key
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

            listaItems.Clear();
        }

        private List<string> validarCamposCreateOrEdit()
        {
            List<string> errores = new List<string>();

            if (txtNroFactura.Text == "" || !Regex.IsMatch(txtNroFactura.Text, regexSoloNumeros))
                errores.Add("Ingrese un numero de factura valido");

            if (txtFecha.Text == "" || txtFecha.Value >= ConfiguracionFecha.FechaSistema)
                errores.Add("La fecha de la factura debe ser anterior a la fecha del sistema (" + ConfiguracionFecha.FechaSistema + ")");

            if (txtFechaVencimiento.Text == "" || txtFechaVencimiento.Value < txtFecha.Value)
                errores.Add("La fecha de vencimiento debe ser mayor a la fecha de la factura");

            if (txtFechaVencimiento.Text == "" || txtFechaVencimiento.Value <= ConfiguracionFecha.FechaSistema)
                errores.Add("La fecha de vencimiento de la factura debe ser posterior a la fecha del sistema (" + ConfiguracionFecha.FechaSistema + ")");

            if (txtDNI.Text == "" || !Regex.IsMatch(txtDNI.Text, regexSoloNumeros))
                errores.Add("Ingrese un numero de DNI valido");

            if (listBox1.Items.Count == 0)
                errores.Add("Ingrese al menos un item");

            return errores;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtNroFactura.Text == "")
            {
                MessageBox.Show("Ingrese una factura antes de ingresar un item");
            }
            else if (txtDNI.Text == "")
            {
                MessageBox.Show("Ingrese un DNI antes de ingresar un item");
            }
            else
            {
                ItemFactura item = new ItemFactura();

                item.Detalle = txtNombreItem.Text;

                int a;
                int b;

                if (!int.TryParse(txtCantidadItems.Text, out a))
                {
                    MessageBox.Show("La cantidad de items debe ser un numero");
                }
                else
                {
                    item.Cantidad = a;

                    if (!int.TryParse(txtMontoItem.Text, out b))
                    {
                        MessageBox.Show("El monto del item debe ser un numero");
                    }
                    else
                    {
                        item.Monto = b;

                        importePago += a * b;

                        lblImporte.Text = "$" + importePago;

                        listBox1.Items.Add(item);
                    }
                }
            }

            txtNombreItem.Text = "";
            txtCantidadItems.Text = "";
            txtMontoItem.Text = "";

            txtNombreItem.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count > 0)
            {
                ItemFactura i = new ItemFactura();

                i = (ItemFactura)listBox1.SelectedItems[0];

                importePago -= i.Cantidad * i.Monto;

                listBox1.Items.Remove(listBox1.SelectedItems[0]);
                listBox1.Refresh();

                lblImporte.Text = "$" + importePago;

                txtNombreItem.Text = "";
                txtCantidadItems.Text = "";
                txtMontoItem.Text = "";
            }
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
            var index = new IndexFacturasForm();
            this.Hide();
            index.Show();
        }
    }
}
