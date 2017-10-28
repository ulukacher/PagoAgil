using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagoAgilFrba.Classes
{
    public class Usuario
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int CantidadIntentosFallidos { get; set; }
        public long? SucursalId { get; set; }
        public bool Activo { get; set; }

    }
}
