

using SyslogDotnet.Server.Cmd;

(var subCommand, var collection) = ArgsParam.ToSettingCollection(args);
if (subCommand == ArgsParam.Subcommand.Server)
{
    (var udp, var tcp) = collection.GetSyslogReceiver();
    using (udp)
    using (tcp)
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
else if (subCommand == ArgsParam.Subcommand.Client)
{

}

