// 日本語対応
using System;
using UnityEngine;

namespace Bullet
{
    /// <summary>
    /// チェンバーに格納可能な事を表現するインターフェース
    /// </summary>
    public interface IStoreableInChamber
    {
        /// <summary>  </summary>
        public BulletType Type { get; }
    }
}