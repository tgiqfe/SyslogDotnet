using SyslogDotnet.Lib.Config;
using SyslogDotnet.Lib.Syslog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Pwsh.Cmdlet
{
    [Cmdlet(VerbsCommunications.Send, "SyslogMessage")]
    public class SendSyslogMessage : PSCmdletWrapper
    {
        #region cmdlet Parameter

        [Parameter(Mandatory = true)]
        public string TargetServer { get; set; }

        [Parameter]
        public SwitchParameter UseSsl { get; set; }

        [Parameter]
        public string CertFile { get; set; }

        [Parameter]
        public string CertPassword { get; set; }

        [Parameter]
        public SwitchParameter IgnoreCheck { get; set; }

        [Parameter]
        public int? Timeout { get; set; }

        [Parameter]
        public Format? Format { get; set; }

        [Parameter]
        public Facility? Facility { get; set; }

        [Parameter]
        public Severity? Severity { get; set; }

        [Parameter(Mandatory = true)]
        public string Message { get; set; }

        [Parameter]
        public string AppName { get; set; }

        [Parameter]
        public string HostName { get; set; }

        [Parameter]
        public string ProcId { get; set; }

        [Parameter]
        public string MsgId { get; set; }

        [Parameter]
        public StructuredData[] StructuredDataParams { get; set; }

        [Parameter]
        public string SettingPath { get; set; }

        [Parameter]
        public string RuleName { get; set; }

        #endregion

        public const string TEMP_SELECTED_RULENAME = "__tempSelectedRuleName__";

        protected override void ProcessRecord()
        {
            #region set SettingCollection

            string selectedRuleName = string.IsNullOrEmpty(this.RuleName) ?
                TEMP_SELECTED_RULENAME :
                this.RuleName;

            var collection = SettingCollection.Deserialize(this.SettingPath);
            collection.Setting ??= new();
            collection.Setting.SubCommand = SubCommand.Client;
            collection.Setting.Client ??= new();
            collection.Setting.Client.SelectedRuleName = selectedRuleName;
            collection.Setting.Client.Rules ??= new();
            if (!collection.Setting.Client.Rules.ContainsKey(selectedRuleName))
            {
                collection.Setting.Client.Rules[selectedRuleName] = new();
            }

            collection.Setting.Client.TargetServer = TargetServer;
            collection.Setting.Server.UseSsl = this.UseSsl;
            collection.Setting.Server.CertFile = this.CertFile;
            collection.Setting.Server.CertPassword = this.CertPassword;
            collection.Setting.Client.SelectedRule.IgnoreCheck = this.IgnoreCheck;
            collection.Setting.Client.SelectedRule.Timeout = this.Timeout;
            collection.Setting.Client.SelectedRule.SetFormat(this.Format);
            collection.Setting.Client.SelectedRule.SetFacility(this.Facility);
            collection.Setting.Client.SelectedRule.SetSeverity(this.Severity);
            collection.Setting.Client.Message = this.Message;
            collection.Setting.Client.AppName = this.AppName;
            collection.Setting.Client.HostName = this.HostName;
            collection.Setting.Client.ProcId = this.ProcId;
            collection.Setting.Client.MsgId = this.MsgId;
            collection.Setting.Client.SetStructuredDataParams(this.StructuredDataParams);

            #endregion

            var message = collection.Setting.Client.GetSyslogMessage();
            using (var sender = collection.Setting.Client.GetSyslogSender())
            {
                if (sender != null)
                {
                    sender.Init();
                    sender.Send(message);
                }
            }
        }
    }
}
