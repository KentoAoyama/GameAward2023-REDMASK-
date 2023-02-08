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
        /// <summary> 前の位置 </summary>
        private Vector2 _previousPos;
        /// <summary> 方向を表す変数 </summary>
        private float _xDir = 1f;

        public Vector2 Offset => _offset;
        public float Radius => _radius;
        public LayerMask TargetLayer => _targetLayer;
        public Transform Origin => _origin;

        /// <summary>
        /// 初期化処理、このクラスを使用するときは、
        /// 最初にこの処理を実行する。
        /// </summary>
        /// <param name="origin"></param>
        public void Init(Transform origin)
        {
            _origin = origin;
        }

        /// <summary>
        /// 更新処理 <br/>
        /// 進行方向に合わせて左右の方向を更新する
        /// </summary>
        public void Update()
        {
            float diff = _previousPos.x - _origin.position.x;
            if (Mathf.Abs(diff) > 0.01f)
            {
                _xDir = diff < 0f ? 1f : -1f;
            }
            _previousPos = _origin.position;
        }

        /// <summary>
        /// 範囲内にあるコライダーを取得する
        /// </summary>
        /// <returns></returns>
        public Collider2D[] GetCollider()
        {
            var posX = _origin.position.x + _offset.x * _xDir;
            var posY = _origin.position.y + _offset.y;

            return Physics2D.OverlapCircleAll(new Vector2(posX, posY), _radius, _targetLayer);
        }

        /// <summary>
        /// 範囲内にコライダーが存在するかどうか判定する。
        /// </summary>
        /// <returns></returns>
        public bool IsHit()
        {
            return GetCollider().Length > 0;
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
        public void OnDrawGizmos(Transform origin)
        {
            if (_isDrawGizmo)
            {
                Vector2 pos = new Vector2(origin.position.x + _offset.x * _xDir, origin.position.y + _offset.y);
                if (Physics2D.OverlapCircleAll(pos, _radius, _targetLayer).Length > 0)
                {
                    Gizmos.color = _gizmoHitColor;
                }
                else
                {
                    Gizmos.color = _gizmoNotHitColor;
                }

                var posX = origin.position.x + _offset.x * _xDir;
                var posY = origin.position.y + _offset.y;
                Gizmos.DrawSphere(new Vector2(posX, posY), _radius);
            }
        }
    }
}