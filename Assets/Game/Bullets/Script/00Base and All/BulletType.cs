namespace Bullet
{
    /// <summary>
    /// 弾の種類を表す列挙体
    /// </summary>
    [System.Serializable]
    public enum BulletType
    {
        /// <summary> エラー値。未設定を表す </summary>
        NotSet = -1,
        /// <summary> 標準的な弾 </summary>
        StandardBullet,
        /// <summary> 貫通する弾 </summary>
        PenetrateBullet,
        /// <summary> 反射する弾 </summary>
        ReflectBullet,


        /// <summary> 殻薬莢を表現する値 </summary>
        ShellCase,
        /// <summary> 空を表現する値 </summary>
        Empty,
        /// <summary> 終了値。この型の最大値を表す。 </summary>
        End,
    }
}