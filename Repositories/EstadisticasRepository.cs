using PagoAgilFrba.Classes.Estadisticas;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagoAgilFrba.Repositories
{
    public class EstadisticasRepository
    {
        public static List<ItemReporteMontosRendidosEmpresas> GetMontosRendidosPorEmpresa(DateTime fechaDesde, DateTime fechaHasta)
        {
            List<ItemReporteMontosRendidosEmpresas> lst = new List<ItemReporteMontosRendidosEmpresas>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "SELECT top 5 empr_nombre,sum(totalRendido) as totalRendido FROM LOS_MANTECOSOS.vMontoRendidoPorEmpresa"
                + " where fechaRendicion >= @fechaDesde and fechaRendicion < @fechaHasta group by empr_nombre order by 2 desc";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@fechaDesde", fechaDesde.Date);
            command.Parameters.AddWithValue("@fechaHasta", fechaHasta.AddDays(1).Date);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    ItemReporteMontosRendidosEmpresas item = new ItemReporteMontosRendidosEmpresas();
                    item.EmpresaNombre = (string)reader["empr_nombre"];
                    item.TotalEnRendiciones = (decimal)reader["totalRendido"];
                    lst.Add(item);
                }
                
            }
            conn.Close();
            return lst;


        }

        public static List<ItemReporteClientesConMasPagos> GetClientesConMasPagos(DateTime fechaDesde, DateTime fechaHasta)
        {
            List<ItemReporteClientesConMasPagos> lst = new List<ItemReporteClientesConMasPagos>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "select top 5 count(*) as cantidad,(select c1.clie_nombre + ' ' + c1.clie_apellido from LOS_MANTECOSOS.Clientes c1 where c1.clie_dni = dni) as nombreCompleto" +
                            " from LOS_MANTECOSOS.vClientesPagos" +
                            " where fechaPago >=@fechaDesde and fechaPago < @fechaHasta" +
                            " group by dni" +
                            " order by 1 desc";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@fechaDesde", fechaDesde.Date);
            command.Parameters.AddWithValue("@fechaHasta", fechaHasta.AddDays(1).Date);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    ItemReporteClientesConMasPagos item = new ItemReporteClientesConMasPagos();
                    item.Cliente = (string)reader["nombreCompleto"];
                    item.CantidadPagos = (int)reader["cantidad"];
                    lst.Add(item);
                }

            }
            conn.Close();
            return lst;
        }

        public static List<ItemReportePorcentajeFacturasCobradas> GetPorcentajesFacturasCobradasPorEmpresa(DateTime fechaDesde, DateTime fechaHasta)
        {
            List<ItemReportePorcentajeFacturasCobradas> lst = new List<ItemReportePorcentajeFacturasCobradas>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "select * from LOS_MANTECOSOS.ObtenerPorcentajeFacturasCobradasPorEmpresas(@fechaDesde,@fechaHasta)";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@fechaDesde", fechaDesde.Date);
            command.Parameters.AddWithValue("@fechaHasta", fechaHasta.AddDays(1).Date);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    ItemReportePorcentajeFacturasCobradas item = new ItemReportePorcentajeFacturasCobradas();
                    item.Empresa = (string)reader["nombre"];
                    item.Porcentaje = (decimal)reader["porcentaje"];
                    lst.Add(item);
                }

            }
            conn.Close();
            return lst;
        }
        public static List<ItemReporteClientesMasFieles> GetClientesMasFieles(DateTime fechaDesde, DateTime fechaHasta)
        {
            List<ItemReporteClientesMasFieles> lst = new List<ItemReporteClientesMasFieles>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "select * from LOS_MANTECOSOS.ClientesMasFieles(@fechaDesde,@fechaHasta)";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@fechaDesde", fechaDesde.Date);
            command.Parameters.AddWithValue("@fechaHasta", fechaHasta.AddDays(1).Date);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    ItemReporteClientesMasFieles item = new ItemReporteClientesMasFieles();
                    item.Cliente = (string)reader["nombreCompleto"];
                    item.Porcentaje = (decimal)reader["porcentaje"];
                    lst.Add(item);
                }

            }
            conn.Close();
            return lst;
        }
    }
}
