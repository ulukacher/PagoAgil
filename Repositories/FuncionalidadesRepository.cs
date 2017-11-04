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
    public class FuncionalidadesRepository
    {
        public static List<Funcionalidad> GetAllFuncFromRol(Rol rol, List<Funcionalidad> funcionalidades)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            SqlCommand command = new SqlCommand();
            string query = "SELECT funcrol_FuncionalidadId FROM GD2C2017.LOS_MANTECOSOS.FuncionalidadesPorRoles Where funcrol_IdRol = @id_rol";
            command.Connection = conn;
            command.CommandText = query;
            command.Parameters.AddWithValue("@id_rol", rol.Id);
            List<Funcionalidad> funcs = new List<Funcionalidad>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = Convert.ToInt32(reader["funcrol_FuncionalidadId"]);
                    var func = funcionalidades.Find(f => f.Id == id);
                    funcs.Add(func);
                }
            }
            conn.Close();
            return funcs;
        }

        public static List<Funcionalidad> GetAllFuncionalidades()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            SqlCommand command = new SqlCommand();
            string query = "SELECT  func_id , func_nombre FROM GD2C2017.LOS_MANTECOSOS.Funcionalidades";
            command.Connection = conn;
            command.CommandText = query;
            List<Funcionalidad> funcs = new List<Funcionalidad>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Funcionalidad func = new Funcionalidad();
                    func.Id = Convert.ToInt32(reader["func_id"]);
                    func.Nombre = (string)reader["func_nombre"];
                    funcs.Add(func);
                }
            }
            conn.Close();
            return funcs;
        }
    }
}
