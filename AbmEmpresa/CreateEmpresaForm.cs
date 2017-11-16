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
    public partial class CreateEmpresaForm : Form
    {
        const string regexCuit = @"^\d{2}\-\d{8}\-\d{1}$";
        const string regexLetrasEspaciosONumeros = @"^[a-zA-Z\s\d]+$";

        public CreateEmpresaForm()
        {
            InitializeComponent(); 
            CargarCombo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Valido que esten todos los datos y que el tipo de dato sea correcto
            var errores = this.validarCamposCreateOrEdit();
            if (errores.Count == 0)
            {
                try
                {
                    Empresa empresa = new Empresa();
                    empresa.Nombre = txtNombre.Text;
                    empresa.Direccion = txtDireccion.Text;
                    empresa.Cuit= txtCuit.Text;
                    empresa.RubroId = ((ComboboxItem)cboRubro.SelectedItem).Value;
                    EmpresasRepository.AgregarEmpresa(empresa);
                    MessageBox.Show("La empresa ha sido agregada correctamente");
                    this.Hide();
                    var indexForm = new IndexEmpresasForm();
                    indexForm.Show();
                }
                catch (SqlException sqlexc)
                {
                    //Violacion de primary key
                    if (sqlexc.Number == 2627)
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
            if (validarCuit())
                errores.Add("Ingrese un CUIT válido");
            if (txtNombre.Text == "" || !Regex.IsMatch(txtNombre.Text, regexLetrasEspaciosONumeros))
                errores.Add("Ingrese un nombre válido");
            if (txtDireccion.Text == "")
                errores.Add("Ingrese una direccion válida");
        }

        private bool validarCuit()
        {
            int[] mult = new[] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
            int total = 0;
            if (txtCuit.Text == "" || !Regex.IsMatch(txtCuit.Text, regexCuit))
            {
                return true;
            }
            else
            {
                char[] nums = txtCuit.Text.Replace("-", "").ToCharArray();

                for (int i = 0; i < mult.Length; i++)
                {
                    total += int.Parse(nums[i].ToString()) * mult[i];
                }
                int resto = total % 11;
                int digito = int.Parse(nums[nums.Length - 1].ToString());
                if (resto == 0)
                    return !(digito == 0);
                else if (resto == 1)
                    return !(digito == 9);
                else
                    return !(digito == (11 - resto));
            }
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

        private void CreateEmpresaForm_Load(object sender, EventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
        {
            var index = new IndexEmpresasForm();
            this.Hide();
            index.Show();
        }
    }
}
