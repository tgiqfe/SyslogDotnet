using SyslogDotnet.Lib.Syslog;

namespace SyslogDotnet.Lib.Server
{
    /// <summary>
    /// Syslog受信時の動作ルールを記述
    /// </summary>
    internal class Rule
    {
        public string Name { get; set; }
        public string[] Facilities { get; set; }
        public Severity Severity { get; set; }

        public ReceiveAction Action { get; set; }

        public string FileLogPath { get; set; }
        public string JsonLogPath { get; set; }
        public string LiteDBPath { get; set; }
        public string SyslogServer { get; set; }
        public string LogstashServer { get; set; }

        private IEnumerable<Facility> _facilities = null;
        private bool? _isAny = null;

        /// <summary>
        /// FacilityとSeverityから、ルールにマッチするかどうかを確認
        /// </summary>
        /// <param name="facility"></param>
        /// <param name="severity"></param>
        /// <returns></returns>
        public bool IsMatch(Facility facility, Severity severity)
        {
            _facilities ??= Facilities.Select(x => FacilityMapper.ToFacility(x));
            _isAny ??= _facilities.Any(x => x == Facility.Any);

            return (_isAny == true || _facilities.Any(x => x == facility)) && Severity >= severity;
        }
    }
}
