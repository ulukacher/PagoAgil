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
        public static List<Factura> GetFacturasPendientesDeRendicionByEmpresa(string cuitEmpresa)
        {
            //Traigo las facturas pendientes de rendicion para esta empresa, en el mes actual
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            List<Factura> facturas = new List<Factura>();
            string query = "select factura_nro,factura_empresaCuit,factura_clienteDNI,factura_fecha,factura_estado,factura_fechaVencimiento," +
                "(select sum(itemFac_monto) from dbo.ItemsFacturas where itemFac_facturaNro = factura_nro) as monto" +
                " from dbo.facturas " +
                "where factura_empresaCuit = @cuit and factura_estado = @estado and year(factura_fecha) =@anio and month(factura_fecha) = @mes";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@cuit", cuitEmpresa);
            command.Parameters.AddWithValue("@estado", EstadoFactura.Pagada);
            command.Parameters.AddWithValue("@anio", DateTime.Now.Year);
            command.Parameters.AddWithValue("@mes", DateTime.Now.Month);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Factura factura = new Factura();
                    factura.Nro = (decimal)reader["factura_nro"];
                    factura.EmpresaCuit = (string)reader["factura_empresaCuit"];
                    factura.ClienteDNI = (decimal)reader["factura_clienteDNI"];
                    factura.Fecha = (DateTime)reader["factura_fecha"];
                    factura.Estado = (int)reader["factura_estado"];
                    factura.FechaVencimiento = (DateTime)reader["factura_fechaVencimiento"];
                    factura.Monto = (decimal)reader["monto"];

                    facturas.Add(factura);
                }
            }
            conn.Close();
            return facturas;

        }
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

        public static Factura GetFacturaByNro(decimal nroFactura)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);

            conn.Open();

            string query = "SELECT * FROM dbo.Facturas WHERE factura_nro = @nroFactura";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@nroFactura", nroFactura);

            SqlDataReader reader = command.ExecuteReader();

            Factura factura = new Factura();

            if (reader.Read())
            {
                factura.Nro = (decimal)reader["factura_nro"];
                factura.EmpresaCuit = (string)reader["factura_empresaCuit"];
                factura.ClienteDNI = (decimal)reader["factura_clienteDNI"];
                factura.Fecha = (DateTime)reader["factura_fecha"];
                factura.Estado = (int)reader["factura_estado"];
                factura.FechaVencimiento = (DateTime)reader["factura_fechaVencimiento"];
            }

            conn.Close();

            return factura;
        }

        public static List<Factura> GetFacturasByFiltros(string _numero, string _empresaCuit, string _cliente, int _estado)
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

                if (_empresaCuit != "")
                {
                    query += " AND empr_cuit = @empresaCuit";

                    SqlParameter empresaCuit = new SqlParameter("@empresaCuit", DbType.String);
                    empresaCuit.Value = _empresaCuit;
                    command.Parameters.Add(empresaCuit);
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
            else if (_numero == "" && _empresaCuit != "")
            {
                query += " WHERE empr_cuit = @empresaCuit";

                SqlParameter empresaCuit = new SqlParameter("@empresaCuit", DbType.String);
                empresaCuit.Value = _empresaCuit;
                command.Parameters.Add(empresaCuit);

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
            else if (_numero == "" && _empresaCuit == "" && _cliente != "")
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
            else if (_numero == "" && _empresaCuit == "" && _cliente == "" /*&& _fecha != ""*/)
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
            else if (_numero == "" && _empresaCuit == "" && _cliente == "" /*&& _fecha == ""*/ && _estado != -1)
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

        public static void EditarFactura(Factura facturaAEditar, decimal dniOriginal)
        {
            /*SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
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
            conn.Close();*/
        }
        
    }
}