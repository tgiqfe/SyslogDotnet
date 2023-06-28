using SyslogDotnet.Lib;
using SyslogDotnet.Lib.Syslog.Receiver;
using System.Management.Automation;

namespace SyslogDotnet.Pwsh.Cmdlet
{
    [Cmdlet(VerbsCommunications.Receive, "SyslogMessage")]
    public class ReceiveSyslogMessage : PSCmdletWrapper
    {
        #region cmdlet Parameter

        [Parameter]
        public string Udp { get; set; }

        [Parameter]
        public string Tcp { get; set; }

        [Parameter]
        public SwitchParameter UseSsl { get; set; }

        [Parameter]
        public string CertFile { get; set; }

        [Parameter]
        public string CertPassword { get; set; }

        [Parameter]
        public SwitchParameter ClientCertificateRequired { get; set; }

        [Parameter]
        public string PermittedPeer { get; set; }

        #endregion

        protected override void ProcessRecord()
        {
            //  UDP待ち受け
            if (!string.IsNullOrEmpty(this.Udp))
            {
                var sv = new ServerInfo(this.Udp);
                var udp = new SyslogUdpReceiver(sv.Server, sv.Port);
                udp.Init();
                _ = udp.ReceiveAsync().ConfigureAwait(false);
            }

            //  TCP待ち受け
            if (!string.IsNullOrEmpty(this.Tcp))
            {
                var sv = new ServerInfo(this.Tcp);
                if (this.UseSsl)
                {
                    if (this.ClientCertificateRequired)
                    {
                        var tcp = new SyslogTcpReceiverTLS(sv.Server, sv.Port)
                        {
                            CertFile = this.CertFile,
                            CertPassword = this.CertPassword,
                            ClientCertificateRequired = this.ClientCertificateRequired,
                            PermittedPeer = this.PermittedPeer,
                        };
                        tcp.Init();
                        _ = tcp.ReceiveAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        var tcp = new SyslogTcpReceiverTLS(sv.Server, sv.Port)
                        {
                            CertFile = this.CertFile,
                            CertPassword = this.CertPassword,
                        };
                        tcp.Init();
                        _ = tcp.ReceiveAsync().ConfigureAwait(false);
                    }
                }
                else
                {
                    var tcp = new SyslogTcpReceiver(sv.Server, sv.Port);
                    tcp.Init();
                    _ = tcp.ReceiveAsync().ConfigureAwait(false);
                }
            }

            Console.ReadLine();
        }
    }
}
