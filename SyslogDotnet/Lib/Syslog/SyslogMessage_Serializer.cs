using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Lib.Syslog
{
    /// <summary>
    /// SyslogMessageのbyte[]への変換について記述
    /// </summary>
    public partial class SyslogMessage
    {
        const string _version = "1";

        public byte[] Serialize()
        {
            return this.Format switch
            {
                Format.RFC3164 => Serialize_Rfc3164(),
                Format.RFC5424 => Serialize_Rfc5424(),
                _ => Serialize_Rfc3164(),
            };
        }

        private byte[] Serialize_Rfc3164()
        {
            int pri = ((int)this.Facility * 8) + (int)this.Severity;
            string month = this.DateTime.ToString("MMM", CultureInfo.InvariantCulture);
            string day = this.DateTime.Day.ToString().PadLeft(2, ' ');
            string time = this.DateTime.ToString("HH:mm:ss");
            string messageText = string.IsNullOrEmpty(this.AppName) ?
                this.Message :
                (this.AppName.Length > 32 ? this.AppName.Substring(0, 32) : this.AppName) + ":" + this.Message;

            return Encoding.UTF8.GetBytes(
                $"<{pri}>{month} {day} {time} {this.HostName} {messageText}");
        }

        private byte[] Serialize_Rfc5424()
        {
            int pri = ((int)this.Facility * 8) + (int)this.Severity;
            string timestamp = this.DateTime.ToString("yyyy-MM-ddTHH:mm:ss.ffffffK");
            string hostname = Functions.ToAsciiField(this.HostName, 255);
            string appname = Functions.ToAsciiField(this.AppName, 48);
            string procid = Functions.ToAsciiField(this.ProcId, 128);
            string msgid = Functions.ToAsciiField(this.MsgId, 32);

            string sdparam = this.StructuredDataParams?.Length > 0 ?
                string.Join("", this.StructuredDataParams.Select(x => x.Serialize())) :
                "-";

            return Encoding.UTF8.GetBytes(
                $"<{pri}>{_version} {timestamp} {hostname} {appname} {procid} {msgid} {sdparam} {this.Message}");
        }
    }
}
