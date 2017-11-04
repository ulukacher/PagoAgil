using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PagoAgilFrba.ListadoEstadistico
{
    public partial class EstadisticasForm : Form
    {
        public EstadisticasForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var listadoEmpresasMontoRendidoForm = new ListadoEmpresasMontoRendidoForm();
            this.Hide();
            listadoEmpresasMontoRendidoForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var listadoClientesConMasPagos = new ListadoClientesConMasPagosForm();
            this.Hide();
            listadoClientesConMasPagos.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var listadoPorcentajFacturasCobradasEmpresas = new ListaPorcentajeFacturasCobradasPorEmpresaForm();
            this.Hide();
            listadoPorcentajFacturasCobradasEmpresas.Show();
        }
    }
}
