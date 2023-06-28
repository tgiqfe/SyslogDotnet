using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using static SyslogDotnet.Lib.Config.Setting;

namespace SyslogDotnet.Lib.Config
{
    public class SettingClient
    {
        [YamlIgnore]
        public string TargetServer { get; set; }

        [YamlIgnore]
        public string HostName { get; set; }

        [YamlIgnore]
        public string AppName { get; set; }

        [YamlIgnore]
        public string ProcId { get; set; }

        [YamlIgnore]
        public string MsgId { get; set; }

        [YamlIgnore]
        public string Message { get; set; }

        
        //  ★データ型等未定。
        [YamlIgnore]
        public string StructuredDataParams { get; set; }


        #region Serialize parameter

        public Dictionary<string, SettingClientRule> Rules { get; set; }

        #endregion

        /// <summary>
        /// Syslogメッセージに含めるホスト名。
        /// nullの場合は自マシンのホスト名を返す。
        /// </summary>
        /// <returns></returns>
        public string GetHostName()
        {
            return string.IsNullOrEmpty(this.HostName) ?
                Environment.MachineName :
                this.HostName;
        }

        /// <summary>
        /// Syslogメッセージに含めるプロセスID。
        /// nullの場合は、自プロセスのIDを返す。
        /// </summary>
        /// <returns></returns>
        public string GetProcId()
        {
            return string.IsNullOrEmpty(this.ProcId) ?
                Process.GetCurrentProcess().Id.ToString() :
                this.ProcId;
        }
    }
}
