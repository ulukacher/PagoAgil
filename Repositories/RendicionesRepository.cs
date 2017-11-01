using PagoAgilFrba.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagoAgilFrba.Repositories
{
    public class RendicionesRepository
    {
        public static bool ExisteRendicionParaEmpresaYPeriodo(string cuitEmpresa, DateTime fechaRendicion) {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            string query = "select count(*) from LOS_MANTECOSOS.rendiciones where rend_empresaCuit = @cuitEmpresa and year(rend_fecha) = @anio" +
                " and month(rend_fecha)=@mes";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@cuitEmpresa", cuitEmpresa);
            command.Parameters.AddWithValue("@anio", DateTime.Now.Year);
            command.Parameters.AddWithValue("@mes", DateTime.Now.Month);
            int cantRendiciones = (int)command.ExecuteScalar();
            conn.Close();
            return cantRendiciones > 0;
        }
        public static void AgregarRendicion(Rendicion rendicion) {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GDD"].ConnectionString);
            conn.Open();
            using (var tx= conn.BeginTransaction())
            {
                try
                {
                    string query = "insert into LOS_MANTECOSOS.Rendiciones (rend_empresaCuit,rend_fecha,rend_porcentajeComision,rend_montoTotal)" +
                    "VALUES (@empresaCuit, @fecha,@porcentajeComision,@montoFinal)";
                    SqlCommand command = new SqlCommand(query,conn, tx);
                    command.Parameters.AddWithValue("@empresaCuit", rendicion.EmpresaCuit);
                    command.Parameters.AddWithValue("@fecha", rendicion.Fecha);
                    command.Parameters.AddWithValue("@porcentajeComision", rendicion.PorcentajeComision);
                    command.Parameters.AddWithValue("@montoFinal", rendicion.MontoFinal);
                    command.ExecuteNonQuery();

                    //Obtengo el numero nro de rendicion
                    string queryUltimaRendicion = "select max(rend_nro) from LOS_MANTECOSOS.Rendiciones";
                    SqlCommand commandUltimaRendicion = new SqlCommand(queryUltimaRendicion, conn,tx);
                    decimal ultimoNroRendicion = (decimal)commandUltimaRendicion.ExecuteScalar();

                    //Agrego los items de rendicion

                    string queryItems = "insert into LOS_MANTECOSOS.ItemsRendiciones (itemRend_rendicionNro,itemRend_facturaNro,itemRend_montoRendido)"
                        +"select @nroRendicion,factura_nro,(select sum(itemFac_monto) from LOS_MANTECOSOS.ItemsFacturas where itemFac_facturaNro = factura_nro) "+
                        "from LOS_MANTECOSOS.facturas"+
                        " where factura_empresaCuit = @cuit and factura_estado = @estado and year(factura_fecha) = @anio and month(factura_fecha) = @mes";
                    SqlCommand commandItems = new SqlCommand(queryItems, conn,tx);
                    commandItems.Parameters.AddWithValue("@nroRendicion", ultimoNroRendicion);
                    commandItems.Parameters.AddWithValue("@cuit", rendicion.EmpresaCuit);
                    commandItems.Parameters.AddWithValue("@estado", EstadoFactura.Pagada);
                    commandItems.Parameters.AddWithValue("@anio", DateTime.Now.Year);
                    commandItems.Parameters.AddWithValue("@mes", DateTime.Now.Month);
                    commandItems.ExecuteNonQuery();


                    //Marco la factura como rendida
                    string queryEstado = "update f"+
                                       " set F.factura_Estado = @nuevoEstado"+
                                        " from LOS_MANTECOSOS.Facturas f"+
                                        " inner join LOS_MANTECOSOS.itemsRendiciones i on i.itemRend_facturaNro = f.factura_nro"+
                                        " where itemRend_rendicionNro = @nroRendicion";
                    SqlCommand commandEstado = new SqlCommand(queryEstado, conn, tx);
                    commandEstado.Parameters.AddWithValue("@nroRendicion", ultimoNroRendicion);
                    commandEstado.Parameters.AddWithValue("@nuevoEstado", EstadoFactura.Rendida);
                    commandEstado.ExecuteNonQuery();

                    tx.Commit();
                    conn.Close();
                }
                catch (Exception exc) {
                    tx.Rollback();
                    conn.Close();
                    throw new Exception(exc.Message);
                }
            }        
            
        }
    }
}
