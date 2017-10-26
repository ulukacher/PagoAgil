using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PagoAgilFrba
{
    static class Program
    {
        public static string regexSoloLetras
        {
            get
            {
                return "^[a-zA-Z]+$";
            }
        }
        public static string regexSoloNumeros
        {
            get {
            return @"^\d+$";
        }
        } 
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
