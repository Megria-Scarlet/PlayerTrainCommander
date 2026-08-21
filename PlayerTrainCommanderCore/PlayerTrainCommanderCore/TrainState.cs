namespace PTC.Core
{
    /// <summary>
    /// 列車の状態を表す定数を定義します。
    /// </summary>
    public enum TrainState
    {
        /// <summary>
        /// 未定義の値。
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// 留置中の値。
        /// </summary>
        Detention = 1,
        /// <summary>
        /// 待機中の値。
        /// </summary>
        Waiting = 2,
        /// <summary>
        /// 乗降中の値。
        /// </summary>
        BoardingAndAlighting = 3,
        /// <summary>
        /// 停車中の値。
        /// </summary>
        Stop = 4,

        /// <summary>
        /// 加速中の値。
        /// </summary>
        Acceleration = 128,
        /// <summary>
        /// 惰行中の値。
        /// </summary>
        Coasting = 129,
        /// <summary>
        /// 減速中の値。
        /// </summary>
        Deceleration = 130
    }
}
