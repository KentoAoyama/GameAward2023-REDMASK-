using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HitSupport
{
    [System.Serializable]
    public class OverlapBox2D
    {
        [SerializeField]
        private Vector2 _offset;
        [SerializeField]
        private Vector2 _size;
        [SerializeField]
        private LayerMask _targetLayer;

        private Transform _origin;
        private Vector2 _previousPos;
        private float _dir = 1f;

        public Vector2 Offset => _offset;
        public Vector2 Size => _size;
        public LayerMask TargetLayer => _targetLayer;
        public Transform Origin => _origin;

        public void Init(Transform origin)
        {
            _origin = origin;
        }

        public void Update()
        {
            float diff = _previousPos.x - _origin.position.x;
            if (Mathf.Abs(diff) > 0.01f)
            {
                _dir = diff < 0f ? 1f : -1f;
            }
            _previousPos = _origin.position;
        }

        public Collider2D[] GetCollider()
        {
            var posX = _origin.position.x + _offset.x * _dir;
            var posY = _origin.position.y + _offset.y;

            return Physics2D.OverlapBoxAll(new Vector2(posX, posY), _size, 0.0f, _targetLayer);
        }

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
        public void OnDrawGizmos(Transform origin)
        {
            if (_isDrawGizmo)
            {
                Vector2 pos = new Vector2(origin.position.x + _offset.x * _dir, origin.position.y + _offset.y);
                if (Physics2D.OverlapBoxAll(pos, _size, 0.0f, _targetLayer).Length > 0)
                {
                    Gizmos.color = _gizmoHitColor;
                }
                else
                {
                    Gizmos.color = _gizmoNotHitColor;
                }

                var posX = origin.position.x + _offset.x * _dir;
                var posY = origin.position.y + _offset.y;
                Gizmos.DrawCube(new Vector2(posX, posY), _size);
            }
        }
    }
}
