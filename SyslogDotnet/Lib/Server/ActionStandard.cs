using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyslogDotnet.Lib.Syslog;

namespace SyslogDotnet.Lib.Server
{
    internal class ActionStandard
    {
        public static void Output(SyslogMessage message)
        {
            var dt = message.DateTime.ToString("yyyy/MM/dd HH:mm:ss");
            string appName = (message.AppName == null || message.AppName == "-") ?
                "" :
                message.AppName + " : ";
            string text = $"[{dt}][{message.HostName}][{message.Facility}][{message.Severity}][{message.Format}] {appName}{message.Message}";
            Console.WriteLine(text);
        }
    }
}
