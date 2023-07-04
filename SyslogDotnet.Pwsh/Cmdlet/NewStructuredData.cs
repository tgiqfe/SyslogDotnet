using SyslogDotnet.Lib.Syslog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Pwsh.Cmdlet
{
    [Cmdlet(VerbsCommon.New, "StructuredData")]
    public class NewStructuredData : PSCmdletWrapper
    {
        [Parameter]
        public string SdId { get; set; }

        [Parameter]
        public Hashtable SdParam { get; set; }

        protected override void ProcessRecord()
        {
            if (string.IsNullOrEmpty(this.SdId))
            {
                WriteObject(null);
            }
            else
            {
                var sdParam = new Dictionary<string, string>();
                if (SdParam != null)
                {
                    foreach (DictionaryEntry item in SdParam)
                    {
                        sdParam[item.Key.ToString()] = item.Value.ToString();
                    }
                }

                WriteObject(new StructuredData(this.SdId, sdParam));
            }
        }
    }
}
