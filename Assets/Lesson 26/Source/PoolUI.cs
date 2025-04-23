using System;
using TMPro;
using UnityEngine;

namespace Lesson26
{
    public class PoolUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _poolCount;
        [SerializeField] private TextMeshProUGUI _activeCount;
        [SerializeField] private Transform _poolContainer;
        [SerializeField] private Transform _activeContainer;
        
        
        private void Update()
        {
            _poolCount.SetText(_poolContainer.childCount.ToString());
            _activeCount.SetText(_activeContainer.childCount.ToString());
        }
    }
}