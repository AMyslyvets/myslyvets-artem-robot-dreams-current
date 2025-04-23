
using UnityEngine;
using Random = UnityEngine.Random;

namespace Inventory.VisualEffects
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        [SerializeField] private float _speed;

        private Quaternion _rotation;

        private void Awake()
        {
            Quaternion rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            _rotation = _target.rotation;
            _rotation = rotation * _rotation;
            _target.rotation = _rotation;
        }

        private void Update()
        {
            Quaternion rotation = Quaternion.Euler(0, _speed * Time.deltaTime, 0);
            _rotation = rotation * _rotation;
            _target.rotation = _rotation;
        }
    }
}