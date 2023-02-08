// 日本語対応
using System;
using UnityEngine;

namespace HitSupport
{
    [System.Serializable]
    public class OverlapCircle2D
    {
        [SerializeField]
        private Vector2 _offset;
        [SerializeField]
        private float _radius;
        [SerializeField]
        private LayerMask _targetLayer;

        /// <summary> 当たり判定の中央を表す変数 </summary>
        private Transform _origin;

        public Vector2 Offset => _offset;
        public float Radius => _radius;
        public LayerMask TargetLayer => _targetLayer;
        public Transform Origin => _origin;

        /// <summary>
        /// 初期化処理、このクラスを使用するときは、
        /// 最初にこの処理を実行する。
        /// </summary>
        /// <param name="origin"> 原点 </param>
        public void Init(Transform origin)
        {
            _origin = origin;
        }

        /// <summary>
        /// 範囲内にあるコライダーを取得する
        /// </summary>
        /// <returns> 移動方向 :正の値, 負の値 </returns>
        public Collider2D[] GetCollider(float xDir)
        {
            if (xDir >= 0f) xDir = Constant.Right;
            else xDir = Constant.Left;

            var posX = _origin.position.x + _offset.x * xDir;
            var posY = _origin.position.y + _offset.y;

            return Physics2D.OverlapCircleAll(new Vector2(posX, posY), _radius, _targetLayer);
        }

        /// <summary>
        /// 範囲内にコライダーが存在するかどうか判定する。
        /// </summary>
        /// <returns> 移動方向 :正の値, 負の値 </returns>
        public bool IsHit(float xDir)
        {
            return GetCollider(xDir).Length > 0;
        }

        [SerializeField]
        private bool _isDrawGizmo = true;
        [SerializeField]
        private Color _gizmoHitColor = Color.red;
        [SerializeField]
        private Color _gizmoNotHitColor = Color.blue;

        /// <summary>
        /// Gizmoに範囲を描画する
        /// </summary>
        /// <param name="origin"> 当たり判定の中央を表すTransform </param>
        public void OnDrawGizmos(Transform origin, float xDir)
        {
            if (_isDrawGizmo)
            {
                if (xDir >= 0f) xDir = Constant.Right;
                else xDir = Constant.Left;

                Vector2 pos = new Vector2(origin.position.x + _offset.x * xDir, origin.position.y + _offset.y);
                if (Physics2D.OverlapCircleAll(pos, _radius, _targetLayer).Length > 0)
                {
                    Gizmos.color = _gizmoHitColor;
                }
                else
                {
                    Gizmos.color = _gizmoNotHitColor;
                }

                var posX = origin.position.x + _offset.x * xDir;
                var posY = origin.position.y + _offset.y;
                Gizmos.DrawSphere(new Vector2(posX, posY), _radius);
            }
        }
    }
}