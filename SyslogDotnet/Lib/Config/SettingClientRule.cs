using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Lib.Config
{
    public class SettingClientRule
    {
        public string Name { get; set; }
        public string Format { get; set; }
        public string Facilities { get; set; }
        public string Severity { get; set; }
        public bool? UseSsl { get; set; }
        public string CertFile { get; set; }
        public string CertPassword { get; set; }
        public bool? IgnoreCheck { get; set; }
        public int? Timeout { get; set; }
    }
}
