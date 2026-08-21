using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PTC.Core
{
    /// <summary>
    /// 動的に情報が変化する列車を管理するクラス。
    /// </summary>
    public class Train : INotifyPropertyChanged
    {
        #region フィールド変数
        /// <summary>
        /// 現在の列車状態。
        /// </summary>
        protected TrainState state;
        /// <summary>
        /// 同期処理に使用する <see cref="System.Threading.SpinLock"/> 構造体。
        /// </summary>
        protected System.Threading.SpinLock spinLock;
        #endregion
        #region Get プロパティ

        /// <summary>
        /// 現在の列車の状態を示す値を取得します。
        /// </summary>
        /// <returns>
        /// 現在の列車の状態を示す <see cref="TrainState"/> 型の値。
        /// </returns>
        /// <remarks>
        /// このメソッドはスレッドセーフです。
        /// </remarks>
        public TrainState State
        {
            get
            {
                bool gotLock = false;
                try
                {
                    spinLock.Enter(ref gotLock);
                    return state;
                }
                finally
                {
                    if (gotLock) spinLock.Exit();
                }
            }
        }

        #endregion

        /// <summary>
        /// 既定の値を使用して、新しい <see cref="Train"/> 型のオブジェクトを作成します。
        /// </summary>
        public Train()
        {
            state = TrainState.Undefined;
            spinLock = new();
        }

        #region INotifyPropertyChanged メソッド
        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
