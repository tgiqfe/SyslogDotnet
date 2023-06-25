using System.Net.Sockets;
using System.Text;

namespace SyslogDotnet.Lib.Syslog.Sender
{
    public class SyslogTcpSender : SyslogSender
    {
        private enum MessageTransfer
        {
            OctetCouting,
            NonTransportFraming,
        }

        private MessageTransfer _messageTransfer { get; set; }
        private TcpClient _client = null;
        private NetworkStream _stream = null;

        public SyslogTcpSender(string server = "127.0.0.1", int port = 514, bool octetCounting = true)
        {
            Server = server;
            Port = port;
            _messageTransfer = octetCounting ?
                MessageTransfer.OctetCouting :
                MessageTransfer.NonTransportFraming;
        }

        public override void Init()
        {
            this.Connect();
        }

        public void Connect()
        {
            try
            {
                _client = new TcpClient(Server, Port);
                _stream = _client.GetStream();
            }
            catch
            {
                Disconnect();
            }
        }

        public void Disconnect()
        {
            if (_stream != null) { _stream.Dispose(); }
            if (_client != null) { _client.Dispose(); }
        }

        public override void Close()
        {
            Disconnect();
        }

        public override async Task SendAsync(SyslogMessage message)
        {
            if (_stream == null)
            {
                throw new IOException("No transport stream.");
            }

            using (var ms = new MemoryStream())
            {
                var datagram = message.Serialize();

                if (_messageTransfer == MessageTransfer.OctetCouting)
                {
                    byte[] messageLength = Encoding.UTF8.GetBytes(datagram.Length.ToString());
                    ms.Write(messageLength, 0, messageLength.Length);
                    ms.WriteByte(32);   //  0x20 Space
                }
                ms.Write(datagram, 0, datagram.Length);

                if (_messageTransfer == MessageTransfer.NonTransportFraming)
                {
                    ms.WriteByte(10);   //  0xA LF
                }
                await _stream.WriteAsync(ms.GetBuffer(), 0, (int)ms.Length);
                await _stream.FlushAsync();
            }
        }
    }
}
