using SyslogDotnet.Lib.Syslog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Lib.Server
{
    internal class ActionFileLog
    {
        private readonly static object _lock = new object();

        public static void Output(SyslogMessage message, string outputPath)
        {
            var dt = message.DateTime.ToString("yyyy/MM/dd HH:mm:ss");
            string appName = (message.AppName == null || message.AppName == "-") ?
                "" :
                message.AppName + " : ";
            string text = $"[{dt}][{message.HostName}][{message.Facility}][{message.Severity}][{message.Format}] {appName}{message.Message}";

            lock (_lock)
            {
                using (var sw = new StreamWriter(outputPath, true, Encoding.UTF8))
                {
                    sw.WriteLine(text);
                }
            }
        }
    }
}
