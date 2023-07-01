using SyslogDotnet.Lib.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Pwsh.Cmdlet
{
    [Cmdlet(VerbsCommon.Set, "SyslogSetting")]
    public class SetSyslogSetting : PSCmdletWrapper
    {
        [Parameter(Position = 0)]
        public SubCommand SubCommand { get; set; }

        [Parameter(Position = 1)]
        public string SettingPath { get; set; }

        [Parameter]
        public string RuleName { get; set; }

        [Parameter]
        public string OutputPath { get; set; }

        [Parameter]
        public string Udp { get; set; }

        [Parameter]
        public string Tcp { get; set; }

        [Parameter]
        public string TargetServer { get; set; }

        [Parameter]
        public SwitchParameter UseSsl { get; set; }

        [Parameter]
        public string CertFile { get; set; }

        [Parameter]
        public string CertPassword { get; set; }

        [Parameter]
        public SwitchParameter ClientCertificateRequired { get; set; }

        [Parameter]
        public string PermittedPeer { get; set; }

        [Parameter]
        public SwitchParameter IgnoreCheck { get; set; }

        [Parameter]
        public int? Timeout { get; set; }

        protected override void ProcessRecord()
        {
            var collection = SettingCollection.Deserialize(this.SettingPath);
            collection.Setting ??= new();
            collection.Setting.SubCommand = this.SubCommand;

            if (SubCommand == SubCommand.Server)
            {
                collection.Setting.Server ??= new();
            }
            else if (SubCommand == SubCommand.Client && !string.IsNullOrEmpty(this.RuleName))
            {
                string selectedRuleName = this.RuleName;

                collection.Setting.Client ??= new();
                collection.Setting.Client.SelectedRuleName = selectedRuleName;
                collection.Setting.Client.Rules ??= new();
                if (!collection.Setting.Client.Rules.ContainsKey(selectedRuleName))
                {
                    collection.Setting.Client.Rules[selectedRuleName] = new();
                }
            }

            if (string.IsNullOrEmpty(this.OutputPath))
            {
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
            else
            {
                collection.Serialize(this.OutputPath);
            }
        }
    }
}
