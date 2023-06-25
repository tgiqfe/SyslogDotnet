using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using SyslogDotnet.Lib.Syslog;

namespace SyslogDotnet.Lib.Syslog.Sender
{
    public class SyslogUdpSender : SyslogSender
    {
        private UdpClient _client = null;

        public SyslogUdpSender(string server = "127.0.0.1", int port = 514)
        {
            Server = server;
            Port = port;
        }

        public override async Task SendAsync(SyslogMessage message)
        {
            _client ??= new UdpClient(this.Server, this.Port);
            var datagram = message.Serialize();
            await _client.SendAsync(datagram, datagram.Length);
        }

        public override void Close()
        {
            if (_client != null)
            {
                _client.Dispose();
            }
        }
    }
}
