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
    class DevolucionesRepository
    {
        public static void DevolverFactura(List<Factura> facturasADevolver, string motivo)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);

            conn.Open();

            using (var tx = conn.BeginTransaction())
            {
                try
                {
                    string queryMontoTotal;
                    SqlCommand commandMontoTotal;

                    decimal montoTotal = 0;

                    List<decimal> listaNumerosFactura = new List<decimal>();

                    foreach (Factura factura in facturasADevolver)
                    {
                        queryMontoTotal = "SELECT SUM(itemFac_monto * itemFac_cantidad) FROM LOS_MANTECOSOS.ItemsFacturas WHERE itemFac_facturaNro = @nroFactura";

                        commandMontoTotal = new SqlCommand(queryMontoTotal, conn, tx);

                        commandMontoTotal.Parameters.AddWithValue("@nroFactura", factura.Nro);

                        montoTotal += (decimal)commandMontoTotal.ExecuteScalar();

                        listaNumerosFactura.Add(factura.Nro);
                    }

                    string query = "INSERT INTO LOS_MANTECOSOS.Devoluciones VALUES (@clienteDNI, @motivo, @fecha, @montoTotal)";

                    SqlCommand command = new SqlCommand(query, conn, tx);

                    command.Parameters.AddWithValue("@clienteDNI", facturasADevolver[0].ClienteDNI);
                    command.Parameters.AddWithValue("@motivo", motivo);
                    command.Parameters.AddWithValue("@fecha", DateTime.Today);
                    command.Parameters.AddWithValue("@montoTotal", montoTotal);

                    command.ExecuteNonQuery();

                    string queryDevolucionMax = "SELECT MAX(devo_id) FROM LOS_MANTECOSOS.Devoluciones";

                    SqlCommand commandDevolucionMax = new SqlCommand(queryDevolucionMax, conn, tx);

                    decimal devolucion = (decimal)commandDevolucionMax.ExecuteScalar();

                    var inList = "(" + string.Join(", ", listaNumerosFactura) + ")";

                    string queryItems = "INSERT INTO LOS_MANTECOSOS.ItemsDevoluciones SELECT @nroDevolucion, factura_nro, (SELECT SUM(itemFac_monto * itemFac_cantidad) FROM LOS_MANTECOSOS.ItemsFacturas WHERE itemFac_facturaNro = factura_nro) FROM LOS_MANTECOSOS.Facturas WHERE factura_nro IN "+inList;

                    SqlCommand commandItems = new SqlCommand(queryItems, conn, tx);

                    commandItems.Parameters.AddWithValue("@nroDevolucion", devolucion);
                    
                    commandItems.ExecuteNonQuery();

                    string queryUpdateFactura = "UPDATE LOS_MANTECOSOS.Facturas SET factura_estado = 0 WHERE factura_nro IN " + inList;

                    SqlCommand commandUpdate = new SqlCommand(queryUpdateFactura, conn, tx);

                    commandUpdate.ExecuteNonQuery();

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
