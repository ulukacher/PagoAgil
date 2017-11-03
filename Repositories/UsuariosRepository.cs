using PagoAgilFrba.Classes;
using PagoAgilFrba.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PagoAgilFrba.Repositories
{
    public class UsuariosRepository
    {
        public static void CambiarHabilitacion(string username)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "UPDATE LOS_MANTECOSOS.usuarios set usuario_activo = (CASE usuario_activo WHEN 1 THEN 0 ELSE 1 END) "
                +" where usuario_nombre = @nombre";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@nombre", username);
            command.ExecuteNonQuery();
            conn.Close();
            
        }
        public static List<Usuario> GetAll()
        {
            List<Usuario> usuarios = new List<Usuario>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "select * from LOS_MANTECOSOS.usuarios";
            SqlCommand command = new SqlCommand(query, conn);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Usuario us = new Usuario();
                    us.Activo = (bool)reader["usuario_activo"];
                    us.Username = (string)reader["usuario_nombre"];
                    usuarios.Add(us);
                }
            }
            conn.Close();
            return usuarios;
        }
        public static bool ExisteUsuarioConEseUsername(string username) 
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "select count(*) from LOS_MANTECOSOS.usuarios where usuario_nombre = @username";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@username", username);
            int cantUsuarios = (int)command.ExecuteScalar();           
            conn.Close();
            return cantUsuarios > 0;
        }
        public string SHA256Encrypt(string input)
        {
            SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();

            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashedBytes = provider.ComputeHash(inputBytes);

            StringBuilder output = new StringBuilder();

            for (int i = 0; i < hashedBytes.Length; i++)
                output.Append(hashedBytes[i].ToString("x2").ToLower());

            return output.ToString();
        }
        public static bool Login(string username, string password) 
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "select count(*) from LOS_MANTECOSOS.usuarios where usuario_nombre = @username and usuario_password = @password";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@username", username);
            byte[] passwordHasheada = EncryptHelper.SHA256Encrypt(password);
            command.Parameters.AddWithValue("@password", passwordHasheada);
            int CantUsuariosConEseMailYContrasenia = (int)command.ExecuteScalar();

            //Intentos fallidos
            string queryLimpioIntentos;             
            if (CantUsuariosConEseMailYContrasenia > 0)
            {
                //Limpio los intentos fallidos del usuario
                queryLimpioIntentos = "update LOS_MANTECOSOS.usuarios set usuario_cantidadIntentosFallidos = 0 where usuario_nombre =@username";
            }
            else
            {
                queryLimpioIntentos = "update LOS_MANTECOSOS.usuarios set usuario_cantidadIntentosFallidos =  usuario_cantidadIntentosFallidos + 1 where usuario_nombre =@username";

            }
            SqlCommand comandoIntentos = new SqlCommand(queryLimpioIntentos, conn);
            comandoIntentos.Parameters.AddWithValue("@username", username);
            comandoIntentos.ExecuteNonQuery();

            conn.Close();
            return CantUsuariosConEseMailYContrasenia > 0;

        }
    }
}
