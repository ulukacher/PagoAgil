using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagoAgilFrba.Classes
{
   public class Rendicion
    {
       public Rendicion(string empresaCuit,List<Factura> facturas,DateTime fecha,decimal porcentajeComision)
       {
           this.Fecha = fecha;
           this.PorcentajeComision = porcentajeComision;
           this.Facturas = facturas;
           this.EmpresaCuit = empresaCuit;
       }
        public decimal PorcentajeComision{ get; set; }
        public string EmpresaCuit { get; set; }
        public DateTime Fecha { get; set; }
        public List<Factura> Facturas { get; set; }
        public decimal MontoFacturas { get {
            return this.Facturas.Sum(x => x.Monto);
        } }
        public decimal MontoComision
        {
            get {
                return this.MontoFacturas * (PorcentajeComision / 100);
            }
        }
        public decimal MontoFinal { get { return this.MontoFacturas - this.MontoComision; } }
    }
}
