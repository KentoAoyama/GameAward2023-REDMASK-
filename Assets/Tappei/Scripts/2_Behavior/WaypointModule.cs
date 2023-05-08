using UnityEngine;

/// <summary>
/// �^�[�Q�b�g�ł͂Ȃ��ړ�����w�肵�Ĉړ�����ꍇ�̈ړ���𐧌䂷��N���X
/// MoveBehavior�N���X����g�p�����
/// </summary>
[System.Serializable]
public class WaypointModule
{
    private GameObject _searchWaypoint;
    private GameObject _forwardWaypoint;
    /// <summary>
    /// ���̍��W����ɂ���Search��Ԃ̈ړ����s��
    /// </summary>
    private Vector3 _footPos;

    /// <summary>
    /// �O��̍��E�̈ړ�������-1�������邱�ƂŔ��]������̂Ń����o�Ƃ��ĕێ����Ă���
    /// </summary>
    private int _prevRandomDir;

    /// <summary>
    /// GameObject�𐶐����邽�߂�Awake�̃^�C�~���O�ŌĂяo���K�v������
    /// �킴�킴�������Ă���̂�Serializable������t���Č����ڂ𓝈ꂳ���邽��
    /// </summary>
    public void InitOnAwake()
    {
        _searchWaypoint = new("SearchWaypoint");
        _forwardWaypoint = new("ForwardWaypoint");
        _prevRandomDir = (int)Mathf.Sign(Random.Range(-100, 100));
    }

    public void UpdateFootPos(Vector3 pos) => _footPos = pos;

    /// <summary>
    /// Search��Ԃ̈ړ�������ۂɌĂ΂��
    /// Transform��Ԃ����Ƃňړ����Ɉړ���𓮂������Ƃ��o����
    /// </summary>
    public Transform GetSearchWaypoint(float distance, bool useRandomDistance)
    {
        if (Random.value <= 0.7f)
        {
            _prevRandomDir *= -1;
        }
        else
        {
            _prevRandomDir = (int)Mathf.Sign(Random.Range(-100, 100));
        }

        float percentage = useRandomDistance ? Random.value : 1;
        Vector3 targetPos = Vector3.right * _prevRandomDir * distance * percentage;
        Vector3 pos = _footPos + targetPos;
        _searchWaypoint.transform.position = pos;

        return _searchWaypoint.transform;
    }

    /// <summary>
    /// �O���Ɉړ�������ۂɌĂ΂��
    /// Transform��Ԃ����Ƃňړ����Ɉړ���𓮂������Ƃ��o����
    /// </summary>
    public Transform GetForwardWaypoint(float destination)
    {
        // FootPos�̍X�V�����Ԋu�Ȃ̂ōX�V�ƍX�V�̊Ԃɍ����Ƃ��납�痎�����
        // �i���̏��FootPos�������Ԃł��̃��\�b�h���Ă΂�Ă��܂���������Ȃ�
        // ���������ꍇ�A�ǂɈ���������ȂǈӐ}���Ȃ�����������\��������

        _footPos.x += destination;
        _forwardWaypoint.transform.position = _footPos;
        
        return _forwardWaypoint.transform;
    }
}
