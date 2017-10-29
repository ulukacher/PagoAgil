using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagoAgilFrba.Classes
{
    public class ItemFactura
    {
        public decimal NroItem { get; set; }
        public decimal NroFactura { get; set; }
        public decimal Monto { get; set; }
        public decimal Cantidad { get; set; }
        public string Detalle { get; set; }

        public override string ToString()
        {
            return Detalle;
        }
    }
}