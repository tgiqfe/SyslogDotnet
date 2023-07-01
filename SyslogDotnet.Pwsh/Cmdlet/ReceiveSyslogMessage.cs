using SyslogDotnet.Lib;
using SyslogDotnet.Lib.Config;
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

        [Parameter]
        public string SettingPath { get; set; }

        #endregion

        protected override void ProcessRecord()
        {
            #region set SettingCollection

            var collection = SettingCollection.Deserialize(this.SettingPath);
            collection.Setting ??= new();
            collection.Setting.SubCommand = SubCommand.Server;
            collection.Setting.Server ??= new();

            if (!string.IsNullOrEmpty(this.Udp))
            {
                collection.Setting.Server.UdpServer = this.Udp;
            }
            if (!string.IsNullOrEmpty(this.Tcp))
            {
                collection.Setting.Server.TcpServer = this.Tcp;
            }
            collection.Setting.Server.UseSsl = this.UseSsl;
            if (!string.IsNullOrEmpty(this.CertFile))
            {
                collection.Setting.Server.CertFile = this.CertFile;
            }
            if (!string.IsNullOrEmpty(this.CertPassword))
            {
                collection.Setting.Server.CertPassword = this.CertPassword;
            }
            collection.Setting.Server.ClientCertificateRequired = this.ClientCertificateRequired;
            if (!string.IsNullOrEmpty(this.PermittedPeer))
            {
                collection.Setting.Server.PermittedPeer = this.PermittedPeer;
            }

            #endregion

            using (var udp = collection.Setting.Server.GetUdpServer())
            using (var tcp = collection.Setting.Server.GetTcpServer())
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
    }
}
