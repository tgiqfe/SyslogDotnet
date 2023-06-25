using SyslogDotnet.Lib.Syslog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Lib.Config
{
    internal class SettingCollection
    {
        public Setting Setting { get; set; }

        private Dictionary<string, IEnumerable<Facility>> _faciliies = null;
        private Dictionary<string, bool> _isAny = null;

        public Setting.SettingServerRule GetServerRule(Facility facility, Severity severity)
        {
            return Setting.Server.Rules.
                FirstOrDefault(x => x.Value.IsMatch(facility, severity)).Value;
        }

        public Setting.SettingClientRule GetClientRule(string name)
        {
            return Setting.Client.Rules.FirstOrDefault(x => x.Value.Equals(name)).Value;
        }
    }
}
