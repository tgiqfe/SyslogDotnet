using SyslogDotnet.Lib.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Pwsh.Cmdlet
{
    [Cmdlet(VerbsCommon.Get, "Syslog")]
    public class GetSyslogSetting : PSCmdletWrapper
    {
        [Parameter(Position = 0)]
        public SubCommand SubCommand { get; set; }

        [Parameter(Position = 1)]
        public string SettingPath { get; set; }

        protected override void ProcessRecord()
        {
            var collection = SettingCollection.Deserialize(this.SettingPath);

            object ret = collection == null ?
                null :
                this.SubCommand switch
                {
                    SubCommand.Server => collection.Setting.Server,
                    SubCommand.Client => collection.Setting.Client,
                    _ => null,
                };
            WriteObject(ret);
        }
    }
}
