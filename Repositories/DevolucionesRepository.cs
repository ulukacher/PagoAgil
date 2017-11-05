﻿using PagoAgilFrba.Classes;
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

                    int montoTotal = 0;

                    List<decimal> listaNumerosFactura = new List<decimal>();

                    foreach (Factura factura in facturasADevolver)
                    {
                        queryMontoTotal = "SELECT SUM(itemFac_monto * itemFac_cantidad) FROM LOS_MANTECOSOS.ItemsFacturas WHERE itemFac_facturaNro = factura_nro";

                        commandMontoTotal = new SqlCommand(queryMontoTotal, conn, tx);

                        montoTotal += (int)commandMontoTotal.ExecuteScalar();

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

                    SqlCommand command2 = new SqlCommand(queryDevolucionMax, conn, tx);

                    int devolucion = (int)command2.ExecuteScalar();

                    var inList = "(" + string.Join(", ", listaNumerosFactura) + ")";

                    string queryItems = "INSERT INTO LOS_MANTECOSOS.ItemsDevoluciones SELECT @nroDevolucion, factura_nro, (select sum(itemFac_monto * itemFac_cantidad) from LOS_MANTECOSOS.ItemsFacturas where itemFac_facturaNro = factura_nro) FROM Facturas WHERE factura_nro IN @listaNumeros";

                    SqlCommand commandItems = new SqlCommand(queryItems, conn, tx);

                    command.Parameters.AddWithValue("@nroDevolucion", devolucion);
                    command.Parameters.AddWithValue("@listaNumeros", inList);

                    /*foreach (Factura factura in listaFacturas)
                    {
                        string queryFacturas = "UPDATE LOS_MANTECOSOS.Facturas SET factura_estado = @estadoPagado WHERE factura_nro = @nroFactura";

                        SqlCommand commandFacturas = new SqlCommand(queryFacturas, conn, tx);

                        commandFacturas.Parameters.AddWithValue("@nroFactura", factura.Nro);
                        commandFacturas.Parameters.AddWithValue("@estadoPagado", EstadoFactura.Pagada);

                        commandFacturas.ExecuteNonQuery();

                        string queryItems = "INSERT LOS_MANTECOSOS.ItemsPagos VALUES ((SELECT MAX(pago_nro) FROM LOS_MANTECOSOS.Pagos), @nroFactura, (select sum(itemFac_monto * itemFac_cantidad) from LOS_MANTECOSOS.ItemsFacturas where itemFac_facturaNro = factura_nro))";

                        SqlCommand commandItems = new SqlCommand(queryItems, conn, tx);

                        commandItems.Parameters.AddWithValue("@nroFactura", factura.Nro);
                        commandItems.Parameters.AddWithValue("@montoPagado", pago.Importe);

                        commandItems.ExecuteNonQuery();
                    }*/

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
