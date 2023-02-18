using UnityEngine;

namespace Bullet
{
    /// <summary>
    /// 薬莢を表すクラス
    /// </summary>
    public class ShellCase : MonoBehaviour, IStoreableInChamber
    {
        public BulletType Type => BulletType.ShellCase;

        /// <summary>
        /// 排莢処理
        /// </summary>
        public void ExcretedPods()
        {

        }
    }
}