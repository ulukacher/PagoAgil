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
    public partial class ListadoClientesMasFieles : Form
    {
        public ListadoClientesMasFieles()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            DateTime fechaDesde = txtFechaDesde.Value;
            DateTime fechaHasta = txtFechaHasta.Value;
            List<ItemReporteClientesMasFieles> itemsReporte = EstadisticasRepository.GetClientesMasFieles(fechaDesde,fechaHasta);
            foreach (var item in itemsReporte)
            {
                dataGridView1.Rows.Add(item.Cliente, Math.Round(item.Porcentaje, 2) + "%");
            }
        }
    }
}
