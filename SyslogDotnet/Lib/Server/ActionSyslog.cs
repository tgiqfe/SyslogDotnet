using SyslogDotnet.Lib.Syslog;
using SyslogDotnet.Lib.Syslog.Sender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Lib.Server
{
    internal class ActionSyslog
    {
        const string _defaultProtocol = "udp";
        const int _defaultPort = 514;

        public static void Output(SyslogMessage message,
            string server,
            bool useSsl,
            string certFile,
            string certPassword,
            bool ignoreCheck,
            int timeout)
        {
            var svInfo = new ServerInfo(server, _defaultPort, _defaultProtocol);

            SyslogSender sender = null;
            if (svInfo.Protocol == "udp")
            {
                sender = new SyslogUdpSender(svInfo.Server, svInfo.Port);
            }
            else if (svInfo.Protocol == "tcp")
            {
                if (useSsl)
                {
                    if (string.IsNullOrEmpty(certFile))
                    {
                        sender = new SyslogTcpSenderTLS(svInfo.Server, svInfo.Port)
                        {
                            IgnoreCheck = ignoreCheck,
                            Timeout = timeout,
                        };
                    }
                    else
                    {
                        sender = new SyslogTcpSenderTLS(svInfo.Server, svInfo.Port)
                        {
                            IgnoreCheck = ignoreCheck,
                            Timeout = timeout,
                            CertFile = certFile,
                            CertPassword = certPassword,
                        };
                    }
                }
                else
                {
                    sender = new SyslogTcpSender(svInfo.Server, svInfo.Port);
                }
            }

            sender.Init();
            sender.Send(message);
        }
    }
}
