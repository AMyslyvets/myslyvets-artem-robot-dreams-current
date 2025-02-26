
using UnityEngine;

public class Operators1 : MonoBehaviour
{
    [SerializeField] private Transform _1first;
    [SerializeField] private Transform _1second;
    [SerializeField] private float _range;
    [SerializeField] private ColorSwathA1 _colorSwatch55;

    [ContextMenu("Is in range")]
    public void InRange()
    {
        Vector3 difference = _1first.position - this._1second.position;
        float distance = difference.magnitude;
        Debug.Log(distance <= _range ? "Is in range" : "Is not in range");
        /*if (distance < _range)
        {
            Debug.Log("Is in range");
        }
        else
        {
            Debug.Log("Is not in range");
        }*/
    }
}
