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
using System.Linq;
namespace PagoAgilFrba.AbmRol
{
    public partial class CrearRolForm : Form
    {

        const string regexLetrasEspaciosONumeros = @"^[a-zA-Z\s\d]+$";
        const string regexSoloLetras = "^[a-zA-Z]+$";
        const string regexSoloNumeros = @"^\d+$";
        public static string ConnectionString { get { return ConfigurationManager.ConnectionStrings["GDD"].ConnectionString; } }
        SqlConnection conn = new SqlConnection(ConnectionString);

        List<Funcionalidad> funcionalidades;

        public CrearRolForm()
        {
            funcionalidades = new List<Funcionalidad>();
            InitializeComponent();
            CargarCombo();
        }

        private void CargarCombo()
        {
            var Funcionalidades = RolesRepository.GetAllFuncionalidades();
            foreach (var item in Funcionalidades)
            {
                ComboboxItem cbItem = new ComboboxItem();
                cbItem.Text = item.Nombre;
                cbItem.Value = item.Id;
                cboFuncionalidades.Items.Add(cbItem);
            }
            cboFuncionalidades.SelectedIndex = 0;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //Valido que esten todos los datos y que el tipo de dato sea correcto
            var errores = this.validarCamposCreateOrEdit();
            if (errores.Count == 0)
            {
                try
                {
                    Rol rol = new Rol();
                    rol.Nombre = txtNombre.Text;
                    rol.Funcionalidades = funcionalidades;
                    RolesRepository.AgregarRol(rol);
                    MessageBox.Show("El Rol ha sido agregado correctamente");
                    this.Hide();
                    var indexForm = new IndexRolesForm();
                    indexForm.Show();
                }
                catch (SqlException sqlexc)
                {
                    MessageBox.Show(sqlexc.Message);
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

            //Valido el nombre
            if (txtNombre.Text == "" || !Regex.IsMatch(txtNombre.Text, regexLetrasEspaciosONumeros))
                errores.Add("Ingrese un nombre valido");
            else
            {
                int cantIguales = RolesRepository.GetCantidadRolesConEseNombre(txtNombre.Text);
                if (cantIguales > 0)
                    errores.Add("Ya existe un rol con ese nombre");
            }
            return errores;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var funcionalidad = ((ComboboxItem)cboFuncionalidades.SelectedItem);
            if (!funcionalidades.Any(func => func.Id == funcionalidad.Value))
            {
                listBoxFuncionalidades.Items.Add(cboFuncionalidades.SelectedItem);
                funcionalidades.Add(new Funcionalidad { Id = funcionalidad.Value, Nombre = funcionalidad.Text });
            }
            else
                MessageBox.Show("Las funcionalidades no pueden estar repetidas en un rol.");

        }

    }
}
