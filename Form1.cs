using PagoAgilFrba.AbmCliente;
using PagoAgilFrba.Repositories;
using PagoAgilFrba.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PagoAgilFrba
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var viewIndexClientes = new IndexClientesForm();
            this.Hide();
            viewIndexClientes.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userName = txtUsuario.Text;
            string password = txtContrasena.Text;
            if (UsuariosRepository.ExisteUsuarioConEseUsername(userName))
            {
                bool successLogin = UsuariosRepository.Login(userName, password);
                //MessageBox.Show("Existe un usuario con ese username");
                if (successLogin)
                {
                    var roles = RolesRepository.GetRolDeUsuario(userName);
                    
                    if (roles.Count > 1)
                    {
                        var indexForm = new ElegirRolForm(roles, userName);
                        this.Hide();
                        indexForm.Show();
                    }
                    else if (roles.Count == 1)
                    {
                        Rol.SetRolActual(roles.First(), userName);
                        var indexForm = new IndexForm();
                        this.Hide();
                        indexForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Error, su usuario no tiene ningun rol asignado");
                    }
                }
                else 
                {
                    MessageBox.Show("Ha ingresado una contraseña incorrecta");
                    txtContrasena.Clear();
                    txtUsuario.Clear();
                    txtUsuario.Focus();
                }
            }
            else
            {
                MessageBox.Show("No existe un usuario con ese username activo");
                txtContrasena.Clear();
                txtUsuario.Clear();
                txtUsuario.Focus();
            }
           
        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
