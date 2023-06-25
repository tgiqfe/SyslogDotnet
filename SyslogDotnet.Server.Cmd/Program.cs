

using SyslogDotnet.Lib.Syslog.Receiver;
using SyslogDotnet.Server.Cmd;
using System.Security.Cryptography.X509Certificates;

/*
using (var tls = new SyslogTcpReceiverTLS())
{
    tls.CertFile = @"cert3.pfx";
    tls.CertPassword = "1234";
    tls.ClientCertificateRequired = true;
    tls.PermittedPeer = "tqWin04";
    tls.ReceiveAsync().Wait();
}


Console.ReadLine();
*/


var ap = new ArgsParam(args);
using (var udp = ap.ToUdpReceiver())
using (var tcp = ap.ToTcpReceiver())
{
    if(udp != null)
    {
        udp.Init();
        _ = udp.ReceiveAsync().ConfigureAwait(false);
    }
    if(tcp != null)
    {
        tcp.Init();
        _ = tcp.ReceiveAsync().ConfigureAwait(false);
    }

    Console.ReadLine();
}



/*
 
出力先
- コンソールへ標準出力
- ファイルへ出力
- ファイルへ行ごとJSONで出力
- LiteDBへ出力
- 別Syslogサーバへ出力
- Logstashへ転送

*/