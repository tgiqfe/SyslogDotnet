using SyslogDotnet.Lib;
using SyslogDotnet.Lib.Config;
using SyslogDotnet.Lib.Syslog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Cmd
{
    internal class ArgsParam
    {
        const string _defaultLocalAddress = "0.0.0.0";
        const string _defaultRemoteAddress = "127.0.0.1";

        public const string TEMP_SELECTED_RULENAME = "__tempSelectedRuleName__";

        public static SettingCollection ToSettingCollection(string[] args)
        {
            var collection = CreateSettingCollection(args);
            SetupSettingCollection(args, collection);

            return collection;
        }

        private static SettingCollection CreateSettingCollection(string[] args)
        {
            if (args == null || args.Length == 0) { return null; }

            var subCommand = args[0].ToLower() switch
            {
                "server" => SubCommand.Server,
                "client" => SubCommand.Client,
                _ => SubCommand.None,
            };

            string settingPath = null;
            string selectedRuleName = TEMP_SELECTED_RULENAME;
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "/g":
                    case "-g":
                    case "/config":
                    case "--config":
                        settingPath = args[++i];
                        break;
                    case "/n":
                    case "-n":
                    case "/rulename":
                    case "--rulename":
                        selectedRuleName = args[++i];
                        break;
                }
            }

            var collection = SettingCollection.Deserialize(settingPath);
            collection.Setting ??= new();
            collection.Setting.SubCommand = subCommand;
            if (subCommand == SubCommand.Server)
            {
                collection.Setting.Server ??= new();
            }
            else if (subCommand == SubCommand.Client)
            {
                collection.Setting.Client ??= new();
                collection.Setting.Client.SelectedRuleName = selectedRuleName;
                collection.Setting.Client.Rules ??= new();
                if (!collection.Setting.Client.Rules.ContainsKey(selectedRuleName))
                {
                    collection.Setting.Client.Rules[selectedRuleName] = new();
                }
            }

            return collection;
        }

        private static void SetupSettingCollection(string[] args, SettingCollection collection)
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
                        if (collection.Setting.SubCommand == SubCommand.Server)
                        {
                            collection.Setting.Server.UseSsl = true;
                        }
                        else if (collection.Setting.SubCommand == SubCommand.Client)
                        {
                            collection.Setting.Client.SelectedRule.UseSsl = true;
                        }
                        break;
                    case "/r":
                    case "-r":
                    case "/certfile":
                    case "--certfile":
                        if (collection.Setting.SubCommand == SubCommand.Server)
                        {
                            collection.Setting.Server.CertFile = args[++i];
                        }
                        else if (collection.Setting.SubCommand == SubCommand.Client)
                        {
                            collection.Setting.Client.SelectedRule.CertFile = args[++i];
                        }
                        break;
                    case "/w":
                    case "-w":
                    case "/certpassword":
                    case "--certpassword":
                        if (collection.Setting.SubCommand == SubCommand.Server)
                        {
                            collection.Setting.Server.CertPassword = args[++i];
                        }
                        else if (collection.Setting.SubCommand == SubCommand.Client)
                        {
                            collection.Setting.Client.SelectedRule.CertPassword = args[++i];
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
                        collection.Setting.Client.SelectedRule.Format = args[++i];
                        break;
                    case "/c":
                    case "-c":
                    case "/facility":
                    case "--facility":
                        collection.Setting.Client.SelectedRule.Facility = args[++i];
                        break;
                    case "/v":
                    case "-v":
                    case "/severity":
                    case "--severity":
                        collection.Setting.Client.SelectedRule.Severity = args[++i];
                        break;
                    case "/i":
                    case "-i":
                    case "/ignorecheck":
                    case "--ignorecheck":
                        collection.Setting.Client.SelectedRule.IgnoreCheck = true;
                        break;
                    case "/o":
                    case "-o":
                    case "/timeout":
                    case "--timeout":
                        if (int.TryParse(args[++i], out int num))
                        {
                            collection.Setting.Client.SelectedRule.Timeout = num;
                        }
                        break;
                    case "/m":
                    case "-m":
                    case "/message":
                    case "--message":
                        collection.Setting.Client.Message = args[++i];
                        break;
                    case "/a":
                    case "-a":
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
                    case "/x":
                    case "-x":
                    case "/stractureddata":
                    case "--stractureddata":
                        //  StracturedDataを指定する場合は、最後の引数として使用。
                        collection.Setting.Client.StructuredDataParams = string.Join(" ", args.Skip(i + 1));
                        break;
                }
            }
        }
    }
}
