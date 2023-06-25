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

        /// <summary>
        /// サーバ側で使用。リモート側(クライアント側)のIPアドレス
        /// </summary>
        public string RemoteIPAddress { get; set; }

        /// <summary>
        /// サーバ側で使用。リモート側(クライアント側)のポート番号
        /// </summary>
        public int RemotePort { get; set; }

        public SyslogMessage() { }

        /*
        public SyslogMessage(
            Facility facility,
            Severity severity,
            string appName,
            string message) :
            this(DateTime.Now, facility, severity, Environment.MachineName, appName, Process.GetCurrentProcess().Id.ToString(), "", message)
        { }

        public SyslogMessage(
            DateTime dt,
            Facility facility,
            Severity severity,
            string hostName,
            string appName,
            string procId,
            string msgId,
            string message,
            params StructuredData[] StructuredDatas)
        {
            this.DateTime = dt;
            this.Facility = facility;
            this.Severity = severity;
            this.HostName = hostName;
            this.AppName = appName;
            this.ProcId = procId;
            this.MsgId = msgId;
            this.Message = message;
            this.StructuredDataParams = StructuredDatas;
        }
        */

        /*
        public override string ToString()
        {
            var dt = this.DateTime.ToString("yyyy/MM/dd HH:mm:ss");
            string appName = (this.AppName == null  || this.AppName == "-") ?
                "" : 
                this.AppName + " : ";
            return $"[{dt}][{this.HostName}][{this.Facility}][{this.Severity}][{this.Format}] {appName}{this.Message}";
        }
        */
    }
}
