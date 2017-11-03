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
    class FormasDePagoRepository
    {
        public static List<FormaDePago> GetAllFormasDePago()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();

            string query = "SELECT * FROM LOS_MANTECOSOS.FormasDePago";

            SqlCommand command = new SqlCommand(query, conn);

            List<FormaDePago> formasDePago = new List<FormaDePago>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    FormaDePago formaDePago = new FormaDePago();

                    formaDePago.Id = Convert.ToInt32(reader["formapago_id"]);
                    formaDePago.Descripcion = (string)reader["formapago_descripcion"];

                    formasDePago.Add(formaDePago);
                }
            }

            conn.Close();

            return formasDePago;
        }
    }
}
