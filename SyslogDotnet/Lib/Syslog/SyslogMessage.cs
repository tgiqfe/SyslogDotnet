using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SyslogDotnet.Lib.Syslog
{
    public partial class SyslogMessage
    {
        public Format Format { get; set; }
        
        public DateTime DateTime { get; set; }
        public Facility Facility { get; set; }
        public Severity Severity { get; set; }
        public string HostName { get; set; }
        public string AppName { get; set; }
        public string ProcId { get; set; }
        public string MsgId { get; set; }
        public string Message { get; set; }
        public StructuredData[] StructuredDataParams { get; set; }

        #region for Server

        /// <summary>
        /// ログ用。リモート側(クライアント側)のIPアドレス
        /// </summary>
        public string RemoteIPAddress { get; set; }

        /// <summary>
        /// ログ用。リモート側(クライアント側)のポート番号
        /// </summary>
        public int RemotePort { get; set; }

        /// <summary>
        /// ログ用。リモート接続した通信種類
        /// </summary>
        public string TransferType { get; set; }

        #endregion
    }
}
