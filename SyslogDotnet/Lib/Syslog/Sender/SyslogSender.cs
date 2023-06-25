using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyslogDotnet.Lib.Syslog;

namespace SyslogDotnet.Lib.Syslog.Sender
{
    public class SyslogSender : IDisposable
    {
        /// <summary>
        /// Syslog送信先サーバのアドレス
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Syslog送信先サーバのポート番号
        /// </summary>
        public int Port { get; set; }

        public virtual void Init() { }
        
        public void Send(SyslogMessage message) { SendAsync(message).Wait(); }
        
        public virtual async Task SendAsync(SyslogMessage message)
        {
            await Task.Run(() => { });
        }

        public virtual void Close() { }

        #region Dispose

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Close();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
