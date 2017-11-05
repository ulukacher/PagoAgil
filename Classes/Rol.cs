using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagoAgilFrba.Repositories;

namespace PagoAgilFrba.Classes
{
    public class Rol
    {
        public static Rol RolActual { get; set; }

        public static void SetRolActual(Rol rol, string userName)
        {
            rol.Funcionalidades = FuncionalidadesRepository.GetAllFuncFromRol(rol);

            if (rol.Nombre == "COBRADOR")
                Sucursal.SucursalActual = SucursalesRepository.GetSucursalByUsuario(userName);

            RolActual = rol;
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public List<Funcionalidad> Funcionalidades { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
