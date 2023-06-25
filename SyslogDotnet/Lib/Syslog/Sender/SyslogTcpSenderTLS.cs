using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Security.Authentication;
using System.Threading;
using SyslogDotnet.Lib.Syslog;

namespace SyslogDotnet.Lib.Syslog.Sender
{
    public class SyslogTcpSenderTLS : SyslogSender
    {
        private enum MessageTransfer
        {
            OctetCouting,
            NonTransportFraming,
        }

        #region public Parameter

        /// <summary>
        /// サーバ接続成功までの最大待ち時間
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// クライアント証明書を使用する場合の証明書ファイルへのパス
        /// (PKCS12)
        /// </summary>
        public string CertFile { get; set; }

        /// <summary>
        /// クライアント証明書を使用する場合の証明書のパスワード
        /// </summary>
        public string CertPassword { get; set; }

        /// <summary>
        /// インストール済みの証明書を使用する場合の証明書フレンドリ名
        /// </summary>
        public string CertFriendryName { get; set; }

        /// <summary>
        /// クライアント証明書を使用しない場合で、サーバ側証明書のチェックをスキップ
        /// </summary>
        public bool IgnoreCheck { get; set; }

        #endregion

        private TcpClient _client = null;
        private SslStream _stream = null;
        private MessageTransfer _messageTransfer { get; set; }

        public SyslogTcpSenderTLS(string server = "127.0.0.1", int port = 514, bool octetCounting = true)
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
                _stream = new SslStream(_client.GetStream(), false, RemoteCertificateValidationCallback)
                {
                    ReadTimeout = Timeout,
                    WriteTimeout = Timeout
                };
                _stream.AuthenticateAsClient(
                    Server,
                    GetCollection(),
                    SslProtocols.Tls12 | SslProtocols.Tls13,
                    false);

                if (!_stream.IsEncrypted)
                {
                    throw new SecurityException("Could not establish an encrypted connection.");
                }
            }
            catch
            {
                Disconnect();
            }
        }

        private X509Certificate2Collection GetCollection()
        {
            var collection = new X509Certificate2Collection();
            if (!string.IsNullOrEmpty(CertFile) && !string.IsNullOrEmpty(CertPassword) && File.Exists(CertFile))
            {
                collection.Add(new X509Certificate2(CertFile, CertPassword));
            }
            else if (!string.IsNullOrEmpty(CertFriendryName))
            {
                var myCert = new X509Store(StoreName.My, StoreLocation.CurrentUser, OpenFlags.ReadOnly).Certificates.
                    FirstOrDefault(x => x.FriendlyName == CertFriendryName);
                if (myCert == null)
                {
                    new X509Store(StoreName.My, StoreLocation.LocalMachine, OpenFlags.ReadOnly).Certificates.
                        FirstOrDefault(x => x.FriendlyName == CertFriendryName);
                }
                if (myCert != null)
                {
                    collection.Add(myCert);
                }
            }

            return collection;
        }

        private bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            //  デバッグ用
#if DEBUG
            /*
            Console.WriteLine("===========================================");
            Console.WriteLine("Subject={0}", certificate.Subject);
            Console.WriteLine("Issuer={0}", certificate.Issuer);
            Console.WriteLine("Format={0}", certificate.GetFormat());
            Console.WriteLine("ExpirationDate={0}", certificate.GetExpirationDateString());
            Console.WriteLine("EffectiveDate={0}", certificate.GetEffectiveDateString());
            Console.WriteLine("KeyAlgorithm={0}", certificate.GetKeyAlgorithm());
            Console.WriteLine("PublicKey={0}", certificate.GetPublicKeyString());
            Console.WriteLine("SerialNumber={0}", certificate.GetSerialNumberString());
            Console.WriteLine("===========================================");
            */
#endif
            if (IgnoreCheck || sslPolicyErrors == SslPolicyErrors.None)
            {
                //  Successful verification of server certificate.
                return true;
            }
            else
            {
                //  ChainStatus returned a non-empty array.
                if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) == SslPolicyErrors.RemoteCertificateChainErrors) { }
                //  Certificate names do not match.
                if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNameMismatch) == SslPolicyErrors.RemoteCertificateNameMismatch) { }
                //  Certificate not available.
                if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNotAvailable) == SslPolicyErrors.RemoteCertificateNotAvailable) { }

                return false;
            }
        }

        public void Disconnect()
        {
            if (_stream != null) { _stream.Dispose(); }
            if (_client != null) { _client.Dispose(); }
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

        public override void Close()
        {
            Disconnect();
        }
    }
}
