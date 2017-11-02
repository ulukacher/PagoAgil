using PagoAgilFrba.Classes.Estadisticas;
using PagoAgilFrba.Helpers;
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
    public partial class ListadoEmpresasMontoRendidoForm : Form
    {
        public ListadoEmpresasMontoRendidoForm()
        {
            InitializeComponent();
        }

        private void ListadoEmpresasMontoRendidoForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            DateTime fechaDesde =  txtFechaDesde.Value;
            DateTime fechaHasta = txtFechaHasta.Value;
            List<ItemReporteMontosRendidosEmpresas> itemsReporte = EstadisticasRepository.GetMontosRendidosPorEmpresa(fechaDesde,fechaHasta);
            foreach (var item in itemsReporte)
            {
                dataGridView1.Rows.Add(item.EmpresaNombre, item.TotalEnRendiciones);
            }
        }
    }
}
