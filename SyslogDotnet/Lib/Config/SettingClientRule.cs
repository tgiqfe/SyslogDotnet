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

        private Format? _format = null;
        private Facility? _facility = null;
        private Severity? _severity = null;

        public Format GetFormat()
        {
            _format ??= FormatMapper.ToFormat(this.Format);
            return (Format)_format;

            //return FormatMapper.ToFormat(this.Format);
        }

        public Facility GetFacility()
        {
            _facility ??= FacilityMapper.ToFacility(this.Facility);
            return (Facility)_facility;
            //return FacilityMapper.ToFacility(this.Facility);
        }

        public Severity GetSeverity()
        {
            _severity = SeverityMapper.ToSeverity(this.Severity);
            return (Severity)_severity;
            //return SeverityMapper.ToSeverity(this.Severity);
        }

        public void SetFormat(Format? format)
        {
            _format = format;
        }

        public void SetFacility(Facility? facility)
        {
            _facility = facility;
        }

        public void SetSeverity(Severity? severity)
        {
            _severity = severity;
        }
    }
}
