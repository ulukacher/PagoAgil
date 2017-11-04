using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PagoAgilFrba.Classes;
using PagoAgilFrba.Repositories;

namespace PagoAgilFrba
{
    public partial class ElegirRolForm : Form
    {
        string username;
        public ElegirRolForm(List<Rol> roles, string _username)
        {
            username = _username;
            InitializeComponent();
            foreach (var rol in roles)
            {
                listBox1.Items.Add(rol);
            }
            listBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Rol.SetRolActual((Rol)listBox1.SelectedItem, username);
            var indexForm = new IndexForm();
            this.Hide();
            indexForm.Show();
        }
    }
}
