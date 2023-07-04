using SyslogDotnet.Lib.Syslog;
using SyslogDotnet.Lib.Syslog.Sender;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using static SyslogDotnet.Lib.Config.Setting;

namespace SyslogDotnet.Lib.Config
{
    public class SettingClient
    {
        const int _defaultPort = 514;
        const string _defaultProtocol = "udp";
        const int _defaultTimeout = 3000;

        [YamlIgnore]
        public string TargetServer { get; set; }

        [YamlIgnore]
        public string DateTime { get; set; }

        [YamlIgnore]
        public string HostName { get; set; }

        [YamlIgnore]
        public string AppName { get; set; }

        [YamlIgnore]
        public string ProcId { get; set; }

        [YamlIgnore]
        public string MsgId { get; set; }

        [YamlIgnore]
        public string Message { get; set; }

        [YamlIgnore]
        public string StructuredDataParams { get; set; }

        [YamlIgnore]
        public ServerInfo ServerInfo
        {
            get
            {
                _serverInfo ??= new ServerInfo(this.TargetServer, _defaultPort, _defaultProtocol);
                return _serverInfo;
            }
        }
        private ServerInfo _serverInfo = null;

        [YamlIgnore]
        public string SelectedRuleName { get; set; }

        [YamlIgnore]
        public SettingClientRule SelectedRule
        {
            get
            {
                _selectedRule ??= !string.IsNullOrEmpty(this.SelectedRuleName) && this.Rules.ContainsKey(this.SelectedRuleName) ?
                    Rules[this.SelectedRuleName] :
                    null;
                return _selectedRule;
            }
        }
        private SettingClientRule _selectedRule = null;

        #region Serialize parameter

        public Dictionary<string, SettingClientRule> Rules { get; set; }

        #endregion

        /// <summary>
        /// Syslogメッセージに含めるログ生成時間。
        /// nullの場合は現在の時刻を返す。
        /// </summary>
        /// <returns></returns>
        public DateTime GetDateTime()
        {
            return System.DateTime.TryParse(this.DateTime, out DateTime dt) ?
                dt :
                System.DateTime.Now;
        }

        /// <summary>
        /// Syslogメッセージに含めるホスト名。
        /// nullの場合は自マシンのホスト名を返す。
        /// </summary>
        /// <returns></returns>
        public string GetHostName()
        {
            return string.IsNullOrEmpty(this.HostName) ?
                Environment.MachineName :
                this.HostName;
        }

        /// <summary>
        /// Syslogメッセージに含めるプロセスID。
        /// nullの場合は、自プロセスのIDを返す。
        /// </summary>
        /// <returns></returns>
        public string GetProcId()
        {
            return string.IsNullOrEmpty(this.ProcId) ?
                Process.GetCurrentProcess().Id.ToString() :
                this.ProcId;
        }

        private StructuredData[] _sd = null;

        public StructuredData[] GetStructuredDataParams()
        {
            _sd ??= StructuredData.Deserialize(this.StructuredDataParams);
            return _sd;
        }

        public void SetStructuredDataParams(StructuredData[] sd)
        {
            _sd = sd;
        }

        public SyslogMessage GetSyslogMessage(string ruleName = null)
        {
            if (this.SelectedRule == null) { return null; }

            return new SyslogMessage()
            {
                Format = SelectedRule.GetFormat(),
                DateTime = GetDateTime(),
                Facility = SelectedRule.GetFacility(),
                Severity = SelectedRule.GetSeverity(),
                HostName = GetHostName(),
                AppName = AppName,
                ProcId = GetProcId(),
                MsgId = MsgId,
                StructuredDataParams = StructuredData.Deserialize(StructuredDataParams),
                Message = Message,
            };
        }

        public SyslogSender GetSyslogSender()
        {
            if (this.ServerInfo.Protocol == "udp")
            {
                //  UDP
                return new SyslogUdpSender(this.ServerInfo.Server, this.ServerInfo.Port);
            }
            else if (ServerInfo.Protocol == "tcp")
            {
                if (this.SelectedRule.UseSsl == true)
                {
                    if (string.IsNullOrEmpty(this.SelectedRule.CertFile))
                    {
                        //  TCP,暗号化有り,クライアント証明書無し
                        return new SyslogTcpSenderTLS(this.ServerInfo.Server, this.ServerInfo.Port)
                        {
                            IgnoreCheck = this.SelectedRule.IgnoreCheck == true,
                            Timeout = this.SelectedRule.Timeout is null ?
                                _defaultTimeout :
                                (int)this.SelectedRule.Timeout,
                        };
                    }
                    else
                    {
                        //  TCP,暗号化有り,クライアント証明書有り
                        return new SyslogTcpSenderTLS(this.ServerInfo.Server, this.ServerInfo.Port)
                        {
                            IgnoreCheck = this.SelectedRule.IgnoreCheck == true,
                            Timeout = this.SelectedRule.Timeout is null ?
                                _defaultTimeout :
                                (int)this.SelectedRule.Timeout,
                            CertFile = this.SelectedRule.CertFile,
                            CertPassword = this.SelectedRule.CertPassword,
                        };
                    }
                }
                else
                {
                    //  TCP
                    return new SyslogTcpSender(this.ServerInfo.Server, this.ServerInfo.Port);
                }
            }
            return null;
        }
    }
}
