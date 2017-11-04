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
    public class RolesRepository
    {
        public static void HabilitarRol(int id)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            var query = "update LOS_MANTECOSOS.Roles set rol_activo=1 where rol_id = @id_rol;";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@id_rol", id);
            command.ExecuteNonQuery();
            conn.Close();
        }

        public static void DesHabilitarRol(int id)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            var query = "update LOS_MANTECOSOS.Roles set rol_activo=0 where rol_id = @id_rol;";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@id_rol", id);
            command.ExecuteNonQuery();
            conn.Close();
        }

        public static void EditarRol(Rol rol, List<Funcionalidad> nuevasFunc)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            var query = "update LOS_MANTECOSOS.Roles set rol_nombre=@nombre where rol_id = @id_rol;";
            foreach (var funcNueva in nuevasFunc)
            {
                if (!rol.Funcionalidades.Contains(funcNueva))
                {
                    //agregar funcNueva
                    query += "insert into LOS_MANTECOSOS.FuncionalidadesPorRoles values (@id_rol,"
                    + funcNueva.Id.ToString() + ");";
                }
            }
            foreach (var funcVieja in rol.Funcionalidades)
            {
                if (!nuevasFunc.Contains(funcVieja))
                {
                    //borrar funcVieja
                    query += "delete from LOS_MANTECOSOS.FuncionalidadesPorRoles where funcrol_IdRol = @id_rol and funcrol_FuncionalidadId = "
                    + funcVieja.Id.ToString() + ";";
                }
            }

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@nombre", rol.Nombre);
            command.Parameters.AddWithValue("@id_rol", rol.Id);
            command.ExecuteNonQuery();
            conn.Close();
        }

        public static void AgregarRol(Rol rol)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);

            conn.Open();

            string query = "insert into LOS_MANTECOSOS.Roles values (@nombre,@activo); DECLARE @id_rol numeric(18,0) = SCOPE_IDENTITY();";
            foreach (var func in rol.Funcionalidades)
            {
                query += "insert into LOS_MANTECOSOS.FuncionalidadesPorRoles values (@id_rol," 
                    + func.Id.ToString() + ");";
            }
            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.AddWithValue("@nombre", rol.Nombre);
            command.Parameters.AddWithValue("@activo", 1);

            command.ExecuteNonQuery();

            conn.Close();
        }

        public static int GetCantidadRolesConEseNombre(string nombre)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            SqlCommand command = new SqlCommand();
            string query = "SELECT COUNT(*) as cantidad FROM GD2C2017.LOS_MANTECOSOS.Roles Where rol_nombre = @nombre";
            command.Connection = conn;
            command.CommandText = query;
            command.Parameters.AddWithValue("@nombre", nombre);
            int cant=-1;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    cant = Convert.ToInt32(reader["cantidad"]);
                }
            }
            conn.Close();
            return cant;
        }

        public static List<Rol> GetAllRoles()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            SqlCommand command = new SqlCommand();
            string query = "SELECT  rol_id , rol_nombre, rol_activo FROM GD2C2017.LOS_MANTECOSOS.Roles";
            command.Connection = conn;
            command.CommandText = query;
            List<Rol> roles = new List<Rol>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Rol rol = new Rol();
                    rol.Id = Convert.ToInt32(reader["rol_id"]);
                    rol.Nombre  = (string)reader["rol_nombre"];
                    rol.Activo = (bool)reader["rol_activo"];
                    roles.Add(rol);
                }
            }
            conn.Close();
            return roles;
        }
    }
}
