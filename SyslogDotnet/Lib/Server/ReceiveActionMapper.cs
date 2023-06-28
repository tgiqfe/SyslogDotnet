using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Lib.Server
{
    internal class ReceiveActionMapper
    {
        private static Dictionary<ReceiveAction, string[]> _map = null;

        public static ReceiveAction ToReceiveAction(string text)
        {
            _map ??= new Dictionary<ReceiveAction, string[]>()
            {
                { ReceiveAction.None,       new string[] { "none" } },
                { ReceiveAction.Standard,   new string[] { "standard" } },
                { ReceiveAction.FileLog,    new string[] { "filelog", "file" } },
                { ReceiveAction.JsonLog,    new string[] { "jsonlog", "json" } },
                { ReceiveAction.LiteDB,     new string[] { "litedb", "db" } },
                { ReceiveAction.Syslog,     new string[] { "syslog" } },
                { ReceiveAction.Logstash,   new string[] { "logstash" } },
            };

            var ret = ReceiveAction.None;
            var actions = text.Split(",").Select(x => x.Trim());

            foreach (var pair in _map)
            {
                var isMatch = pair.Value.Any(x =>
                    actions.Any(y => y.Equals(x, StringComparison.OrdinalIgnoreCase)));
                if (isMatch)
                {
                    ret |= pair.Key;
                }
            }

            return ret;
        }
    }
}
