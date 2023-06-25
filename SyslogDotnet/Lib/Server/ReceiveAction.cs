namespace SyslogDotnet.Lib.Server
{
    [Flags]
    internal enum ReceiveAction
    {
        None = 0,
        Standard = 1,
        FileLog = 2,
        JsonLog = 4,
        LiteDB = 8,
        Syslog = 16,
        Logstash = 32,
    }
}
