using System.Text.RegularExpressions;

namespace SyslogDotnet.Lib.Syslog
{
    /// <summary>
    /// RFC5424用 構造化データ(Strctured-Data)
    /// </summary>
    public class StructuredData
    {
        const int DefaultPrivateEnterpriseNumber = 32473;

        #region Serialize parameter

        public string SdId { get; set; }

        public Dictionary<string, string> SdParam { get; set; }

        #endregion

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

        public static StructuredData[] Deserialize(string text)
        {
            if (string.IsNullOrEmpty(text)) { return null; }

            var patternA = new Regex(@"\]\s*\[");
            var patternB = new Regex(@"\s+(?=(?:[^""]*""[^""]*"")*[^""]*$)");

            text = text.Substring(text.IndexOf("[") + 1);
            text = text.Substring(0, text.LastIndexOf("]"));
            var list = new List<StructuredData>();
            foreach (var sdText in patternA.Split(text))
            {
                var fields = patternB.Split(sdText);
                if (fields.Length > 0)
                {
                    var sd = new StructuredData(
                        fields[0],
                        fields.Skip(1).
                            Where(x => x.Contains("=")).
                        ToDictionary(x => x.Substring(0, x.IndexOf("=")), x => x.Substring(x.IndexOf("=") + 1).Trim('\"')));
                    list.Add(sd);
                }
            }

            return list.ToArray();
        }
    }
}
