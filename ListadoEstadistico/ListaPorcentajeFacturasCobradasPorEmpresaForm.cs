using PagoAgilFrba.Classes.Estadisticas;
using PagoAgilFrba.Repositories;
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
    public partial class ListaPorcentajeFacturasCobradasPorEmpresaForm : Form
    {
        public ListaPorcentajeFacturasCobradasPorEmpresaForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            DateTime fechaDesde = txtFechaDesde.Value;
            DateTime fechaHasta = txtFechaHasta.Value;
            List<ItemReportePorcentajeFacturasCobradas> itemsReporte = EstadisticasRepository.GetPorcentajesFacturasCobradasPorEmpresa(fechaDesde,fechaHasta);
            foreach (var item in itemsReporte)
            {
                dataGridView1.Rows.Add(item.Empresa, Math.Round( item.Porcentaje,2)+ "%");
            }
        }

        private void ListaPorcentajeFacturasCobradasPorEmpresaForm_Load(object sender, EventArgs e)
        {

        }
    }
}
