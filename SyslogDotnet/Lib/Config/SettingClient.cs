using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SyslogDotnet.Lib.Config.Setting;

namespace SyslogDotnet.Lib.Config
{
    public class SettingClient
    {
        public Dictionary<string, SettingClientRule> Rules { get; set; }
    }
}
