using UniRx;
using UniRx.Triggers;
using UnityEngine;

/// <summary>
/// �������p
/// �e�U�镑���̃N���X�̃��\�b�h��g�ݍ��킹�čs���𐧌䂷��N���X
/// </summary>
public class ShieldEnemyController : EnemyController
{
    [Header("���̃R���C�_�[���t�����I�u�W�F�N�g")]
    [SerializeField] GameObject _shield;

    protected override void Awake()
    {
        InitSubscribeShield();
        base.Awake();
    }

    private void InitSubscribeShield()
    {
        _shield.OnDisableAsObservable().Subscribe(_ => Debug.Log("�q�b�g"));
    }
}
