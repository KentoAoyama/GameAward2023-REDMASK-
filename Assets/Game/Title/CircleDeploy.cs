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
        int i = 0;
        float angle2 = 360f / transform.childCount;

        foreach (Transform n in transform)
        {
            int num = i;
            var start = _currentAngle;

            DOTween.To(
                () => start,
                x => {
                    float tempAngle = (x + angle2 * num) * Mathf.Deg2Rad;
                    Vector3 temp = new Vector3();
                    temp.x = _radius * Mathf.Cos(tempAngle);
                    temp.y = _radius * Mathf.Sin(tempAngle);

                    n.localPosition = temp;
                },
                angle + _currentAngle,
                0.5f);
            i++;
        }

        _currentAngle += angle;
    }
}
