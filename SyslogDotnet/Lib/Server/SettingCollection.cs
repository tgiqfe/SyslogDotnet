using SyslogDotnet.Lib.Syslog;

namespace SyslogDotnet.Lib.Server
{
    internal class SettingCollection
    {
        public List<Rule> Settings { get; set; }

        public Rule MatchSetting(Facility facility, Severity severity)
        {
            return Settings.FirstOrDefault(x => x.IsMatch(facility, severity));
        }
    }
}
