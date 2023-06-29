

using SyslogDotnet.Cmd;

var collection = ArgsParam.ToSettingCollection(args);
if (collection.Setting.Mode == SyslogDotnet.Lib.Config.SubCommand.Server)
{
    using (var udp = collection.Setting.Server.GetUdpServer())
    using (var tcp = collection.Setting.Server.GetTcpServer())
    {
        if (udp != null)
        {
            udp.Init();
            _ = udp.ReceiveAsync().ConfigureAwait(false);
        }
        if (tcp != null)
        {
            tcp.Init();
            _ = tcp.ReceiveAsync().ConfigureAwait(false);
        }

        if (udp != null || tcp != null)
        {
            Console.WriteLine("Syslog待ち受け中...");
            Console.ReadLine();
        }
    }
}
else if (collection.Setting.Mode == SyslogDotnet.Lib.Config.SubCommand.Client)
{
    var message = collection.Setting.Client.GetSyslogMessage();
    using (var sender = collection.Setting.Client.GetSyslogSender())
    {
        if(sender != null)
        {
            sender.Init();
            sender.Send(message);
        }
    }
}

