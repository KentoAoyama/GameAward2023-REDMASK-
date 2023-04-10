using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class StairsCheck
    {
        [Header("���C���[�̊J�n�n�_")]
        [SerializeField] private Vector2 _rayOffSet;

        [Header("Ray�̒���")]
        [SerializeField] private float _rayLong = 0.3f;

        [Header("�v���C���[�̃��C���[")]
        [SerializeField] private LayerMask _playerLayer;

        [Header("�K�i�̃��C���[")]
        [SerializeField] private LayerMask _stairsLayer;

        [SerializeField] LayerMask _ground;

        [SerializeField] float _rayLongDown = 1.2f;

        Vector2 dir;

        private float _timeCount;
        private float _time = 0.2f;

        private bool _nowJumping;

        private bool _isHitStairs = false;


        private bool _isCanStaits = false;

        private PlayerController _playerController;
        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
        }


        public bool CheckMoveDir(float inputH)
        {
            if (_nowJumping)
            {
                _timeCount += Time.deltaTime;

                if (_timeCount > _time)
                {
                    _timeCount = 0;
                    _nowJumping = false;
                }

                _isHitStairs = false;
                return false;
            }
            else
            {
                RaycastHit2D hit;

                hit = Physics2D.Raycast(_playerController.Player.transform.position, -_playerController.Player.transform.up, _rayLongDown, _stairsLayer);

                Vector2 moveDir = Vector2.right * inputH;

                float angle = Vector3.Angle(Vector2.up, hit.normal);

                dir = Quaternion.AngleAxis(angle, Vector3.forward) * moveDir;

                Debug.DrawRay(_playerController.Player.transform.position, -_playerController.Player.transform.up * _rayLongDown, Color.blue);

                if (hit.collider == null)
                {
                    _isHitStairs = false;
                    return false;
                }
                else
                {
                    _isHitStairs = true;
                    Debug.DrawRay(_playerController.Player.transform.position, dir * _rayLongDown, Color.blue);
                    return true;
                }
            }
        }


        //public Vector3 GetDirStairs(float h)
        //{
        //    RaycastHit2D hit;

        //    hit = Physics2D.Raycast(_playerController.Player.transform.position, Vector2.down, _rayLongDown, _stairsLayer);

        //    Vector2 moveDir = Vector2.right * h;

        //    float angle = Vector3.Angle(Vector2.up, hit.normal);

        //    dir = Quaternion.AngleAxis(angle, Vector3.forward) * moveDir;

        //    Debug.DrawRay(_playerController.Player.transform.position, -_playerController.Player.transform.up * _rayLongDown, Color.blue);

        //    if (hit.collider == null)
        //    {
        //        _isHitStairs = false;
        //        return false;
        //    }
        //    else
        //    {
        //        _isHitStairs = true;
        //        Debug.DrawRay(_playerController.Player.transform.position, dir * _rayLongDown, Color.blue);
        //        return true;
        //    }
        //}

        public bool CheckStairs()
        {

            float v = _playerController.InputManager.GetValue<float>(InputType.MoveHorizontal) > 0f ? 1f : -1f;

            Vector2 orizin = (Vector2)_playerController.Player.transform.position + _rayOffSet;

            bool rightHit = Physics2D.Raycast(orizin, Vector2.right, _rayLong, _stairsLayer);
            bool leftHit = Physics2D.Raycast(orizin, Vector2.left, _rayLong, _stairsLayer);

            


            //�K�i�̏�ɂ���B�܂�A�K�i��o���Ă�����
            if (_isHitStairs)
            {
                //�n�ʂɂ��Ă���ꍇ
                if(_playerController.GroungChecker.IsHit(_playerController.DirectionControler.MovementDirectionX))
                {

                }


                //�������̓��͂���������K�i�Ƃ̔���𖳂����A��������B
                if (v < 0)
                {
                    ChangeLayer(false);
                    _isCanStaits = false;
                }
            }

            return false;
        }


        public void ChangeLayer(bool a)
        {
            Physics2D.IgnoreLayerCollision(_playerLayer, _stairsLayer, a);
        }

        public void Check()
        {


            if (_playerController.InputManager.IsExist[InputType.InputVertical])
            {

            }




        }



    }
}