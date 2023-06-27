using SyslogDotnet.Lib;
using SyslogDotnet.Lib.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Server.Cmd
{
    internal class ArgsParam2
    {
        const string _defaultaddress = "0.0.0.0";
        const string _defaultProtocol = "udp";
        const int _defaultPort = 514;

        public string SettingPath { get; private set; }

        public ArgsParam2(string[] args)
        {
            this.SettingPath = GetSettingPath(args);
            var collection = GetSettingCollection(args, SettingPath);


        }

        private string GetSettingPath(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "/g":
                    case "-g":
                    case "/config":
                    case "--config":
                        return args[++i];
                }
            }
            return null;
        }

        private SettingCollection GetSettingCollection(string[] args, string settingPath)
        {
            SettingCollection collection = SettingCollection.Deserialize(settingPath);
            
            collection.Setting ??= new();
            collection.Setting.Server ??= new();

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
                        collection.Setting.Server.UdpServer = udpServer;
                        break;
                    case "/t":
                    case "-t":
                    case "/tcp":
                    case "--tcp":
                        string tcpServer =
                            (i + 1) < args.Length && (!args[i + 1].StartsWith("/") && !args[i + 1].StartsWith("-")) ?
                            args[++i] : _defaultaddress;
                        collection.Setting.Server.TcpServer = tcpServer;
                        break;
                    case "/l":
                    case "-l":
                    case "/usessl":
                    case "--usessl":
                        collection.Setting.Server.UseSsl = true;
                        break;
                    case "/r":
                    case "-r":
                    case "/certfile":
                    case "--certfile":
                        collection.Setting.Server.CertFile = args[++i];
                        break;
                    case "/w":
                    case "-w":
                    case "/certpassword":
                    case "--certpassword":
                        collection.Setting.Server.CertPassword = args[++i];
                        break;
                    case "/q":
                    case "-q":
                    case "/clientcertificaterequired":
                    case "--clientcertificaterequired":
                        collection.Setting.Server.ClientCertificateRequired = true;
                        break;
                    case "/e":
                    case "-e":
                    case "/permittedpeer":
                    case "--permittedpeer":
                        collection.Setting.Server.PermittedPeer = args[++i];
                        break;
                }
            }
            return collection;
        }
    }
}
