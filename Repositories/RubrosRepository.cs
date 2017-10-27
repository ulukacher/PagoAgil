using PagoAgilFrba.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PagoAgilFrba.Repositories
{
    public class RubrosRepository
    {
        public static List<Rubro> GetAllRubros()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "Select * from dbo.Rubros";
            SqlCommand command = new SqlCommand(query, conn);
            List<Rubro> rubros = new List<Rubro>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Rubro rubro = new Rubro();
                    rubro.rubr_id = Convert.ToInt32(reader["rubr_id"]);
                    rubro.rubr_descripcion = (string)reader["rubr_descripcion"];
                    rubros.Add(rubro);
                }
            }
            conn.Close();
            return rubros;
        }
    }
}
