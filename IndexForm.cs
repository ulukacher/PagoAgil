﻿using PagoAgilFrba.AbmCliente;
using PagoAgilFrba.AbmEmpresa;
using PagoAgilFrba.AbmSucursal;
using PagoAgilFrba.AbmFactura;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PagoAgilFrba.Rendiciones;
using PagoAgilFrba.ListadoEstadistico;
using PagoAgilFrba.AbmRol;
using PagoAgilFrba.RegistroPago;
using PagoAgilFrba.Usuarios;

namespace PagoAgilFrba
{
    public partial class IndexForm : Form
    {
        public IndexForm()
        {
            InitializeComponent();
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var indexClientes = new IndexClientesForm();
            this.Hide();
            indexClientes.Show();
        }

        private void empresasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var indexEmpresas = new IndexEmpresasForm();
            this.Hide();
            indexEmpresas.Show();
        }

        private void sucursalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var indexSucursales = new IndexSucursalesForm();
            this.Hide();
            indexSucursales.Show();
        }

        private void facturasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var indexFacturas = new IndexFacturasForm();
            this.Hide();
            indexFacturas.Show();
        }

        private void rendicionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rendicionForm = new RendicionForm();
            this.Hide();
            rendicionForm.Show();
        }

        private void estadisticasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var estadisticasForm = new EstadisticasForm();
            this.Hide();
            estadisticasForm.Show();
        }
        private void rolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var indexRoles = new IndexRolesForm();
            this.Hide();
            indexRoles.Show();
        }
        private void registroDePagosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var pagoForm = new PagoForm();
            this.Hide();
            pagoForm.Show();
        }
        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var indexUsuariosForm = new IndexUsuariosForm();
            this.Hide();
            indexUsuariosForm.Show();
        }

        private void IndexForm_Load(object sender, EventArgs e)
        {

        }
    }
}
