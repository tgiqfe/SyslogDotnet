using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Lib.Syslog
{
    public class SeverityMapper
    {
        private static Dictionary<Severity, string[]> _map = null;

        public static Severity ToSeverity(string text, Severity defaultSeverity = Severity.Informational)
        {
            _map ??= new Dictionary<Severity, string[]>()
            {
                { Severity.Emergency,       new string[]{ "Emergency", "emer" } },
                { Severity.Alert,           new string[]{ "Alert", "alrt"} },
                { Severity.Critical,        new string[]{ "Critical", "cri" } },
                { Severity.Error,           new string[]{ "Error", "err" } },
                { Severity.Warning,         new string[]{ "Warning", "warn" } },
                { Severity.Notice,          new string[]{ "Notice", "notification" } },
                { Severity.Informational,   new string[]{ "Informational", "info" } },
                { Severity.Debug,           new string[]{ "Debug", "dbg" } },
            };

            foreach (var pair in _map)
            {
                if (pair.Value.Any(x => x.Equals(text, StringComparison.OrdinalIgnoreCase)))
                {
                    return pair.Key;
                }
            }
            return defaultSeverity;
        }

        public static Severity ToSeverity(int num)
        {
            return (Severity)Enum.ToObject(typeof(Severity), num);
        }

        public static Severity ToSeverityFromPri(int pri)
        {
            return (Severity)Enum.ToObject(typeof(Severity), pri % 8);
        }
    }
}
