using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Lib.Syslog
{
    public class Functions
    {
        const string _nilValue = "-";

        private static readonly char[] _disallowChars = new char[] { ' ', '=', ']', '"' };

        const string _version = "1";

        public static string ToAsciiField(string text, int maxLength, bool sdName = false)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return _nilValue;
            }
            if (text.Length > maxLength)
            {
                text = text.Substring(0, maxLength);
            }

            char[] buff = new char[255];
            int index = 0;
            foreach (char c in text)
            {
                if ((c >= 33 && c <= 126) && (!sdName || !_disallowChars.Contains(c)))
                {
                    buff[index++] = c;
                }
            }
            return new string(buff, 0, index);
        }
    }
}
