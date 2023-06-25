using SyslogDotnet.Client.Cmd;

var ap = new ArgsParam(args);

var msg = ap.ToSyslogMessage();
using (var sender = ap.ToSyslogSender())
{
    sender.Init();
    sender.Send(msg);
}
