using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PagoAgilFrba.Helpers
{
    public class EncryptHelper
    {
        public static byte[] SHA256Encrypt(string input)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(input));

                //foreach (Byte b in result)
                //    Sb.Append(b.ToString("x2"));
                return result;
            }

        }
    }
}
