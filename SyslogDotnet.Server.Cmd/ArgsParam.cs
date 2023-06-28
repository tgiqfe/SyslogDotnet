using SyslogDotnet.Lib;
using SyslogDotnet.Lib.Syslog.Receiver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Server.Cmd
{
    internal class ArgsParam
    {
        const string _defaultaddress = "0.0.0.0";
        const int _defaultPort = 514;

        public ServerInfo Udp { get; private set; }
        public ServerInfo Tcp { get; private set; }

        public string SettingPath { get; set; }
        public bool SaveSetting { get; set; }

        public bool UseSsl { get; private set; }
        public string CertFile { get; private set; }
        public string CertPassword { get; private set; }
        public bool ClientCertificateRequired { get; private set; }
        public string PermittedPeer { get; private set; }

        public ArgsParam(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "/u":
                    case "-u":
                    case "/udp":
                    case "--udp":
                        string udpServer =
                            (i + 1) < args.Length && (!args[i + 1].StartsWith("/") && !args[i + 1].StartsWith("-")) ?
                                args[++i] : _defaultaddress;
                        this.Udp = new ServerInfo(udpServer, _defaultPort, "udp");
                        break;
                    case "/t":
                    case "-t":
                    case "/tcp":
                    case "--tcp":
                        string tcpServer =
                            (i + 1) < args.Length && (!args[i + 1].StartsWith("/") && !args[i + 1].StartsWith("-")) ?
                            args[++i] : _defaultaddress;
                        this.Tcp = new ServerInfo(tcpServer, _defaultPort, "tcp");
                        break;
                    case "/g":
                    case "-g":
                    case "/config":
                    case "--config":
                        this.SettingPath = args[++i];
                        break;
                    case "/v":
                    case "-v":
                    case "/save":
                    case "--save":
                        this.SaveSetting = true;
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
                    case "/q":
                    case "-q":
                    case "/clientcertificaterequired":
                    case "--clientcertificaterequired":
                        this.ClientCertificateRequired = true;
                        break;
                    case "/e":
                    case "-e":
                    case "/permittedpeer":
                    case "--permittedpeer":
                        this.PermittedPeer = args[++i];
                        break;
                }
            }
        }

        public SyslogReceiver ToUdpReceiver()
        {
            //  UDP
            return this.Udp == null ?
                null :
                new SyslogUdpReceiver(Udp.Server, Udp.Port);
        }

        public SyslogReceiver ToTcpReceiver()
        {
            if (this.Tcp == null)
            {
                return null;
            }
            else if (this.UseSsl)
            {
                if (this.ClientCertificateRequired)
                {
                    //  TCP,暗号化有り,クライアント証明書有り
                    return new SyslogTcpReceiverTLS(Tcp.Server, Tcp.Port)
                    {
                        CertFile = this.CertFile,
                        CertPassword = this.CertPassword,
                        ClientCertificateRequired = this.ClientCertificateRequired,
                        PermittedPeer = this.PermittedPeer,
                    };
                }
                else
                {
                    //  TCP,暗号化有り、クライアント証明書無し
                    return new SyslogTcpReceiverTLS(Tcp.Server, Tcp.Port)
                    {
                        CertFile = this.CertFile,
                        CertPassword = this.CertPassword,
                    };
                }
            }
            else
            {
                //  TCP,暗号化無し
                return new SyslogTcpReceiver(Tcp.Server, Tcp.Port);
            }
        }
    }
}
