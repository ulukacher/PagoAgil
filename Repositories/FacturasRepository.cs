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
    public class FacturasRepository
    {
        public static void EliminarFactura(decimal numeroFactura)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();

            string query = "DELETE FROM dbo.Facturas WHERE factura_nro = @numeroFactura";
            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.AddWithValue("@numeroFactura", numeroFactura);
            command.ExecuteNonQuery();

            conn.Close();
        }

        public static void AgregarFactura(Factura factura)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);

            conn.Open();

            string query = "INSERT INTO dbo.Facturas VALUES (@nro, @cuit, @dni, @fecha, @estado, @fechaVencimiento)";

            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.AddWithValue("@nro", factura.Nro);
            command.Parameters.AddWithValue("@cuit", factura.EmpresaCuit);
            command.Parameters.AddWithValue("@dni", factura.ClienteDNI);
            command.Parameters.AddWithValue("@fecha", factura.Fecha);
            command.Parameters.AddWithValue("@estado", factura.Estado);
            command.Parameters.AddWithValue("@fechaVencimiento", factura.FechaVencimiento);

            command.ExecuteNonQuery();

            conn.Close();
        }

        public static void AgregarItemsFactura(List<ItemFactura> listaItems)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);

            conn.Open();

            foreach (ItemFactura itemFactura in listaItems)
            {
                string query = "INSERT INTO dbo.ItemsFacturas VALUES (@nroItem, @nroFactura, @monto, @cantidad, @detalle)";

                SqlCommand command = new SqlCommand(query, conn);

                command.Parameters.AddWithValue("@nroItem", itemFactura.NroItem);
                command.Parameters.AddWithValue("@nroFactura", itemFactura.NroFactura);
                command.Parameters.AddWithValue("@monto", itemFactura.Monto);
                command.Parameters.AddWithValue("@cantidad", itemFactura.Cantidad);
                command.Parameters.AddWithValue("@detalle", itemFactura.Detalle);

                command.ExecuteNonQuery();
            }

            conn.Close();
        }

        public static int GetCantidadClientesConEseMail(string mail)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            var queryMailUnico = "select count(*) from dbo.Clientes where clie_mail = @mailIngresado";
            SqlCommand command = new SqlCommand(queryMailUnico, conn);
            command.Parameters.AddWithValue("@mailIngresado", mail);
            int cant = (int)command.ExecuteScalar();
            conn.Close();
            return cant;
        }

        public static Factura GetFacturaByNro(decimal nroFactura)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);

            conn.Open();

            string query = "SELECT * FROM dbo.Facturas WHERE factura_nro = @nroFactura";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@nroFactura", nroFactura);

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
            Factura f = new Factura();
            return f;
        }

        public static List<Cliente> GetAllClientes()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "select * from dbo.clientes";
            SqlCommand command = new SqlCommand(query, conn);
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

        public static List<Factura> GetFacturasByFiltros(string _numero, string _empresaNombre, string _cliente, int _estado)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);

            conn.Open();

            SqlCommand command = new SqlCommand();

            string query = "SELECT factura_nro, empr_cuit, factura_clienteDNI, factura_fecha, factura_estado, factura_fechaVencimiento"
                         + " FROM dbo.Facturas INNER JOIN dbo.Empresas ON factura_empresaCuit = empr_cuit";

            if (_numero != "")
            {
                query += " WHERE factura_nro = @numero";

                SqlParameter numero = new SqlParameter("@numero", DbType.Int32);
                numero.Value = _numero;
                command.Parameters.Add(numero);

                if (_empresaNombre != "")
                {
                    query += " AND empr_nombre = @empresaNombre";

                    SqlParameter empresaNombre = new SqlParameter("@empresaNombre", DbType.String);
                    empresaNombre.Value = _empresaNombre;
                    command.Parameters.Add(empresaNombre);
                }

                if (_cliente != "")
                {
                    query += " AND factura_clienteDNI = @cliente";

                    SqlParameter cliente = new SqlParameter("@cliente", DbType.Int32);
                    cliente.Value = _cliente;
                    command.Parameters.Add(cliente);
                }

                /*if (_fecha != "")
                {
                    query += " AND factura_fecha = @fecha";

                    SqlParameter fecha = new SqlParameter("@fecha", DbType.DateTime);
                    fecha.Value = _fecha;
                    command.Parameters.Add(fecha);
                }*/

                if (_estado != -1)
                {
                    query += " AND factura_estado = " + _estado.ToString();

                    SqlParameter estado = new SqlParameter("@estado", DbType.Int32);
                    estado.Value = _estado;
                    command.Parameters.Add(estado);
                }

                /*if (_fechaVencimiento != "")
                {
                    query += " AND factura_fechaVencimiento = @fechaVencimiento";

                    SqlParameter fechaVencimiento = new SqlParameter("@fechaVencimiento", DbType.DateTime);
                    fechaVencimiento.Value = _fechaVencimiento;
                    command.Parameters.Add(fechaVencimiento);
                }*/
            }
            else if (_numero == "" && _empresaNombre != "")
            {
                query += " WHERE empr_nombre = @empresaNombre";

                SqlParameter empresaNombre = new SqlParameter("@empresaNombre", DbType.String);
                empresaNombre.Value = _empresaNombre;
                command.Parameters.Add(empresaNombre);

                if (_cliente != "")
                {
                    query += " AND factura_clienteDNI = @cliente";

                    SqlParameter cliente = new SqlParameter("@cliente", DbType.Int32);
                    cliente.Value = _cliente;
                    command.Parameters.Add(cliente);
                }

                /*if (_fecha != "")
                {
                    query += " AND factura_fecha = @fecha";

                    SqlParameter fecha = new SqlParameter("@fecha", DbType.DateTime);
                    fecha.Value = _fecha;
                    command.Parameters.Add(fecha);
                }*/

                if (_estado != -1)
                {
                    query += " AND factura_estado = " + _estado.ToString();

                    SqlParameter estado = new SqlParameter("@estado", DbType.Int32);
                    estado.Value = _estado;
                    command.Parameters.Add(estado);
                }

                /*if (_fechaVencimiento != "")
                {
                    query += " AND factura_fechaVencimiento = @fechaVencimiento";

                    SqlParameter fechaVencimiento = new SqlParameter("@fechaVencimiento", DbType.DateTime);
                    fechaVencimiento.Value = _fechaVencimiento;
                    command.Parameters.Add(fechaVencimiento);
                }*/
            }
            else if (_numero == "" && _empresaNombre == "" && _cliente != "")
            {
                query += " WHERE factura_clienteDNI = @cliente";

                SqlParameter cliente = new SqlParameter("@cliente", DbType.Int32);
                cliente.Value = _cliente;
                command.Parameters.Add(cliente);

                /*if (_fecha != "")
                {
                    query += " AND factura_fecha = @fecha";

                    SqlParameter fecha = new SqlParameter("@fecha", DbType.DateTime);
                    fecha.Value = _fecha;
                    command.Parameters.Add(fecha);
                }*/

                if (_estado != -1)
                {
                    query += " AND factura_estado = " + _estado.ToString();

                    SqlParameter estado = new SqlParameter("@estado", DbType.Int32);
                    estado.Value = _estado;
                    command.Parameters.Add(estado);
                }

                /*if (_fechaVencimiento != "")
                {
                    query += " AND factura_fechaVencimiento = @fechaVencimiento";

                    SqlParameter fechaVencimiento = new SqlParameter("@fechaVencimiento", DbType.DateTime);
                    fechaVencimiento.Value = _fechaVencimiento;
                    command.Parameters.Add(fechaVencimiento);
                }*/
            }
            else if (_numero == "" && _empresaNombre == "" && _cliente == "" /*&& _fecha != ""*/)
            {
                /*query += " WHERE factura_fecha = @fecha";

                SqlParameter fecha = new SqlParameter("@fecha", DbType.DateTime);
                fecha.Value = _fecha;
                command.Parameters.Add(fecha);*/

                if (_estado != -1)
                {
                    query += " AND factura_estado = " + _estado.ToString();

                    SqlParameter estado = new SqlParameter("@estado", DbType.Int32);
                    estado.Value = _estado;
                    command.Parameters.Add(estado);
                }

                /*if (_fechaVencimiento != "")
                {
                    query += " AND factura_fechaVencimiento = @fechaVencimiento";

                    SqlParameter fechaVencimiento = new SqlParameter("@fechaVencimiento", DbType.DateTime);
                    fechaVencimiento.Value = _fechaVencimiento;
                    command.Parameters.Add(fechaVencimiento);
                }*/
            }
            else if (_numero == "" && _empresaNombre == "" && _cliente == "" /*&& _fecha == ""*/ && _estado != -1)
            {
                query += " AND factura_estado = " + _estado.ToString();

                SqlParameter estado = new SqlParameter("@estado", DbType.Int32);
                estado.Value = _estado;
                command.Parameters.Add(estado);

                /*if (_fechaVencimiento != "")
                {
                    query += " AND factura_fechaVencimiento = @fechaVencimiento";

                    SqlParameter fechaVencimiento = new SqlParameter("@fechaVencimiento", DbType.DateTime);
                    fechaVencimiento.Value = _fechaVencimiento;
                    command.Parameters.Add(fechaVencimiento);
                }*/
            }
            /*else if (_numero == "" && _empresaNombre == "" && _cliente == "" && _fecha == "" && _estado == "" && _fechaVencimiento != "")
            {
                query += " WHERE factura_fechaVencimiento = @fechaVencimiento";

                SqlParameter fechaVencimiento = new SqlParameter("@fechaVencimiento", DbType.DateTime);
                fechaVencimiento.Value = _fechaVencimiento;
                command.Parameters.Add(fechaVencimiento);
            }*/

            command.Connection = conn;
            command.CommandText = query;

            List<Factura> facturas = new List<Factura>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Factura factura = new Factura();

                    factura.Nro = (decimal)reader["factura_nro"];
                    factura.EmpresaCuit = (string)reader["empr_cuit"];
                    factura.ClienteDNI = (decimal)reader["factura_clienteDNI"];
                    factura.Fecha = (DateTime)reader["factura_fecha"];
                    factura.Estado = (int)reader["factura_estado"];
                    factura.FechaVencimiento = (DateTime)reader["factura_fechaVencimiento"];

                    facturas.Add(factura);
                }
            }

            conn.Close();

            return facturas;
        }
        public static void EditarCliente(Cliente clienteAEditar, decimal dniOriginal)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            var query = "update dbo.clientes set clie_nombre=@nombre,clie_apellido = @apellido,clie_dni=@dni,"
                + "clie_direccion=@direccion,clie_mail=@mail,clie_codigoPostal = @codigoPostal,"
                + "clie_telefono=@telefono,clie_fechaNacimiento=@fechaNacimiento,clie_activo=@activo where clie_dni = @dniOriginal";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@dniOriginal", dniOriginal);
            command.Parameters.AddWithValue("@nombre", clienteAEditar.Nombre);
            command.Parameters.AddWithValue("@apellido", clienteAEditar.Apellido);
            command.Parameters.AddWithValue("@dni", clienteAEditar.DNI);
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
            var queryMailUnico = "select count(*) from dbo.Clientes where clie_mail = @mailIngresado and clie_dni <> @dniOriginal";
            SqlCommand command = new SqlCommand(queryMailUnico, conn);
            command.Parameters.AddWithValue("@mailIngresado", mail);
            command.Parameters.AddWithValue("@dniOriginal", dni);
            int cantMailsIguales = (int)command.ExecuteScalar();
            conn.Close();
            return cantMailsIguales;
        }
    }
}