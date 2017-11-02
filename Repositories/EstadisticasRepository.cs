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
            command.Parameters.AddWithValue("@fechaDesde", fechaDesde);
            command.Parameters.AddWithValue("@fechaHasta", fechaHasta.AddDays(1));
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
    }
}
