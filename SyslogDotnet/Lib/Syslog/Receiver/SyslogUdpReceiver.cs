using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SyslogDotnet.Lib.Syslog;

namespace SyslogDotnet.Lib.Syslog.Receiver
{
    public class SyslogUdpReceiver : SyslogReceiver
    {
        private UdpClient _client = null;

        public SyslogUdpReceiver(string ipAddress = "0.0.0.0", int port = 514)
        {
            this.LocalAddress = IPAddress.TryParse(ipAddress, out IPAddress address) ?
                address :
                IPAddress.Any;
            this.LocalPort = port;
        }

        public override async Task ReceiveAsync()
        {
            _client = new UdpClient(new IPEndPoint(LocalAddress, LocalPort));

            try
            {
                while (true)
                {
                    var ret = await _client.ReceiveAsync();
                    var address = ret.RemoteEndPoint.Address;
                    var port = ret.RemoteEndPoint.Port;

                    var sysMsg = SyslogMessage.Deserialize(ret.Buffer);
                    _ = Task.Run(() =>
                    {
                        ReceiveProcess(sysMsg);
                    });
                }
            }
            catch (SocketException)
            {
                Console.WriteLine("UDP待ち受け終了");
            }
        }

        public override void Close()
        {
            _client?.Dispose();
        }
    }
}
