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

namespace PagoAgilFrba.AbmEmpresa
{
    public partial class EditEmpresaForm : Form
    {
        const string regexCuit = @"^\d{2}\-\d{8}\-\d{1}$";
        const string regexLetrasEspaciosONumeros = @"^[a-zA-Z\s\d]+$";
        const string regexSoloNumeros = @"^\d+$";

        public EditEmpresaForm(Empresa _empresa)
        {
            this.empresa = _empresa;
            InitializeComponent();
            CargarCombo();
            ComboboxItem cbi = new ComboboxItem();
            cbi.Text = empresa.Rubro;
            cbi.Value = empresa.RubroId;

            txtNombre.Text = empresa.Nombre;
            txtDireccion.Text = empresa.Direccion;
            txtCuit.Text = empresa.Cuit;
            cboRubro.SelectedItem = cbi;
            chkActiva.Checked = empresa.Activa;
        }

        public EditEmpresaForm()
        {
            InitializeComponent();
        }

        public Empresa empresa { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            //Valido que esten todos los datos y que el tipo de dato sea correcto
            var errores = this.validarCamposCreateOrEdit();
            if (errores.Count == 0)
            {
                try
                {
                    Empresa empresaAEditar = new Empresa();
                    empresaAEditar.Nombre = txtNombre.Text;
                    empresaAEditar.Direccion = txtDireccion.Text;
                    empresaAEditar.Cuit = txtCuit.Text;
                    empresaAEditar.RubroId = ((ComboboxItem)cboRubro.SelectedItem).Value;
                    empresaAEditar.Activa = chkActiva.Checked;

                    if (empresaAEditar.Activa)
                    {
                        EmpresasRepository.EditarEmpresa(empresaAEditar, empresa.Cuit);
                        MessageBox.Show("La empresa ha sido modificada correctamente");
                        this.Hide();
                        var indexForm = new IndexEmpresasForm();
                        indexForm.Show();
                    }
                    else if (EmpresasRepository.PuedeDarDeBaja(empresa.Cuit)) {
                        EmpresasRepository.EditarEmpresa(empresaAEditar, empresa.Cuit);
                        MessageBox.Show("La empresa ha sido modificada correctamente");
                        this.Hide();
                        var indexForm = new IndexEmpresasForm();
                        indexForm.Show();
                    }
                    else
                        MessageBox.Show("La empresa no puede tener estado inactivo debido a que no todas sus facturas fueron rendidas.");

                    
                }
                catch (SqlException exc)
                {
                    //Violacion de primary key
                    if (exc.Number == 2627)
                        MessageBox.Show("Ya existe una empresa con ese CUIT");
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
            //Valido el CUIT
            if (txtCuit.Text == "" || !Regex.IsMatch(txtCuit.Text, regexCuit))
                errores.Add("Ingrese un CUIT válido");
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

        private void CargarCombo()
        {
            var rubros = RubrosRepository.GetAllRubros();
            foreach (var item in rubros)
            {
                ComboboxItem cbItem = new ComboboxItem();
                cbItem.Text = item.rubr_descripcion;
                cbItem.Value = item.rubr_id;
                cboRubro.Items.Add(cbItem);
            }
            cboRubro.SelectedIndex = 0;
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
            var index = new IndexEmpresasForm();
            this.Hide();
            index.Show();
        }
    }
}
