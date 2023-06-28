using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Lib.Config
{
    internal class TestData
    {
        public static SettingCollection Create()
        {
            SettingCollection collection = new();
            collection.Setting = new();

            collection.Setting.Server = new()
            {
                UdpServer = "udp://localhost:514",
                TcpServer = "tcp://localhost:514",
                UseSsl = true,
                CertFile = "cert.pfx",
                CertPassword = "password",
                ClientCertificateRequired = true,
                PermittedPeer = "localhost.localdomain",
            };
            collection.Setting.Server.Rules = new();
            SettingServerRule sRule1 = new()
            {
                Name = "サーバRule01",
                Facilities = "User",
                Severity = "warn",
                Action = "JsonLog",
                FileLogPath = null,
                JsonLogPath = "output1.json",
                LiteDBPath = null,
                SyslogServer = null,
                LogstashServer = null,
            };
            SettingServerRule sRule2 = new()
            {
                Name = "サーバRule02",
                Facilities = "Local0, Local1, Local2",
                Severity = "info",
                Action = "JsonLog",
                FileLogPath = null,
                JsonLogPath = "output2.json",
                LiteDBPath = null,
                SyslogServer = null,
                LogstashServer = null,
            };
            collection.Setting.Server.Rules[sRule1.Name] = sRule1;
            collection.Setting.Server.Rules[sRule2.Name] = sRule2;

            collection.Setting.Client = new();
            collection.Setting.Client.Rules = new();
            SettingClientRule cRule1 = new()
            {
                Name = "ルール1",
                Format = "3164",
                Facilities = "Local0, Local1, Local2",
                Severity = "Info",
                UseSsl = true,
                CertFile = "clcert.pfx",
                CertPassword = "password",
                IgnoreCheck = false,
                Timeout = 3000,
            };
            SettingClientRule cRule2 = new()
            {
                Name = "ルール2",
                Format = "3164",
                Facilities = "Local0, Local1, Local2",
                Severity = "Warn",
                UseSsl = true,
                CertFile = "clcert.pfx",
                CertPassword = "password",
                IgnoreCheck = false,
                Timeout = 3000,
            };
            SettingClientRule cRule3 = new()
            {
                Name = "ルール3",
                Format = "3164",
                Facilities = "Local0, Local1, Local2",
                Severity = "Error",
                UseSsl = true,
                CertFile = "clcert.pfx",
                CertPassword = "password",
                IgnoreCheck = false,
                Timeout = 3000,
            };
            collection.Setting.Client.Rules[cRule1.Name] = cRule1;
            collection.Setting.Client.Rules[cRule2.Name] = cRule2;
            collection.Setting.Client.Rules[cRule3.Name] = cRule3;

            return collection;
        }
    }
}
