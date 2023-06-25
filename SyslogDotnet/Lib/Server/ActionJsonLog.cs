using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using SyslogDotnet.Lib.Syslog;

namespace SyslogDotnet.Lib.Server
{
    internal class ActionJsonLog
    {
        public static void Output(SyslogMessage message, string outputPath)
        {   
            string json = JsonSerializer.Serialize(message, new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            });
            using (var sw = new StreamWriter(outputPath, true, Encoding.UTF8))
            {
                sw.WriteLine(json);
            }
        }
    }
}
