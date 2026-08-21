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
        #region 公開定数
        /// <summary>
        /// 上り方面を示す定数。
        /// </summary>
        public int DirectionUp = 1;
        /// <summary>
        /// 下り方面を示す定数。
        /// </summary>
        public int DirectionDown = -1;
        #endregion
        #region フィールド変数
        /// <summary>
        /// 現在の列車状態。
        /// </summary>
        protected TrainState state;
        /// <summary>
        /// 列車情報。
        /// </summary>
        protected TrainData trainData;
        /// <summary>
        /// 列車種別。
        /// </summary>
        protected ServiceType? serviceType;
        /// <summary>
        /// 現在の閉そく。
        /// </summary>
        protected Track currentTrack;
        /// <summary>
        /// 進行方向。
        /// </summary>
        /// <value>
        /// <b>-1</b> 下り方面<br></br>
        /// <b>0</b> なし<br></br>
        /// <b>1</b> 上り方面
        /// </value>
        protected int direction;
        /// <summary>
        /// 乗客数。
        /// </summary>
        protected uint passengers;
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
        /// <summary>
        /// 列車の編成情報を示す <see cref="TrainData"/> 型のオブジェクトを取得します。
        /// </summary>
        /// <returns>
        /// 列車の編成情報を示す <see cref="TrainData"/> 型のオブジェクト。
        /// </returns>
        /// <remarks>
        /// このメソッドはスレッドセーフではありません。
        /// </remarks>
        public TrainData Data
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return trainData;
            }
        }
        /// <summary>
        /// 進行方向を示す値を取得します。
        /// </summary>
        /// <returns>
        /// 進行方向を示す 32 ビット符号付き整数。
        /// </returns>
        /// <remarks>
        /// このメソッドはスレッドセーフです。
        /// </remarks>
        /// <value>
        /// <b>-1</b> 下り方面<br></br>
        /// <b>0</b> なし<br></br>
        /// <b>1</b> 上り方面
        /// </value>
        public int Direction
        {
            get
            {
                bool gotLock = false;
                try
                {
                    spinLock.Enter(ref gotLock);
                    return direction;
                }
                finally
                {
                    if (gotLock) spinLock.Exit();
                }
            }
        }
        /// <summary>
        /// 乗客数を示す値を取得します。
        /// </summary>
        /// <returns>
        /// 乗客数を示す 32 ビット符号なし整数。
        /// </returns>
        /// <remarks>
        /// このメソッドはスレッドセーフです。
        /// </remarks>
        public uint Passengers
        {
            get
            {
                bool gotLock = false;
                try
                {
                    spinLock.Enter(ref gotLock);
                    return passengers;
                }
                finally
                {
                    if (gotLock) spinLock.Exit();
                }
            }
        }
        /// <summary>
        /// 現在の列車種別を示す <see cref="ServiceType"/> 型のオブジェクトを取得します。
        /// </summary>
        /// <returns>
        /// 列車種別を示す <see cref="ServiceType"/> 型のオブジェクト、または <see langword="null"/> 。
        /// </returns>
        /// <remarks>
        /// このメソッドはスレッドセーフです。
        /// </remarks>
        public ServiceType? CurrentServiceType
        {
            get
            {
                bool gotLock = false;
                try
                {
                    spinLock.Enter(ref gotLock);
                    return serviceType;
                }
                finally
                {
                    if (gotLock) spinLock.Exit();
                }
            }
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 既定の値を使用して、新しい <see cref="Train"/> 型のオブジェクトを作成します。
        /// </summary>
        public Train() : this(null!, null!)
        {

        }
        /// <summary>
        /// 列車情報と現在の閉そくを指定して、新しい <see cref="Train"/> 型のオブジェクトを作成します。
        /// </summary>
        /// <param name="trainData">列車情報を示す <see cref="TrainData"/> 型のオブジェクト。</param>
        /// <param name="currentTrack">現在の閉そくを示す <see cref="Track"/> 型のオブジェクト。</param>
        /// <param name="direction">進行方向を示す値。</param>
        public Train(TrainData trainData, Track currentTrack, int direction = 0)
        {
            this.state = TrainState.Undefined;
            this.trainData = trainData;
            this.currentTrack = currentTrack;
            this.direction = direction;
            spinLock = new();
        }

        #endregion

        /// <summary>
        /// 乗客数を設定します。
        /// </summary>
        /// <remarks>
        /// <see cref="State"/> が <see cref="TrainState.BoardingAndAlighting"/> の時のみ設定できます。
        /// </remarks>
        /// <param name="passengers">乗客数。</param>
        /// <exception cref="InvalidOperationException"/>
        public void SetPassengers(uint passengers)
        {
            bool isChenged = false;
            bool gotLock = false;
            try
            {
                spinLock.Enter(ref gotLock);
                if (this.state != TrainState.BoardingAndAlighting)
                {
                    ThrowInvalidOperation("The value of \"State\" is not \"TrainState.BoardingAndAlighting\".");
                }
                if (passengers != this.passengers)
                {
                    isChenged = true;
                    this.passengers = passengers;
                }
            }
            finally
            {
                if (gotLock) spinLock.Exit();
            }
            if (isChenged)
            {
                NotifyPropertyChanged(nameof(Passengers));
            }
        }
        /// <summary>
        /// 現在の列車種別を即時に変更します。
        /// </summary>
        /// <remarks>
        /// <see cref="State"/> が <see cref="TrainState.BoardingAndAlighting"/> の時のみ設定できます。
        /// </remarks>
        /// <param name="serviceType">新しい列車種別。</param>
        /// <exception cref="InvalidOperationException"/>
        public void SetServiceType(ServiceType? serviceType)
        {
            bool isChenged = false;
            bool gotLock = false;
            try
            {
                spinLock.Enter(ref gotLock);
                if (this.state != TrainState.BoardingAndAlighting)
                {
                    ThrowInvalidOperation("The value of \"State\" is not \"TrainState.BoardingAndAlighting\".");
                }
                if (!EqualityComparer<ServiceType?>.Default.Equals(serviceType, this.serviceType))
                {
                    isChenged = true;
                    this.serviceType = serviceType;
                }
            }
            finally
            {
                if (gotLock) spinLock.Exit();
            }
            if (isChenged)
            {
                NotifyPropertyChanged(nameof(CurrentServiceType));
            }
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowInvalidOperation(string message)
        {
            throw new InvalidOperationException(message);
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
