using SyslogDotnet.Lib.Syslog;
using SyslogDotnet.Lib.Syslog.Receiver;
using SyslogDotnet.Lib.Syslog.Sender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace SyslogDotnet.Lib.Config
{
    public class SettingCollection
    {
        public Setting Setting { get; set; }

        #region Serialize/Deserialize

        /// <summary>
        /// デシリアライズ (yml文字列⇒SettingCollectionオブジェクト)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static SettingCollection Deserialize(string path)
        {
            if (path == null) { return new SettingCollection(); }

            SettingCollection collection = null;
            try
            {
                var yml = File.ReadAllText(path);
                collection = new Deserializer().Deserialize<SettingCollection>(yml);
            }
            catch { }
            if (collection == null)
            {
                collection = new();
            }
            return collection;
        }

        /// <summary>
        /// シリアライズ (SettingCollectionオブジェクト⇒yml文字列)
        /// </summary>
        /// <param name="path"></param>
        public void Serialize(string path)
        {
            string parent = Path.GetDirectoryName(path);
            if (!Directory.Exists(parent))
            {
                Directory.CreateDirectory(parent);
            }
            string yml = new Serializer().Serialize(this);
            File.WriteAllText(path, yml);
        }

        #endregion

        /*
        public (SyslogReceiver, SyslogReceiver) GetSyslogReceiver()
        {
            if (this.Setting == null || this.Setting.Server == null)
            {
                return (null, null);
            }
            return (this.Setting.Server.GetUdpServer(), this.Setting.Server.GetTcpServer());
        }
        */

        public SyslogMessage GetSyslogMessage(string ruleName)
        {
            var rule = this.Setting.Client.Rules[ruleName];
            return new SyslogMessage()
            {
                Format = rule.GetFormat(),
                DateTime = this.Setting.Client.GetDateTime(),
                Facility = rule.GetFacility(),
                Severity = rule.GetSeverity(),
                HostName = this.Setting.Client.GetHostName(),
                AppName = this.Setting.Client.AppName,
                ProcId = this.Setting.Client.GetProcId(),
                MsgId = this.Setting.Client.MsgId,
                Message = this.Setting.Client.Message,
            };
        }




        public SettingServerRule GetMatchServerRule(Facility facility, Severity severity)
        {
            return Setting.Server.Rules.
                FirstOrDefault(x => x.Value.IsMatch(facility, severity)).Value;
        }

        public SettingClientRule GetMatchClientRule(string name)
        {
            return Setting.Client.Rules.FirstOrDefault(x => x.Value.Equals(name)).Value;
        }
    }
}
