

using SyslogDotnet.Lib.Syslog.Receiver;
using SyslogDotnet.Server.Cmd;
using System.Security.Cryptography.X509Certificates;

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
