using System;
using UnityEngine;

namespace Lesson26
{
    public class SimplexEnemy : MonoBehaviour
    {
        public Action<SimplexEnemy> onArrived;
        
        private Vector3 _target;
        private float _speed;

        public void SetTarget(Vector3 target)
        {
            _target = target;
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }
        
        private void Update()
        {
            Vector3 position = transform.position;
            float interval = _speed * Time.deltaTime;
            if (interval >= (_target - position).magnitude)
            {
                position = _target;
                onArrived?.Invoke(this);
            }
            else
            {
                position = Vector3.MoveTowards(position, _target, interval);
            }
            transform.position = position;
        }
    }
}