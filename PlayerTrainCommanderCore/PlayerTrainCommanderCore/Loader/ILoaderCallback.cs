using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTC.Core.Loader
{
    /// <summary>
    /// 読み取り時に発生したメッセージをコールバックする機能を公開します。
    /// </summary>
    public interface ILoaderCallback
    {
        /// <inheritdoc cref="WriteError(string, Exception)"/>
        public void WriteError(string message);
        /// <summary>
        /// エラー文を書き込みます。
        /// </summary>
        /// <param name="message">エラー文を表す文字列。</param>
        /// <param name="exception">例外を示す <see cref="Exception"/> 型のオブジェクト。</param>
        public void WriteError(string message, Exception exception);
        /// <summary>
        /// 警告文を書き込みます。
        /// </summary>
        /// <param name="message">警告文を表す文字列。</param>
        public void WriteWarning(string message);
        /// <summary>
        /// メッセージを書き込みます。
        /// </summary>
        /// <param name="message">メッセージを表す文字列。</param>
        public void WriteMessage(string message);
    }
}
