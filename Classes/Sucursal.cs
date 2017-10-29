using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagoAgilFrba.Classes
{
    public class Sucursal
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public int CodigoPostal{ get; set; }
        public bool Activa { get; set; }
    }
}
