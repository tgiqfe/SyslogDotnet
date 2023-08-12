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
            //  ---2---------3---------4---------5---------6---------7---------8---------9--------10
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
  syslogdotnet /?
";
        }

        public static string HelpBlock3()
        {
            //  Sub Command
            //  ---2---------3---------4---------5---------6---------7---------8---------9--------10
            return @"Sub command:
  server       Syslog待ち受けモード
               以下の接続が使用可能
               - UDP
               - TCP (暗号化無し)
               - TCP (暗号化有り, クライアント証明書無し)
               - TCP (暗号化有り, クライアント証明書有り)
  client       Syslog送信
";
        }

        public static string HelpBlock4()
        {
            //  Options
            //  ---2---------3---------4---------5---------6---------7---------8---------9--------10
            return @"Options:
    server options:
    /u         UDP待ち受け。接続許可するネットワークアドレスと待ち受けポートを指定。
               使用例) 192.168.24.0:514 ⇒ 
";
        }
    }
}
