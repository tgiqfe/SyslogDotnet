using SyslogDotnet.Lib.Syslog;
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
        public string Facility { get; set; }
        public string Severity { get; set; }
        public bool? UseSsl { get; set; }
        public string CertFile { get; set; }
        public string CertPassword { get; set; }
        public bool? IgnoreCheck { get; set; }
        public int? Timeout { get; set; }

        public Format GetFormat()
        {
            return FormatMapper.ToFormat(this.Format);
        }

        public Facility GetFacility()
        {
            return FacilityMapper.ToFacility(this.Facility);
        }

        public Severity GetSeverity()
        {
            return SeverityMapper.ToSeverity(this.Severity);
        }
    }
}
