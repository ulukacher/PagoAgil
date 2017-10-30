using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagoAgilFrba.Classes
{
    public enum EstadoFactura { 
        PendienteDePago = 0,
        Pagada = 1,
        Rendida = 2
    }
    public class Factura
    {
        public decimal Nro { get; set; }
        public string EmpresaCuit { get; set; }
        public decimal ClienteDNI { get; set; }
        public DateTime Fecha { get; set; }
        public int Estado { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal Monto { get; set; }
    }
}