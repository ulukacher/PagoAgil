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

            string query = "select distinct factura_nro,factura_empresaCuit,factura_clienteDNI,factura_fecha,factura_estado,factura_fechaVencimiento," +
                "(select sum(itemFac_monto) from LOS_MANTECOSOS.ItemsFacturas where itemFac_facturaNro = factura_nro) as monto" +
                " from LOS_MANTECOSOS.facturas INNER JOIN LOS_MANTECOSOS.ItemsPagos ON factura_nro = itemPago_facturaNro INNER JOIN LOS_MANTECOSOS.Pagos ON itemPago_pagoNro = pago_nro" +
                " where factura_empresaCuit = @cuit and factura_estado = @estado and year(pago_fecha) = @anio and month(pago_fecha) = @mes";

            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.AddWithValue("@cuit", cuitEmpresa);
            command.Parameters.AddWithValue("@estado", EstadoFactura.Pagada);
            command.Parameters.AddWithValue("@anio", ConfiguracionFecha.FechaSistema.Year);
            command.Parameters.AddWithValue("@mes", ConfiguracionFecha.FechaSistema.Month);

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

            string queryItems = "DELETE FROM LOS_MANTECOSOS.ItemsFacturas WHERE itemFac_facturaNro = @numeroFactura";
            SqlCommand commandItems = new SqlCommand(queryItems, conn);

            commandItems.Parameters.AddWithValue("@numeroFactura", numeroFactura);
            commandItems.ExecuteNonQuery();

            string query = "DELETE FROM LOS_MANTECOSOS.Facturas WHERE factura_nro = @numeroFactura";
            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.AddWithValue("@numeroFactura", numeroFactura);
            command.ExecuteNonQuery();

            conn.Close();
        }

        public static void AgregarFactura(Factura factura, List<ItemFactura> listaItems)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);

            conn.Open();

            using (var tx = conn.BeginTransaction())
            {
                try
                {
                    string query = "INSERT INTO LOS_MANTECOSOS.Facturas VALUES (@nro, @cuit, @dni, @fecha, @estado, @fechaVencimiento)";

                    SqlCommand command = new SqlCommand(query, conn, tx);

                    command.Parameters.AddWithValue("@nro", factura.Nro);
                    command.Parameters.AddWithValue("@cuit", factura.EmpresaCuit);
                    command.Parameters.AddWithValue("@dni", factura.ClienteDNI);
                    command.Parameters.AddWithValue("@fecha", factura.Fecha);
                    command.Parameters.AddWithValue("@estado", factura.Estado);
                    command.Parameters.AddWithValue("@fechaVencimiento", factura.FechaVencimiento);

                    command.ExecuteNonQuery();

                    foreach (ItemFactura itemFactura in listaItems)
                    {
                        string queryItems = "INSERT INTO LOS_MANTECOSOS.ItemsFacturas VALUES (@nroItem, @nroFactura, @monto, @cantidad, @detalle)";

                        SqlCommand commandItems = new SqlCommand(queryItems, conn, tx);

                        commandItems.Parameters.AddWithValue("@nroItem", itemFactura.NroItem);
                        commandItems.Parameters.AddWithValue("@nroFactura", itemFactura.NroFactura);
                        commandItems.Parameters.AddWithValue("@monto", itemFactura.Monto);
                        commandItems.Parameters.AddWithValue("@cantidad", itemFactura.Cantidad);
                        commandItems.Parameters.AddWithValue("@detalle", itemFactura.Detalle);

                        commandItems.ExecuteNonQuery();
                    }

                    tx.Commit();

                    conn.Close();
                }
                catch(SqlException sqlExc)
                {
                    tx.Rollback();

                    conn.Close();

                    throw sqlExc;
                }
                catch (Exception exc)
                {
                    tx.Rollback();

                    conn.Close();

                    throw new Exception(exc.Message);
                }
            }
        }

        public static void EditarFactura(Factura factura, List<ItemFactura> listaItems, decimal nroFacturaOriginal)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);

            conn.Open();

            using (var tx = conn.BeginTransaction())
            {
                try
                {
                    string queryItemsEliminar = "DELETE FROM LOS_MANTECOSOS.ItemsFacturas WHERE itemFac_facturaNro = @nroFacturaOriginal";

                    SqlCommand commandItemsEliminar = new SqlCommand(queryItemsEliminar, conn, tx);

                    commandItemsEliminar.Parameters.AddWithValue("@nroFacturaOriginal", nroFacturaOriginal);

                    commandItemsEliminar.ExecuteNonQuery();

                    foreach (ItemFactura itemFactura in listaItems)
                    {
                        string queryItems = "INSERT INTO LOS_MANTECOSOS.ItemsFacturas VALUES (@nroItem, @nroFactura, @monto, @cantidad, @detalle)";

                        SqlCommand commandItems = new SqlCommand(queryItems, conn, tx);

                        commandItems.Parameters.AddWithValue("@nroItem", itemFactura.NroItem);
                        commandItems.Parameters.AddWithValue("@nroFactura", itemFactura.NroFactura);
                        commandItems.Parameters.AddWithValue("@monto", itemFactura.Monto);
                        commandItems.Parameters.AddWithValue("@cantidad", itemFactura.Cantidad);
                        commandItems.Parameters.AddWithValue("@detalle", itemFactura.Detalle);

                        commandItems.ExecuteNonQuery();
                    }

                    string query = "UPDATE LOS_MANTECOSOS.Facturas SET factura_nro = @nro, factura_empresaCuit = @cuit, factura_clienteDNI = @dni, factura_fecha = @fecha, factura_estado = @estado, factura_fechaVencimiento = @fechaVencimiento"
                                 + " WHERE factura_nro = @nroFacturaOriginal";

                    SqlCommand command = new SqlCommand(query, conn, tx);

                    command.Parameters.AddWithValue("@nro", factura.Nro);
                    command.Parameters.AddWithValue("@cuit", factura.EmpresaCuit);
                    command.Parameters.AddWithValue("@dni", factura.ClienteDNI);
                    command.Parameters.AddWithValue("@fecha", factura.Fecha);
                    command.Parameters.AddWithValue("@estado", factura.Estado);
                    command.Parameters.AddWithValue("@fechaVencimiento", factura.FechaVencimiento);
                    command.Parameters.AddWithValue("@nroFacturaOriginal", nroFacturaOriginal);

                    command.ExecuteNonQuery();

                    tx.Commit();

                    conn.Close();
                }
                catch (Exception exc)
                {
                    tx.Rollback();

                    conn.Close();

                    throw new Exception(exc.Message);
                }
            }
        }

        public static Factura GetFacturaByNro(decimal nroFactura)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);

            conn.Open();

            string query = "SELECT * FROM LOS_MANTECOSOS.Facturas WHERE factura_nro = @nroFactura";

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
                         + " FROM LOS_MANTECOSOS.Facturas INNER JOIN LOS_MANTECOSOS.Empresas ON factura_empresaCuit = empr_cuit";

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

                if (_estado != -1)
                {
                    query += " AND factura_estado = " + _estado.ToString();

                    SqlParameter estado = new SqlParameter("@estado", DbType.Int32);
                    estado.Value = _estado;
                    command.Parameters.Add(estado);
                }
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

                if (_estado != -1)
                {
                    query += " AND factura_estado = " + _estado.ToString();

                    SqlParameter estado = new SqlParameter("@estado", DbType.Int32);
                    estado.Value = _estado;
                    command.Parameters.Add(estado);
                }
            }
            else if (_numero == "" && _empresaCuit == "" && _cliente != "")
            {
                query += " WHERE factura_clienteDNI = @cliente";

                SqlParameter cliente = new SqlParameter("@cliente", DbType.Int32);
                cliente.Value = _cliente;
                command.Parameters.Add(cliente);

                if (_estado != -1)
                {
                    query += " AND factura_estado = " + _estado.ToString();

                    SqlParameter estado = new SqlParameter("@estado", DbType.Int32);
                    estado.Value = _estado;
                    command.Parameters.Add(estado);
                }
            }
            else if (_numero == "" && _empresaCuit == "" && _cliente == "")
            {
                if (_estado != -1)
                {
                    query += " AND factura_estado = " + _estado.ToString();

                    SqlParameter estado = new SqlParameter("@estado", DbType.Int32);
                    estado.Value = _estado;
                    command.Parameters.Add(estado);
                }
            }
            else if (_numero == "" && _empresaCuit == "" && _cliente == "" && _estado != -1)
            {
                query += " AND factura_estado = " + _estado.ToString();

                SqlParameter estado = new SqlParameter("@estado", DbType.Int32);
                estado.Value = _estado;
                command.Parameters.Add(estado);
            }

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

        public static List<ItemFactura> GetAllItemsFactura(decimal nroFactura)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);

            conn.Open();

            string query = "SELECT * FROM LOS_MANTECOSOS.ItemsFacturas WHERE itemFac_facturaNro = @nroFactura";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@nroFactura", nroFactura);

            List<ItemFactura> itemsFactura = new List<ItemFactura>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    ItemFactura itemFactura = new ItemFactura();

                    itemFactura.NroItem = (decimal)reader["itemFac_nroItem"];
                    itemFactura.NroFactura = (decimal)reader["itemFac_facturaNro"];
                    itemFactura.Monto = (decimal)reader["itemFac_monto"];
                    itemFactura.Cantidad = (decimal)reader["itemFac_cantidad"];
                    itemFactura.Detalle = (string)reader["itemFac_detalle"];

                    itemsFactura.Add(itemFactura);
                }
            }

            return itemsFactura;
        }

        public static bool FacturaEstaPaga(decimal nroFactura)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();

            var query = "SELECT COUNT(*) FROM LOS_MANTECOSOS.Facturas WHERE factura_nro = @nroFactura AND factura_estado = 1";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@nroFactura", nroFactura);

            int cant = (int)command.ExecuteScalar();

            conn.Close();

            return cant > 0;
        }

        public static bool FacturaEstaRendida(decimal nroFactura)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();

            var query = "SELECT COUNT(*) FROM LOS_MANTECOSOS.Facturas WHERE factura_nro = @nroFactura AND factura_estado = 2";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@nroFactura", nroFactura);

            int cant = (int)command.ExecuteScalar();

            conn.Close();

            return cant > 0;
        }

        public static bool FacturaEsDeCliente(decimal nroFactura, decimal clienteDNI)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();

            var query = "SELECT COUNT(*) FROM LOS_MANTECOSOS.Facturas WHERE factura_nro = @nroFactura AND factura_clienteDNI = @clienteDNI";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@nroFactura", nroFactura);
            command.Parameters.AddWithValue("@clienteDNI", clienteDNI);

            int cant = (int)command.ExecuteScalar();

            conn.Close();

            return cant > 0;
        }

        public static decimal ImporteFactura(decimal nroFactura)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();

            var query = "SELECT SUM(itemFac_monto * itemFac_cantidad) FROM LOS_MANTECOSOS.ItemsFacturas WHERE itemFac_facturaNro = @nroFactura";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@nroFactura", nroFactura);

            decimal importe = (decimal)command.ExecuteScalar();

            conn.Close();

            return importe;
        }

        public static DateTime FechaDeVencimientoFactura(decimal nroFactura)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();

            var query = "SELECT factura_fechaVencimiento FROM LOS_MANTECOSOS.Facturas WHERE factura_nro = @nroFactura";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@nroFactura", nroFactura);

            DateTime fechaVencimiento = (DateTime)command.ExecuteScalar();

            conn.Close();

            return fechaVencimiento;
        }
    }
}