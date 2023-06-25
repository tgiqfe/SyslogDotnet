using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace SyslogDotnet.Lib.Syslog.Receiver
{
    public class SyslogTcpReceiverTLS : SyslogReceiver
    {
        private TcpListener _listener = null;
        private X509Certificate2 _serverCertificate = null;
        private string _rootThumbprint = null;

        #region Public parameter

        /// <summary>
        /// SSL接続に使用するサーバ証明書。
        /// (PKCS12)
        /// </summary>
        public string CertFile { get; set; }

        /// <summary>
        /// サーバ証明書のパスワード
        /// </summary>
        public string CertPassword { get; set; }

        /// <summary>
        /// クライアント証明書を必要とするかどうか
        /// </summary>
        public bool ClientCertificateRequired { get; set; }

        /// <summary>
        /// クライアント証明書と照会する為のCN名
        /// </summary>
        public string PermittedPeer { get; set; }

        #endregion

        public SyslogTcpReceiverTLS(string ipAddress = "0.0.0.0", int port = 514)
        {
            this.LocalAddress = IPAddress.TryParse(ipAddress, out IPAddress address) ?
                address :
                IPAddress.Any;
            this.LocalPort = port;
        }

        public override async Task ReceiveAsync()
        {
            _serverCertificate = new X509Certificate2(CertFile, CertPassword);
            var chain = new X509Chain();
            chain.Build(_serverCertificate);
            _rootThumbprint = chain.ChainElements.Last().Certificate.Thumbprint;

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
                    using (var ss = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(RemoteCertificateValidationCallback), null))
                    using (var ms = new MemoryStream())
                    {
                        ss.ReadTimeout = 1000;
                        try
                        {
                            ss.AuthenticateAsServer(_serverCertificate, ClientCertificateRequired, SslProtocols.Tls12 | SslProtocols.Tls13, false);

                            byte[] buffer = new byte[1024];
                            int retSize = -1;
                            do
                            {
                                retSize = ss.Read(buffer, 0, buffer.Length);
                                ms.Write(buffer, 0, retSize);
                            } while (retSize != 0);
                        }
                        catch { /* クライアント側でSSLセッションを閉じるときに必ず例外発生 */ }

                        if (ms.Length > 0)
                        {
                            sysMsg = SyslogMessage.Deserialize(ms.ToArray());
                            sysMsg.RemoteIPAddress = address.ToString();
                            sysMsg.RemotePort = port;
                            sysMsg.TransferType = "TCP,SSL" + (ClientCertificateRequired ? ",ClCrtReq" : "");
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

        private bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            //  クライアント証明書が不要の場合はtrue
            if (!ClientCertificateRequired) { return true; }

            //  クライアント証明書が必要だが、送られてこなければfalse
            if (certificate == null) { return false; }

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

            //  以下のルールにマッチしたらtrue
            //  - クライアント証明書とサーバ側の証明書のそれぞれのルート証明書の、署名(Thumbprint)が一致
            //  - クライアント証明書のCN名が、事前定義した値と一致している
            var clcert = certificate as X509Certificate2;
            if (chain.Build(clcert))
            {
                var root = chain.ChainElements.Last().Certificate;
                if (root.Thumbprint != _rootThumbprint)
                {
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(this.PermittedPeer))
            {
                string cnName = clcert.SubjectName.Name.Split(",").
                    Select(x => x.Trim()).
                    FirstOrDefault(x => x.StartsWith("CN=") || x.StartsWith("cn="));
                if (cnName.Substring(3) != this.PermittedPeer)
                {
                    return false;
                }
            }

            return true;
        }

        public override void Close()
        {
            _serverCertificate?.Dispose();
            _listener?.Stop();
        }
    }
}
