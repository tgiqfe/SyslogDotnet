using SyslogDotnet.Lib;
using SyslogDotnet.Lib.Config;
using SyslogDotnet.Lib.Syslog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SyslogDotnet.Lib.Config.Setting;

namespace SyslogDotnet.Client.Cmd
{
    internal class ArgsParam2
    {
        const string _defaultaddress = "0.0.0.0";
        const string _defaultProtocol = "udp";
        const int _defaultPort = 514;
        const string _tempRuleName = "temp";

        public ServerInfo ServerInfo { get; private set; }
        public Format Format { get; private set; }
        public Facility Facility { get; private set; }
        public Severity Severity { get; private set; }
        public string AppName { get; private set; }
        public string Message { get; private set; }
        public bool UseSsl { get; private set; }
        public string CertFile { get; private set; }
        public string CertPassword { get; private set; }
        public bool IgnoreCheck { get; private set; }
        public int Timeout { get; private set; } = 3000;

        public string SettingPath { get; set; }
        public bool SaveSetting { get; set; }
        public string RuleName { get; set; }

        public ArgsParam2(string[] args)
        {
            this.SettingPath = GetSettingPath(args);
            var collection = GetSettingCollection(args, SettingPath);

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
            collection.Setting.Client ??= new();
            
            return collection;
        }
    }
}
