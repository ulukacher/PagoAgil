using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagoAgilFrba.Classes
{
    public class Empresa
    {
        public string Cuit { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public int RubroId{ get; set; }
        public string Rubro { get; set; }
        public bool Activa { get; set; }
    }
}
