// 日本語対応
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

        private Rigidbody2D _rigidbody2D = null;
        private InputManager _inputManager = new InputManager();

        public Move Move => _move;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public InputManager InputManager => _inputManager;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _inputManager.Init();
            _move.Init(this);
        }
        private void Update()
        {
            _move.Movement();
        }
    }
}