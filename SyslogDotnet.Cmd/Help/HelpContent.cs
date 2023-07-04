using SyslogDotnet.Lib.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Cmd.Help
{
    internal class HelpContent
    {
        public static void Print()
        {
            Console.WriteLine(
                HelpBlock1() + "\r\n" +
                HelpBlock2() + "\r\n" +
                HelpBlock3() + "\r\n" +
                HelpBlock4());
        }

        public static string HelpBlock1()
        {
            //  Version
            //    ---------1---------2---------3---------4---------5---------6---------7---------8---------9--------10
            var version = Assembly.GetExecutingAssembly().GetName().Version;

            return $"SyslogDotnet {version.Major}.{version.Minor}.{version.Build}.{version.Revision}" + "\r\n";
        }

        public static string HelpBlock2()
        {
            //  Useage
            //  ---2---------3---------4---------5---------6---------7---------8---------9--------10
            return @"Useage:
    syslogdotnet server [/u Address:Port] [/t Address:Port] [/l] [/r CertFile]
                        [/w CertPassword] [/q] [/e PermittedPeer]
    syslogdotnet client [/s URI] [/l] [/r CertFile] [/w CertPassword] [/i] [/t Timeout]
                        [/f Format] [/c Facility] [/v Severity] [/m MEssage] [/a AppName]
                        [/b Hostname] [/d ProcId] [/j MsgId] [/x StructuredData ... ]
    syslogdotnet /?" + "\r\n2";
        }

        public static string HelpBlock3()
        {
            //  Sub Command
            //              ---------1---------2---------3---------4---------5---------6---------7---------8---------9--------10
            return @"Sub command:
";
            
            string text1 = "Sub command:" + "\r\n";
            string text2 = "    server       Syslog待ち受けモード。" + "\r\n" +
                           "                 UDP/TCP/TCP(暗号化,クライアント証明書無し)/TCP(暗号化,クライアント証明書有り)";
            string text3 = "    client       Syslog送信。";

            return text1 + "\r\n" + text2 + "\r\n" + text3 + "\r\n";
        }

        public static string HelpBlock4()
        {
            //  Options
            //             ---------1---------2---------3---------4---------5---------6---------7---------8---------9--------10

            return null;
        }
    }
}
