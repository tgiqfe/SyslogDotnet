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

        [Parameter]
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
        public int Timeout { get; set; }

        [Parameter]
        public Format Format { get; set; }

        [Parameter]
        public Facility Facility { get; set; }

        [Parameter]
        public Severity Severity { get; set; }

        [Parameter]
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

        #endregion

        protected override void ProcessRecord()
        {
            
        }
    }
}
