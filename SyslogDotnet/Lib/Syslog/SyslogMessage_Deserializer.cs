using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyslogDotnet.Lib.Syslog
{
    /// <summary>
    /// byte[]配列からSyslogMessageへの変換について記述
    /// </summary>
    public partial class SyslogMessage
    {
        public static SyslogMessage Deserialize(byte[] datagram)
        {
            string text = Encoding.UTF8.GetString(datagram);

            var list = text.Split(' ').ToList();
            SyslogMessage message = new();

            if (list?.Count > 0)
            {
                //  最初のフィールドがメッセージサイズの場合、無視
                //  (Format RFC5424の場合のみ)
                if (int.TryParse(list[0], out int num)) { list.RemoveAt(0); }

                if (list.Count > 0)
                {
                    if (list[0].StartsWith("<") && list[0].Contains(">"))
                    {
                        string priText = list[0].Substring(1, list[0].IndexOf(">") - 1);
                        list[0] = list[0].Substring(list[0].IndexOf(">") + 1);
                        if (int.TryParse(priText, out int pri))
                        {
                            message.Facility = FacilityMapper.ToFacilityFromPri(pri);
                            message.Severity = SeverityMapper.ToSeverityFromPri(pri);

                            if (DateTime.TryParse(list[1], out DateTime dt5424))
                            {
                                //  Fieldのインデックス1だけで日付判定可能な場合⇒RFC5424
                                message.Format = Format.RFC5424;
                                message.DateTime = dt5424;
                                message.HostName = list[2];
                                message.AppName = list[3];
                                message.ProcId = list[4];
                                message.MsgId = list[5];
                                //message.StructuredDataParams = null;  (list[6])
                                message.Message = string.Join(" ", list.Skip(7));
                            }
                            else if (DateTime.TryParse($"{DateTime.Now.Year} {list[0]} {list[1]} {list[2]}", out DateTime dt3164))
                            {
                                //  Fieldのインデックス0,1,2に年を追加することで日付判定可能な場合⇒RFC3164
                                message.Format = Format.RFC3164;
                                message.DateTime = dt3164;
                                message.HostName = list[3];
                                string messageText = string.Join(" ", list.Skip(4));
                                if (messageText.Contains(":"))
                                {
                                    //  「:」を含む場合は、AppNameとMessageを分けておく。
                                    message.AppName = messageText.Substring(0, messageText.IndexOf(":"));
                                    message.Message = messageText.Substring(messageText.IndexOf(":") + 1);
                                }
                                else
                                {
                                    message.Message = messageText;
                                }
                            }
                        }
                    }
                }
            }

            return message;
        }
    }
}
