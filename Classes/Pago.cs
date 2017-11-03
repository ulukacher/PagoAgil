using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagoAgilFrba.Classes
{
    class Pago
    {
        public decimal Nro { get; set; }
        public decimal Sucursal { get; set; }
        public decimal FormaDePago { get; set; }
        public decimal ClienteDNI { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Importe { get; set; }
    }
}
