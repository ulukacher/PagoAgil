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

        private string verbo;
        List<Funcionalidad> FuncionalidadesDelRol;
        List<Funcionalidad> Funcionalidades;
        Rol Rol;
        bool ParaModificar;
        public CrearRolForm()
        {
            FuncionalidadesDelRol = new List<Funcionalidad>();
            InitializeComponent();
            CargarCombo();
            label1.Text = "Crear Rol";
            button1.Text = "Crear";
            verbo = "agregado";
            ParaModificar = false;
        }

        public CrearRolForm(Rol rol)
        {
            Rol = rol;
            InitializeComponent();
            CargarCombo();
            rol.Funcionalidades = FuncionalidadesRepository.GetAllFuncFromRol(rol);
            FuncionalidadesDelRol = new List<Funcionalidad>(rol.Funcionalidades);
            label1.Text = "Modificar Rol";
            button1.Text = "Modificar";
            txtNombre.Text = rol.Nombre;
            foreach (var item in FuncionalidadesDelRol)
            {
                listBoxFuncionalidades.Items.Add(item);
            }
            verbo = "modificado";
            ParaModificar = true;
        }

        private void CargarCombo()
        {
            Funcionalidades = FuncionalidadesRepository.GetAllFuncionalidades();
            foreach (var item in Funcionalidades)
            {
                cboFuncionalidades.Items.Add(item);
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
                    
                    if (ParaModificar)
                    {
                        Rol.Nombre = txtNombre.Text;
                        RolesRepository.EditarRol(Rol, FuncionalidadesDelRol);

                        MessageBox.Show("El Rol ha sido " + verbo + " correctamente.");

                        Sucursal.SucursalActual = null;

                        var login = new Form1();
                        this.Hide();
                        login.Show();
                    }
                    else
                    {
                        Rol rol = new Rol();
                        rol.Nombre = txtNombre.Text;
                        rol.Funcionalidades = FuncionalidadesDelRol;
                        RolesRepository.AgregarRol(rol);

                        MessageBox.Show("El Rol ha sido " + verbo + " correctamente.");
                        this.Hide();
                        var indexForm = new IndexRolesForm();
                        indexForm.Show();
                    }
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
                if (!(ParaModificar && txtNombre.Text == Rol.Nombre))
                {
                    int cantIguales = RolesRepository.GetCantidadRolesConEseNombre(txtNombre.Text);
                    if (cantIguales > 0)
                        errores.Add("Ya existe un rol con ese nombre");
                }

            }
            return errores;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var funcionalidad = ((Funcionalidad)cboFuncionalidades.SelectedItem);
            if (!FuncionalidadesDelRol.Contains(funcionalidad))
            {
                listBoxFuncionalidades.Items.Add(funcionalidad);
                FuncionalidadesDelRol.Add(funcionalidad);
            }
            else
                MessageBox.Show("Las funcionalidades no pueden estar repetidas en un rol.");

        }

        private void listBoxFuncionalidades_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            var item = ((Funcionalidad)listBoxFuncionalidades.SelectedItem);
            if (item == null)
            {
                MessageBox.Show("Seleccione una Funcionalidad para agregar");
            }
            else
            {
                FuncionalidadesDelRol.Remove(item);
                listBoxFuncionalidades.Items.Remove(item);
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
            var index = new IndexRolesForm();
            this.Hide();
            index.Show();
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
            var index = new IndexRolesForm();
            this.Hide();
            index.Show();
        }

    }
}
