using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagoAgilFrba.Classes
{
   public class Cliente
    {
       public decimal DNI { get; set; }
       public string Nombre { get; set; }
       public string Apellido { get; set; }
       public string Direccion { get; set; }
       public DateTime FechaNacimiento { get; set; }
       public string CodigoPostal { get; set; }
       public bool Activo { get; set; }
       public string Mail { get; set; }
       public string Telefono { get; set; }
    }
}
