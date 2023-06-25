using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Lib.Syslog
{
    /// <summary>
    /// RFC5424用 構造化データ(Strctured-Data)
    /// </summary>
    public class StructuredData
    {
        public const int DefaultPrivateEnterpriseNumber = 32473;

        public string SdId = null;

        public Dictionary<string, string> SdParam = null;
        
        public StructuredData() { }

        public StructuredData(string sdId, Dictionary<string, string> sdParam)
        {
            this.SdId = sdId.Contains("@") ? sdId : $"{sdId}@{DefaultPrivateEnterpriseNumber}";
            this.SdParam = sdParam;
        }

        public string Serialize()
        {
            string sdid = Functions.ToAsciiField(this.SdId, 32, sdName: true);
            var items = this.SdParam == null ?
                new string[0] { } :
                this.SdParam.Select(x =>
                {
                    string key = Functions.ToAsciiField(x.Key, 32, sdName: true);
                    string val = x.Value == null ? "" :
                        x.Value.
                            Replace("\\", "\\\\").
                            Replace("\"", "\\\"").
                            Replace("]", "\\]");
                    return $"{key}=\"{val}\"";
                });

            return $"[{sdid} {string.Join(" ", items)}]";
        }
    }
}
