// 日本語対応
using UnityEngine;

namespace Bullet
{
    /// <summary>
    /// チェンバーに詰めれるモノを表現するクラス
    /// </summary>
    [System.Serializable]
    public class BulletBase : MonoBehaviour
    {
        public virtual BulletType Type => BulletType.NotSet;
    }
}