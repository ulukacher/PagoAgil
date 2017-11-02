using PagoAgilFrba.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagoAgilFrba.Repositories
{
    public class ClientesRepository
    {
        public static void DarDeBajaCliente(decimal dni)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "update LOS_MANTECOSOS.clientes set clie_activo = 0 where clie_dni = @dni";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@dni", dni);
            command.ExecuteNonQuery();
            conn.Close();
        }
        public static void AgregarCliente(Cliente cliente) 
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "insert into LOS_MANTECOSOS.Clientes values (@dni,@nombre,@apellido,@fechaNacimiento,@mail,@direccion,@codigoPostal,@telefono,1)";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@nombre", cliente.Nombre);
            command.Parameters.AddWithValue("@apellido", cliente.Apellido);
            command.Parameters.AddWithValue("@dni",cliente.DNI);
            command.Parameters.AddWithValue("@mail", cliente.Mail);
            command.Parameters.AddWithValue("@telefono", cliente.Telefono);
            command.Parameters.AddWithValue("@direccion", cliente.Direccion);
            command.Parameters.AddWithValue("@codigoPostal", cliente.CodigoPostal);
            command.Parameters.AddWithValue("@fechaNacimiento", cliente.FechaNacimiento);
            command.ExecuteNonQuery();
            conn.Close();
        }
        public static int GetCantidadClientesConEseMail(string mail) 
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            var queryMailUnico = "select count(*) from LOS_MANTECOSOS.Clientes where clie_mail = @mailIngresado";
            SqlCommand command = new SqlCommand(queryMailUnico, conn);
            command.Parameters.AddWithValue("@mailIngresado", mail);
            int cant = (int)command.ExecuteScalar();
            conn.Close();
            return cant;
        }
        public static Cliente GetClienteByDNI(decimal dni)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "select * from LOS_MANTECOSOS.clientes where clie_dni = @dni";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@dni", dni);
            SqlDataReader reader = command.ExecuteReader();
            Cliente cliente = new Cliente();
            if (reader.Read())
            {
                cliente.Nombre = (string)reader["clie_nombre"];
                cliente.Apellido = (string)reader["clie_apellido"];
                cliente.DNI = (decimal)reader["clie_dni"];
                cliente.Direccion = (string)reader["clie_direccion"];
                cliente.Mail = (string)reader["clie_mail"];
                cliente.FechaNacimiento = (DateTime)reader["clie_fechaNacimiento"];
                cliente.CodigoPostal = (string)reader["clie_codigoPostal"];
                cliente.Telefono = (string)reader["clie_telefono"];
                cliente.Activo = (bool)reader["clie_activo"];            
            }
            conn.Close();
            return cliente;         
        }
        public static List<Cliente> GetAllClientes() 
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "select * from LOS_MANTECOSOS.clientes";
            SqlCommand command = new SqlCommand(query, conn);
            List<Cliente> clientes = new List<Cliente>();
            using (SqlDataReader reader = command.ExecuteReader()) 
            {
                while (reader.Read()) {
                    Cliente cliente = new Cliente();
                    cliente.Nombre = (string)reader["clie_nombre"];
                    cliente.Apellido = (string)reader["clie_apellido"];
                    cliente.DNI = (decimal)reader["clie_dni"];
                    cliente.Direccion = (string)reader["clie_direccion"];
                    cliente.Mail = (string)reader["clie_mail"];
                    cliente.FechaNacimiento = (DateTime)reader["clie_fechaNacimiento"];
                    cliente.CodigoPostal = (string)reader["clie_codigoPostal"];
                    cliente.Telefono = (string)reader["clie_telefono"];
                    cliente.Activo = (bool)reader["clie_activo"];
                    clientes.Add(cliente);
                }            
            }
            conn.Close();
            return clientes;  
        }
        public static List<Cliente> GetClientesByNombreApellidoDni(string _nombre, string _apellido, string _dni) 
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            SqlCommand command = new SqlCommand();
            string query = "Select * from LOS_MANTECOSOS.Clientes where clie_nombre like @nombre and clie_apellido like @apellido";
            SqlParameter nombre = new SqlParameter("@nombre", DbType.String);
            nombre.Value = "%" + _nombre + "%";
            command.Parameters.Add(nombre);

            SqlParameter apellido = new SqlParameter("@apellido", DbType.String);
            apellido.Value = "%" + _apellido + "%";
            command.Parameters.Add(apellido);

            if (_dni != "")
            {
                query += " and clie_dni = @dni";
                SqlParameter dniParameter = new SqlParameter("@dni", DbType.Int32);
                int dni;
                if (!int.TryParse(_dni, out dni))
                    throw new Exception("El DNI a buscar debe ser un numero, sin puntos ni comas");
                dniParameter.Value = dni;
                command.Parameters.Add(dniParameter);
            }
            command.Connection = conn;
            command.CommandText = query;
            List<Cliente> clientes = new List<Cliente>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Cliente cliente = new Cliente();
                    cliente.Nombre = (string)reader["clie_nombre"];
                    cliente.Apellido = (string)reader["clie_apellido"];
                    cliente.DNI = (decimal)reader["clie_dni"];
                    cliente.Direccion = (string)reader["clie_direccion"];
                    cliente.Mail = (string)reader["clie_mail"];
                    cliente.FechaNacimiento = (DateTime)reader["clie_fechaNacimiento"];
                    cliente.CodigoPostal = (string)reader["clie_codigoPostal"];
                    cliente.Telefono = (string)reader["clie_telefono"];
                    cliente.Activo = (bool)reader["clie_activo"];
                    clientes.Add(cliente);
                }
            }
            conn.Close();
            return clientes; 
        }
        public static void EditarCliente(Cliente clienteAEditar, decimal dniOriginal) {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            var query = "update LOS_MANTECOSOS.clientes set clie_nombre=@nombre,clie_apellido = @apellido,clie_dni=@dni,"
                + "clie_direccion=@direccion,clie_mail=@mail,clie_codigoPostal = @codigoPostal,"
                + "clie_telefono=@telefono,clie_fechaNacimiento=@fechaNacimiento,clie_activo=@activo where clie_dni = @dniOriginal";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@dniOriginal", dniOriginal);
            command.Parameters.AddWithValue("@nombre", clienteAEditar.Nombre);
            command.Parameters.AddWithValue("@apellido", clienteAEditar.Apellido);
            command.Parameters.AddWithValue("@dni",clienteAEditar.DNI);
            command.Parameters.AddWithValue("@mail", clienteAEditar.Mail);
            command.Parameters.AddWithValue("@telefono", clienteAEditar.Telefono);
            command.Parameters.AddWithValue("@direccion", clienteAEditar.Direccion);
            command.Parameters.AddWithValue("@codigoPostal", clienteAEditar.CodigoPostal);
            command.Parameters.AddWithValue("@fechaNacimiento", clienteAEditar.FechaNacimiento);
            command.Parameters.AddWithValue("@activo", clienteAEditar.Activo);
            command.ExecuteNonQuery();
            conn.Close();
        }

        public static int GetCantClientesDistintosConEseMail(string mail, decimal dni)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            var queryMailUnico = "select count(*) from LOS_MANTECOSOS.Clientes where clie_mail = @mailIngresado and clie_dni <> @dniOriginal";
            SqlCommand command = new SqlCommand(queryMailUnico, conn);
            command.Parameters.AddWithValue("@mailIngresado", mail);
            command.Parameters.AddWithValue("@dniOriginal", dni);
            int cantMailsIguales = (int)command.ExecuteScalar();
            conn.Close();
            return cantMailsIguales;
        }
    }
}
