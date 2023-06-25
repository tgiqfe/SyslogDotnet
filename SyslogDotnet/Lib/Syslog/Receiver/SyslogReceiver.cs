using System.Net;
using SyslogDotnet.Lib.Server;

namespace SyslogDotnet.Lib.Syslog.Receiver
{
    public class SyslogReceiver : IDisposable
    {
        /// <summary>
        /// サーバ側で待ち受けるアドレス
        /// </summary>
        public IPAddress LocalAddress { get; set; }

        /// <summary>
        /// サーバ側で待ち受けるポート番号
        /// </summary>
        public int LocalPort { get; set; }

        public virtual void Init() { }

        public void Receive() { ReceiveAsync().Wait(); }
        
        public virtual async Task ReceiveAsync() { await Task.Run(() => { }); }

        /// <summary>
        /// Syslog受信時の処理
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        /// <param name="message"></param>
        public void ReceiveProcess(SyslogMessage message)
        {
            //Console.WriteLine(message);
            ActionStandard.Output(message);
            ActionJsonLog.Output(message, "jsonlog.txt");
        }

        public virtual void Close() { }

        #region Dispose

        bool disposedValue = false;

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
