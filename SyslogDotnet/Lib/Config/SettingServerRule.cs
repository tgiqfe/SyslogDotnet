using SyslogDotnet.Lib.Syslog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Lib.Config
{
    public class SettingServerRule
    {
        public string Name { get; set; }
        public string Facilities { get; set; }
        public string Severity { get; set; }
        public string Action { get; set; }
        public string FileLogPath { get; set; }
        public string JsonLogPath { get; set; }
        public string LiteDBPath { get; set; }
        public string SyslogServer { get; set; }
        public string LogstashServer { get; set; }

        private IEnumerable<Facility> _facilities = null;
        private Severity? _severity = null;
        private bool? _isAny = null;

        public bool IsMatch(Facility facility, Severity severity)
        {
            _facilities ??= this.Facilities.Split(",").
                Select(x => x.Trim()).
                Select(x => FacilityMapper.ToFacility(x));
            _severity ??= SeverityMapper.ToSeverity(this.Severity);
            _isAny ??= _facilities.Any(x => x == Facility.Any);

            return (_isAny == true || _facilities.Any(x => x == facility)) && _severity >= severity;
        }
    }
}
