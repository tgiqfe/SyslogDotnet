using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyslogDotnet.Lib;
using SyslogDotnet.Lib.Syslog;
using SyslogDotnet.Lib.Syslog.Sender;

namespace SyslogDotnet.Client.Cmd
{
    internal class ArgsParam
    {
        const string _defaultProtocol = "udp";
        const int _defaultPort = 514;

        public ServerInfo ServerInfo { get; private set; }

        public Format Format { get; private set; } = Format.RFC3164;
        public Facility Facility { get; private set; } = Facility.UserLevelMessages;
        public Severity Severity { get; private set; } = Severity.Informational;
        public string AppName { get; private set; }
        public string Message { get; private set; }

        public bool UseSsl { get; private set; }
        public string CertFile { get; private set; }
        public string CertPassword { get; private set; }
        public bool IgnoreCheck { get; private set; }
        public int Timeout { get; private set; } = 3000;

        public ArgsParam(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "/s":
                    case "-s":
                    case "/server":
                    case "--server":
                        this.ServerInfo = new ServerInfo(args[++i], _defaultPort, _defaultProtocol);
                        break;
                    case "/f":
                    case "-f":
                    case "/format":
                    case "--format":
                        this.Format = FormatMapper.ToFormat(args[++i]);
                        break;
                    case "/c":
                    case "-c":
                    case "/facility":
                    case "--facility":
                        this.Facility = FacilityMapper.ToFacility(args[++i]);
                        break;
                    case "/v":
                    case "-v":
                    case "/severity":
                    case "--severity":
                        this.Severity = SeverityMapper.ToSeverity(args[++i]);
                        break;
                    case "/n":
                    case "-n":
                    case "/appname":
                    case "--appname":
                        this.AppName = args[++i];
                        break;
                    case "/m":
                    case "-m":
                    case "/message":
                    case "--message":
                        this.Message = args[++i];
                        break;
                    case "/l":
                    case "-l":
                    case "/usessl":
                    case "--usessl":
                        this.UseSsl = true;
                        break;
                    case "/r":
                    case "-r":
                    case "/certfile":
                    case "--certfile":
                        this.CertFile = args[++i];
                        break;
                    case "/w":
                    case "-w":
                    case "/certpassword":
                    case "--certpassword":
                        this.CertPassword = args[++i];
                        break;
                    case "/i":
                    case "-i":
                    case "/ignorecheck":
                    case "--ignorecheck":
                        this.IgnoreCheck = true;
                        break;
                    case "/o":
                    case "-o":
                    case "/timeout":
                    case "--timeout":
                        if (int.TryParse(args[++i], out int num))
                        {
                            this.Timeout = num;
                        }
                        break;
                }
            }

            if (ServerInfo == null)
            {
                this.ServerInfo = new ServerInfo($"{_defaultProtocol}://localhost:{_defaultPort}");
            }
        }

        public SyslogMessage ToSyslogMessage()
        {
            return new SyslogMessage()
            {
                Format = this.Format,
                DateTime = DateTime.Now,
                Facility = this.Facility,
                Severity = this.Severity,
                HostName = Environment.MachineName,
                AppName = this.AppName,
                ProcId = Process.GetCurrentProcess().Id.ToString(),
                MsgId = "-",
                Message = this.Message,
            };
        }

        public SyslogSender ToSyslogSender()
        {
            if (ServerInfo.Protocol == "udp")
            {
                //  UDP
                return new SyslogUdpSender(this.ServerInfo.Server, this.ServerInfo.Port);
            }
            else if (ServerInfo.Protocol == "tcp")
            {
                if (this.UseSsl)
                {
                    if (this.CertFile == null)
                    {
                        //  TCP,暗号化有り,クライアント証明書無し
                        return new SyslogTcpSenderTLS(this.ServerInfo.Server, this.ServerInfo.Port)
                        {
                            IgnoreCheck = this.IgnoreCheck,
                            Timeout = this.Timeout,
                        };
                    }
                    else
                    {
                        //  TCP,暗号化有り,クライアント証明書有り
                        return new SyslogTcpSenderTLS(this.ServerInfo.Server, this.ServerInfo.Port)
                        {
                            IgnoreCheck = this.IgnoreCheck,
                            Timeout = this.Timeout,
                            CertFile = this.CertFile,
                            CertPassword = this.CertPassword,
                        };
                    }
                }
                else
                {
                    //  TCP,暗号化無し
                    return new SyslogTcpSender(this.ServerInfo.Server, this.ServerInfo.Port);
                }
            }

            return null;
        }
    }
}
