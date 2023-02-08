// 日本語対応
using HitSupport;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Move _move = default;
        [SerializeField]
        private OverlapCircle2D _groungChecker = default;

        private Rigidbody2D _rigidbody2D = null;
        private InputManager _inputManager = new InputManager();

        public Move Move => _move;
        public OverlapCircle2D GroungChecker => _groungChecker;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public InputManager InputManager => _inputManager;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _inputManager.Init();
            _move.Init(this);
            _groungChecker.Init(transform);
        }
        private void Update()
        {
            _move.Movement();
            _groungChecker.Update();
        }
        private void OnDrawGizmosSelected()
        {
            _groungChecker.OnDrawGizmos(transform);
        }
    }
}