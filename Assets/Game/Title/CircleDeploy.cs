using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[ExecuteAlways]
public class CircleDeploy : MonoBehaviour
{
    [SerializeField]
    private float _radius;

    private float _currentAngle = 90;
    private Selectable[] _selectButtons = new Selectable[6];

    public Selectable[] SelectButtons
    {
        get { return _selectButtons; }
    }

    private void Awake()
    {
        Deploy();
    }

    private void OnValidate()
    {
        Deploy();
    }

    private void Deploy()
    {
        var i = 0;
        float angle = 360f / transform.childCount;

        foreach (Transform n in transform)
        {
            float tempAngle = (_currentAngle + angle * i) * Mathf.Deg2Rad;
            Vector3 temp = Vector3.zero;
            temp.x = _radius * Mathf.Cos(tempAngle);
            temp.y = _radius * Mathf.Sin(tempAngle);

            n.localPosition = temp;

            _selectButtons[i] = n.GetComponent<Button>();
            i++;
        }
    }

    public void RotateChild(float angle)
    {
        _currentAngle += angle;

        var i = 0;
        float angle2 = 360f / transform.childCount;

        foreach (Transform n in transform)
        {
            float tempAngle = (_currentAngle + angle2 * i) * Mathf.Deg2Rad;
            Vector3 temp = Vector3.zero;
            temp.x = _radius * Mathf.Cos(tempAngle);
            temp.y = _radius * Mathf.Sin(tempAngle);

            n.DOLocalMove(temp, 0.5f);
            i++;
        }
    }
}
