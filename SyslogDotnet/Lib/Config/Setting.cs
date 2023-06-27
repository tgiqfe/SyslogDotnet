using SyslogDotnet.Lib.Syslog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Lib.Config
{
    public class Setting
    {
        /// <summary>
        /// サーバ側グローバル設定
        /// </summary>
        public class SettingServer
        {
            public string UdpServer { get; set; }
            public string TcpServer { get; set; }
            public bool? UseSsl { get; set; }
            public string CertFile { get; set; }
            public string CertPassword { get; set; }
            public bool? ClientCertificateRequired { get; set; }
            public string PermittedPeer { get; set; }
            public Dictionary<string, SettingServerRule> Rules { get; set; }
        }

        /// <summary>
        /// サーバ側の個別ルール用
        /// </summary>
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

        /// <summary>
        /// クライアント側グローバル設定
        /// </summary>
        public class SettingClient
        {
            public Dictionary<string, SettingClientRule> Rules { get; set; }
        }

        /// <summary>
        /// クライアント側の個別ルール用
        /// </summary>
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

        /// <summary>
        /// サーバ用設定
        /// </summary>
        public SettingServer Server { get; set; }

        /// <summary>
        /// クライアント用設定
        /// </summary>
        public SettingClient Client { get; set; }
    }
}
