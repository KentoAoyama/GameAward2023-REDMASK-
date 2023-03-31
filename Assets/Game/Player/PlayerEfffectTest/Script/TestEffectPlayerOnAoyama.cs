using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

namespace Player
{
    public class TestEffectPlayerOnAoyama : MonoBehaviour
    {
        public InputManager InputManager { get; private set; } = new InputManager();

        [SerializeField] private Animator _mazle;
        [SerializeField] private ParticleSystem _blooad;

        private PlayerController _playerController;

        [SerializeField] CinemachineVirtualCamera _camera;
        private CinemachineImpulseSource _source;

        [SerializeField] private float _lrTime = 0.5f;
        [SerializeField] Transform[] _lrPos = new Transform[2];
        [SerializeField] private LineRenderer _lr;

        void Start()
        {
            InputManager.Init();
            _source = _camera.GetComponent<CinemachineImpulseSource>();
        }

        void Update()
        {
            if (Keyboard.current.spaceKey.IsPressed())
            {
                float a = Random.Range(-0.4f,0.4f);
                float b = Random.Range(0, 0.1f);
                float c = Random.Range(0, 0.3f);

                _source.m_DefaultVelocity = new Vector3(a, b, c);

                StartCoroutine(LRSet());
                _source.GenerateImpulse();


                _blooad.Play();
                _mazle.Play("Muzzle Flash Play");

            }
        }


        public IEnumerator LRSet()
        {
            _lr.positionCount = 2;
            _lr.SetPosition(0, _lrPos[0].position);
            _lr.SetPosition(1, _lrPos[1].position);

            yield return new WaitForSeconds(_lrTime);
            _lr.positionCount = 0;
        }

    }
}