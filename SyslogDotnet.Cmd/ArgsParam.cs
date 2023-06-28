using SyslogDotnet.Lib;
using SyslogDotnet.Lib.Config;
using SyslogDotnet.Lib.Syslog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Server.Cmd
{
    internal class ArgsParam
    {
        const string _defaultLocalAddress = "0.0.0.0";
        const string _defaultRemoteAddress = "127.0.0.1";
        
        public const string TEMP_CLIENT_RULE = "__tempClientRule__";

        public enum Subcommand
        {
            None,
            Server,
            Client,
        }

        public static (Subcommand, SettingCollection) ToSettingCollection(string[] args)
        {
            var subCommand = GetSubcommand(args);
            var settingPath = GetSettingPath(args);
            return (subCommand, GetSettingCollection(args, settingPath, subCommand));
        }

        private static Subcommand GetSubcommand(string[] args)
        {
            if (args?.Length > 0)
            {
                return args[0].ToLower() switch
                {
                    "server" => Subcommand.Server,
                    "client" => Subcommand.Client,
                    _ => Subcommand.None,
                };
            }
            return Subcommand.None;
        }

        private static string GetSettingPath(string[] args)
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

        private static SettingCollection GetSettingCollection(string[] args, string settingPath, Subcommand subCommand)
        {
            SettingCollection collection = SettingCollection.Deserialize(settingPath);

            collection.Setting ??= new();
            collection.Setting.Server ??= new();
            collection.Setting.Client ??= new();

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
                                args[++i] : _defaultLocalAddress;
                        collection.Setting.Server.UdpServer = udpServer;
                        break;
                    case "/t":
                    case "-t":
                    case "/tcp":
                    case "--tcp":
                        string tcpServer =
                            (i + 1) < args.Length && (!args[i + 1].StartsWith("/") && !args[i + 1].StartsWith("-")) ?
                            args[++i] : _defaultLocalAddress;
                        collection.Setting.Server.TcpServer = tcpServer;
                        break;
                    case "/s":
                    case "-s":
                    case "/server":
                    case "--server":
                        string targetServer =
                            (i + 1) < args.Length && (!args[i + 1].StartsWith("/") && !args[i + 1].StartsWith("-")) ?
                            args[++i] :
                            _defaultRemoteAddress;
                        collection.Setting.Client.TargetServer = targetServer;
                        break;
                    case "/l":
                    case "-l":
                    case "/usessl":
                    case "--usessl":
                        if (subCommand == Subcommand.Server)
                        {
                            collection.Setting.Server.UseSsl = true;
                        }
                        else if (subCommand == Subcommand.Client)
                        {
                            initClientRule(collection, TEMP_CLIENT_RULE);
                            collection.Setting.Client.Rules[TEMP_CLIENT_RULE].UseSsl = true;
                        }
                        break;
                    case "/r":
                    case "-r":
                    case "/certfile":
                    case "--certfile":
                        if (subCommand == Subcommand.Server)
                        {
                            collection.Setting.Server.CertFile = args[++i];
                        }
                        else if (subCommand == Subcommand.Client)
                        {
                            initClientRule(collection, TEMP_CLIENT_RULE);
                            collection.Setting.Client.Rules[TEMP_CLIENT_RULE].CertFile = args[++i];
                        }
                        break;
                    case "/w":
                    case "-w":
                    case "/certpassword":
                    case "--certpassword":
                        if (subCommand == Subcommand.Server)
                        {
                            collection.Setting.Server.CertPassword = args[++i];
                        }
                        else if (subCommand == Subcommand.Client)
                        {
                            initClientRule(collection, TEMP_CLIENT_RULE);
                            collection.Setting.Client.Rules[TEMP_CLIENT_RULE].CertPassword = args[++i];
                        }
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

                    case "/f":
                    case "-f":
                    case "/format":
                    case "--format":
                        initClientRule(collection, TEMP_CLIENT_RULE);
                        collection.Setting.Client.Rules[TEMP_CLIENT_RULE].Format = args[++i];
                        break;
                    case "/c":
                    case "-c":
                    case "/facility":
                    case "--facility":
                    case "/facilities":
                    case "--facilities":
                        initClientRule(collection, TEMP_CLIENT_RULE);
                        collection.Setting.Client.Rules[TEMP_CLIENT_RULE].Facilities = args[++i];
                        break;
                    case "/v":
                    case "-v":
                    case "/severity":
                    case "--severity":
                        initClientRule(collection, TEMP_CLIENT_RULE);
                        collection.Setting.Client.Rules[TEMP_CLIENT_RULE].Severity = args[++i];
                        break;

                    case "/i":
                    case "-i":
                    case "/ignorecheck":
                    case "--ignorecheck":
                        initClientRule(collection, TEMP_CLIENT_RULE);
                        collection.Setting.Client.Rules[TEMP_CLIENT_RULE].IgnoreCheck = true;
                        break;
                    case "/o":
                    case "-o":
                    case "/timeout":
                    case "--timeout":
                        if (int.TryParse(args[++i], out int num))
                        {
                            collection.Setting.Client.Rules[TEMP_CLIENT_RULE].Timeout = num;
                        }
                        break;
                    case "/m":
                    case "-m":
                    case "/message":
                    case "--message":
                        collection.Setting.Client.Message = args[++i];
                        break;
                    case "/n":
                    case "-n":
                    case "/appname":
                    case "--appname":
                        collection.Setting.Client.AppName = args[++i];
                        break;
                    case "/b":
                    case "-b":
                    case "/hostname":
                    case "--hostname":
                        collection.Setting.Client.HostName = args[++i];
                        break;
                    case "/d":
                    case "-d":
                    case "/procid":
                    case "--procid":
                        collection.Setting.Client.ProcId = args[++i];
                        break;
                    case "/j":
                    case "-j":
                    case "/msgid":
                    case "--msgid":
                        collection.Setting.Client.MsgId = args[++i];
                        break;
                }
            }
            return collection;

            void initClientRule(SettingCollection clct, string ruleName)
            {
                clct.Setting.Client.Rules ??= new();
                if (!clct.Setting.Client.Rules.ContainsKey(ruleName))
                {
                    clct.Setting.Client.Rules[ruleName] = new();
                }
            }
        }
    }
}
