using System;
using System.Collections.Generic;
using System.Text;

namespace NzxtRGB
{
    internal static class Util
    {
        internal static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString().ToUpper();
        }
    }
}
