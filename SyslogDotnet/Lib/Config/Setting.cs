using SyslogDotnet.Lib.Syslog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace SyslogDotnet.Lib.Config
{
    public class Setting
    {
        [YamlIgnore]
        public SubCommand Mode { get; set; }

        /// <summary>
        /// サーバ用設定
        /// </summary>
        public SettingServer Server { get; set; }

        /// <summary>
        /// クライアント用設定
        /// </summary>
        public SettingClient Client { get; set; }
    }
}
