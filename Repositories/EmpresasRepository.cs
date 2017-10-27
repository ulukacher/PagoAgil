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
    public class EmpresasRepository
    {
        public static void DarDeBajaEmpresa(string cuit)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "update dbo.Empresas set empr_activa = 0 where empr_cuit = @cuit";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@cuit", cuit);
            command.ExecuteNonQuery();
            conn.Close();
        }
        public static void AgregarEmpresa(Empresa empresa)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "insert into dbo.Empresas values (@cuit,@nombre,@direccion,@rubro,@activa)";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@cuit", empresa.Cuit);
            command.Parameters.AddWithValue("@nombre", empresa.Nombre);
            command.Parameters.AddWithValue("@direccion", empresa.Direccion);
            command.Parameters.AddWithValue("@rubro", empresa.RubroId);
            command.Parameters.AddWithValue("@activa", 1);
            command.ExecuteNonQuery();
            conn.Close();
        }

        public static Empresa GetEmpresaByCUIT(string cuit)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "Select E.*, R.rubr_descripcion empr_rubro from dbo.Empresas E INNER JOIN dbo.Rubros R ON E.empr_rubro_id = R.rubr_id where empr_cuit=@cuit";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@cuit", cuit);
            SqlDataReader reader = command.ExecuteReader();
            Empresa empresa = new Empresa();
            if (reader.Read())
            {
                empresa.Nombre = (string)reader["empr_nombre"];
                empresa.Direccion = (string)reader["empr_direccion"];
                empresa.Cuit = (string)reader["empr_cuit"];
                empresa.RubroId = Convert.ToInt32(reader["empr_rubro_id"]);
                empresa.Rubro = (string) reader["empr_rubro"];
                empresa.Activa = (bool)reader["empr_activa"];
            }
            conn.Close();
            return empresa;
        }
        public static List<Empresa> GetAllEmpresas()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "Select E.empr_nombre, " +
                                "E.empr_cuit, " +
                                "E.empr_direccion, " +
                                "E.empr_activa, " +
                                "R.rubr_descripcion empr_rubro " +
                                "from dbo.Empresas E " +
                                "inner join dbo.Rubros R " +
                                "on E.empr_rubro_id = R.rubr_id";
            SqlCommand command = new SqlCommand(query, conn);
            List<Empresa> empresas = new List<Empresa>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Empresa empresa = new Empresa();
                    empresa.Nombre = (string)reader["empr_nombre"];
                    empresa.Direccion = (string)reader["empr_direccion"];
                    empresa.Cuit = (string)reader["empr_cuit"];
                    empresa.Rubro = (string)reader["empr_rubro"];
                    empresa.Activa = (bool)reader["empr_activa"];
                    empresas.Add(empresa);
                }
            }
            conn.Close();
            return empresas;
        }
        public static List<Empresa> GetEmpresasByNombreDireccionCuitRubro(string _nombre, string _direccion, string _cuit, int _rubroId)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            SqlCommand command = new SqlCommand();
            string query = "Select E.empr_nombre, " +
                                "E.empr_cuit, " +
                                "E.empr_direccion, " +
                                "E.empr_activa, " +
                                "R.rubr_descripcion empr_rubro " +
                                "from dbo.Empresas E " +
                                "inner join dbo.Rubros R " +
                                "on E.empr_rubro_id = R.rubr_id " +
                                "where empr_nombre like @nombre and " +
                                "empr_direccion like @direccion";

            SqlParameter nombre = new SqlParameter("@nombre", DbType.String);
            nombre.Value = "%" + _nombre + "%";
            command.Parameters.Add(nombre);

            SqlParameter direccion = new SqlParameter("@direccion", DbType.String);
            direccion.Value = "%" + _direccion + "%";
            command.Parameters.Add(direccion);

            if (_rubroId != 0)
            {
                query += " and R.rubr_id = @rubro";
                SqlParameter rubro = new SqlParameter("@rubro", DbType.Int32);
                rubro.Value = _rubroId;
                command.Parameters.Add(rubro);
            }

            if (_cuit != "")
            {
                query += " and empr_cuit like @cuit";
                SqlParameter cuit = new SqlParameter("@cuit", DbType.String);
                if (!Regex.IsMatch(_cuit, @"^\d{2}\-\d{8}\-\d{1}$"))
                    throw new Exception("El CUIT a buscar debe seguir el formato XX-XXXXXXXX-X, siendo X un número.");
                cuit.Value = _cuit;
                command.Parameters.Add(cuit);
            }

            command.Connection = conn;
            command.CommandText = query;
            List<Empresa> empresas = new List<Empresa>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Empresa empresa = new Empresa();
                    empresa.Nombre = (string)reader["empr_nombre"];
                    empresa.Direccion = (string)reader["empr_direccion"];
                    empresa.Cuit = (string)reader["empr_cuit"];
                    empresa.Rubro = (string)reader["empr_rubro"];
                    empresa.Activa = (bool)reader["empr_activa"];
                    empresas.Add(empresa);
                }
            }
            conn.Close();
            return empresas;
        }
        public static void EditarEmpresa(Empresa empresaAEditar, string cuitOriginal)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            var query = "update dbo.Empresas set empr_nombre=@nombre,empr_direccion=@direccion,empr_cuit=@cuit,"
                + "empr_rubro_id=@rubro,empr_activa=@activa where empr_cuit=@cuitOriginal";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@cuitOriginal", cuitOriginal);
            command.Parameters.AddWithValue("@nombre", empresaAEditar.Nombre);
            command.Parameters.AddWithValue("@direccion", empresaAEditar.Direccion);
            command.Parameters.AddWithValue("@cuit", empresaAEditar.Cuit);
            command.Parameters.AddWithValue("@rubro", empresaAEditar.RubroId);
            command.Parameters.AddWithValue("@activa", empresaAEditar.Activa);
            command.ExecuteNonQuery();
            conn.Close();
        }
    }
}
