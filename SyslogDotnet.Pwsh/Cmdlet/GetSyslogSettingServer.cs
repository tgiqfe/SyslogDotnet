using SyslogDotnet.Lib.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Pwsh.Cmdlet
{
    [Cmdlet(VerbsCommon.Get, "SyslogSettingServer")]
    public class GetSyslogSettingServer : PSCmdletWrapper
    {
        [Parameter]
        public string SettingPath { get; set; }

        protected override void ProcessRecord()
        {
            var collection = SettingCollection.Deserialize(this.SettingPath);

            WriteObject(collection?.Setting?.Server);
        }
    }
}
