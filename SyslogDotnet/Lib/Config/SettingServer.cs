using SyslogDotnet.Lib.Syslog.Receiver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using static SyslogDotnet.Lib.Config.Setting;

namespace SyslogDotnet.Lib.Config
{
    public class SettingServer
    {
        #region Serialize parameter

        public string UdpServer { get; set; }
        public string TcpServer { get; set; }
        public bool? UseSsl { get; set; }
        public string CertFile { get; set; }
        public string CertPassword { get; set; }
        public bool? ClientCertificateRequired { get; set; }
        public string PermittedPeer { get; set; }
        public Dictionary<string, SettingServerRule> Rules { get; set; }

        #endregion

        [YamlIgnore]
        public string SelectedRuleName { get; set; }

        const string _defaultRemoteAddress = "0.0.0.0";
        const string _defaultProtocol = "udp";
        const int _defaultPort = 514;

        #region Udp Server

        [YamlIgnore]
        public ServerInfo UdpServerInfo
        {
            get
            {
                _udpServerInfo ??= string.IsNullOrEmpty(this.UdpServer) ?
                    null :
                    new ServerInfo(this.UdpServer, _defaultPort, _defaultProtocol);
                return _udpServerInfo;
            }
        }
        private ServerInfo _udpServerInfo = null;

        #endregion
        #region Tcp Server

        [YamlIgnore]
        public ServerInfo TcpServerInfo
        {
            get
            {
                _tcpServerInfo ??= string.IsNullOrEmpty(this.TcpServer) ?
                    null :
                    new ServerInfo(this.TcpServer, _defaultPort, _defaultProtocol);
                return _tcpServerInfo;
            }
        }
        private ServerInfo _tcpServerInfo = null;

        #endregion

        public SyslogReceiver GetUdpServer()
        {
            return this.UdpServerInfo == null ?
                null :
                new SyslogUdpReceiver(UdpServerInfo.Server, UdpServerInfo.Port);
        }

        public SyslogReceiver GetTcpServer()
        {
            if (this.TcpServerInfo != null)
            {
                if (this.UseSsl == true)
                {
                    if (this.ClientCertificateRequired == true)
                    {
                        return new SyslogTcpReceiverTLS(this.TcpServerInfo.Server, this.TcpServerInfo.Port)
                        {
                            CertFile = this.CertFile,
                            CertPassword = this.CertPassword,
                            ClientCertificateRequired = true,
                            PermittedPeer = this.PermittedPeer,
                        };
                    }
                    else
                    {
                        return new SyslogTcpReceiverTLS(this.TcpServerInfo.Server, this.TcpServerInfo.Port)
                        {
                            CertFile = this.CertFile,
                            CertPassword = this.CertPassword,
                        };
                    }
                }
                else
                {
                    return new SyslogTcpReceiver(this.TcpServerInfo.Server, this.TcpServerInfo.Port);
                }
            }
            return null;
        }
    }
}
