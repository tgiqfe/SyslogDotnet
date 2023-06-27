using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SyslogDotnet.Lib
{
    /// <summary>
    /// URIからサーバアドレス(IP or FQDN)、ポート、プロトコルを格納
    /// </summary>
    public class ServerInfo
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Protocol { get; set; }

        public ServerInfo() { }

        public ServerInfo(string uri)
        {
            string tempServer = uri;
            string tempPort = "0";
            string tempProtocol = "";

            Match match;
            if ((match = Regex.Match(tempServer, "^.+(?=://)")).Success)
            {
                tempProtocol = match.Value;
                tempServer = tempServer.Substring(tempServer.IndexOf("://") + 3);
            }
            if ((match = Regex.Match(tempServer, @"(?<=:)\d+")).Success)
            {
                tempPort = match.Value;
                tempServer = tempServer.Substring(0, tempServer.IndexOf(":"));
            }

            this.Server = ResolvServerName(tempServer);
            this.Port = int.Parse(tempPort);
            this.Protocol = tempProtocol.ToLower();
        }

        public ServerInfo(string uri, int defaultPort, string defaultProtocol) : this(uri)
        {
            if (Port == 0) { this.Port = defaultPort; }
            if (string.IsNullOrEmpty(Protocol)) { this.Protocol = defaultProtocol.ToLower(); }
        }

        public string ResolvServerName(string text)
        {
            if (text == "localhost") { return "127.0.0.1"; }

            return text;
        }

        public override string ToString()
        {
            return $"{Protocol}://{Server}:{Port}";
        }
    }
}
