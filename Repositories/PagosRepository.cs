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
    class PagosRepository
    {
        public static decimal GetNumeroPagoMaximo()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();

            string query = "SELECT MAX(pago_nro) FROM LOS_MANTECOSOS.Pagos";
            SqlCommand command = new SqlCommand(query, conn);

            decimal numeroPagoMaximo = (decimal)command.ExecuteScalar();

            conn.Close();

            return numeroPagoMaximo;
        }

        public static void AgregarPago(Pago pago, List<Factura> listaFacturas)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);

            conn.Open();

            using (var tx = conn.BeginTransaction())
            {
                try
                {
                    string query = "INSERT INTO LOS_MANTECOSOS.Pagos VALUES (@sucursal, @formaDePago, @clienteDNI, @fecha, @importe)";

                    SqlCommand command = new SqlCommand(query, conn, tx);

                    command.Parameters.AddWithValue("@sucursal", pago.Sucursal);
                    command.Parameters.AddWithValue("@formaDePago", pago.FormaDePago);
                    command.Parameters.AddWithValue("@clienteDNI", pago.ClienteDNI);
                    command.Parameters.AddWithValue("@fecha", pago.Fecha);
                    command.Parameters.AddWithValue("@importe", pago.Importe);

                    command.ExecuteNonQuery();

                    foreach (Factura factura in listaFacturas)
                    {
                        string queryFacturas = "UPDATE LOS_MANTECOSOS.Facturas SET factura_estado = @estadoPagado WHERE factura_nro = @nroFactura";

                        SqlCommand commandFacturas = new SqlCommand(queryFacturas, conn, tx);

                        commandFacturas.Parameters.AddWithValue("@nroFactura", factura.Nro);
                        commandFacturas.Parameters.AddWithValue("@estadoPagado", EstadoFactura.Pagada);

                        commandFacturas.ExecuteNonQuery();

                        string queryItems = "INSERT LOS_MANTECOSOS.ItemsPagos VALUES ((SELECT MAX(pago_nro) FROM LOS_MANTECOSOS.Pagos), @nroFactura, @montoPagado)";

                        SqlCommand commandItems = new SqlCommand(queryItems, conn, tx);

                        commandItems.Parameters.AddWithValue("@nroFactura", factura.Nro);
                        commandItems.Parameters.AddWithValue("@montoPagado", pago.Importe);

                        commandItems.ExecuteNonQuery();
                    }

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
    }
}
