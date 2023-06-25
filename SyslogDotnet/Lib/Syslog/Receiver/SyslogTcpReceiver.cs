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
    public class SyslogTcpReceiver : SyslogReceiver
    {
        private TcpListener _listener = null;

        public SyslogTcpReceiver(string ipAddress = "0.0.0.0", int port = 514)
        {
            this.LocalAddress = IPAddress.TryParse(ipAddress, out IPAddress address) ?
                address :
                IPAddress.Any;
            this.LocalPort = port;
        }

        public override async Task ReceiveAsync()
        {
            _listener = new TcpListener(LocalAddress, LocalPort);
            _listener.Start();
            try
            {
                while (true)
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    var address = ((IPEndPoint)client.Client.RemoteEndPoint).Address;
                    var port = ((IPEndPoint)client.Client.RemoteEndPoint).Port;

                    SyslogMessage sysMsg = null;
                    using (var ns = client.GetStream())
                    using (var ms = new MemoryStream())
                    {
                        byte[] buffer = new byte[1024];
                        do
                        {
                            int retSize = await ns.ReadAsync(buffer, 0, buffer.Length);
                            ms.Write(buffer, 0, retSize);

                        } while (ns.DataAvailable);
                        
                        if (ms.Length > 0)
                        {
                            sysMsg = SyslogMessage.Deserialize(ms.ToArray());
                            sysMsg.RemoteIPAddress = address.ToString();
                            sysMsg.RemotePort = port;
                            _ = Task.Run(() =>
                            {
                                ReceiveProcess(sysMsg);
                            });
                        }
                    }
                }
            }
            catch (SocketException)
            {
                Console.WriteLine("TCP待ち受け終了");
            }
        }

        public override void Close()
        {
            _listener?.Stop();
        }
    }
}
