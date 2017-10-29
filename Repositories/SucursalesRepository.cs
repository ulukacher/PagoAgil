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
    public class SucursalesRepository
    {
        const string regexSoloNumeros = @"^\d+$";
        public static void DarDeBajaSucursal(int codPostal)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "update dbo.Sucursales set sucu_activa = 0 where sucu_codigoPostal = @codigoPostal";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@codigoPostal", codPostal);
            command.ExecuteNonQuery();
            conn.Close();
        }

        public static void AgregarSucursal(Sucursal sucursal)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "insert into dbo.Sucursales values (@codigopostal,@nombre,@direccion,@activa)";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@nombre", sucursal.Nombre);
            command.Parameters.AddWithValue("@direccion", sucursal.Direccion);
            command.Parameters.AddWithValue("@codigopostal", sucursal.CodigoPostal);
            command.Parameters.AddWithValue("@activa", 1);
            command.ExecuteNonQuery();
            conn.Close();
        }

        public static Sucursal GetSucursalByCodigoPostal(int codPostal)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "Select * from dbo.Sucursales where sucu_codigoPostal=@codPostal";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@codPostal", codPostal);
            SqlDataReader reader = command.ExecuteReader();
            Sucursal sucursal= new Sucursal();
            if (reader.Read())
            {
                sucursal.Nombre = (string)reader["sucu_nombre"];
                sucursal.Direccion = (string)reader["sucu_direccion"];
                sucursal.CodigoPostal= Convert.ToInt32(reader["sucu_codigoPostal"]);
                sucursal.Activa = (bool)reader["sucu_activa"];
            }
            conn.Close();
            return sucursal;
        }
        public static List<Sucursal> GetAllSucursales()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "Select * from dbo.Sucursales";
            SqlCommand command = new SqlCommand(query, conn);
            List<Sucursal> sucursales= new List<Sucursal>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Sucursal sucursal = new Sucursal();
                    sucursal.Nombre = (string)reader["sucu_nombre"];
                    sucursal.Direccion = (string)reader["sucu_direccion"];
                    sucursal.CodigoPostal = Convert.ToInt32(reader["sucu_codigoPostal"]);
                    sucursal.Activa = (bool)reader["sucu_activa"];
                    sucursales.Add(sucursal);
                }
            }
            conn.Close();
            return sucursales;
        }
        public static List<Sucursal> GetSucursalByNombreDireccionCodigoPostal(string _nombre, string _direccion, string _codPostal)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            SqlCommand command = new SqlCommand();
            string query = "Select * "+
                                "from dbo.Sucursales " +
                                "where sucu_nombre like @nombre and "+
                                "sucu_direccion like @direccion";

            SqlParameter nombre = new SqlParameter("@nombre", DbType.String);
            nombre.Value = "%" + _nombre + "%";
            command.Parameters.Add(nombre);

            SqlParameter direccion = new SqlParameter("@direccion", DbType.String);
            direccion.Value = "%" + _direccion + "%";
            command.Parameters.Add(direccion);

            if (_codPostal != "")
            {
                query += " and sucu_codigoPostal = @codPostal";
                SqlParameter codPostal = new SqlParameter("@codPostal", DbType.Int32);

                
                if (!Regex.IsMatch(_codPostal, regexSoloNumeros))
                    throw new Exception("El código postal a buscar debe estar formado sólo por números.");
                codPostal.Value = _codPostal;
                command.Parameters.Add(codPostal);
            }

            command.Connection = conn;
            command.CommandText = query;
            List<Sucursal> sucursales= new List<Sucursal>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Sucursal sucursal = new Sucursal();
                    sucursal.Nombre = (string)reader["sucu_nombre"];
                    sucursal.Direccion = (string)reader["sucu_direccion"];
                    sucursal.CodigoPostal = Convert.ToInt32(reader["sucu_codigoPostal"]);
                    sucursal.Activa = (bool)reader["sucu_activa"];
                    sucursales.Add(sucursal);
                }
            }
            conn.Close();
            return sucursales;
        }

        public static void EditarSucursal(Sucursal sucursalAEditar, int codigoPostalOriginal)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            var query = "update dbo.Sucursales set sucu_nombre=@nombre,sucu_direccion=@direccion,sucu_codigoPostal=@codigoPostal,"
                + "sucu_activa=@activa where sucu_codigoPostal=@codigoPostalOriginal";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@codigoPostalOriginal", codigoPostalOriginal);
            command.Parameters.AddWithValue("@nombre", sucursalAEditar.Nombre);
            command.Parameters.AddWithValue("@direccion", sucursalAEditar.Direccion);
            command.Parameters.AddWithValue("@codigoPostal", sucursalAEditar.CodigoPostal);
            command.Parameters.AddWithValue("@activa", sucursalAEditar.Activa);
            command.ExecuteNonQuery();
            conn.Close();
        }
    }
}
