using UnityEngine;

namespace StateMachineSystem
{
    [CreateAssetMenu(fileName = "HealthData", menuName = "Data/Health", order = 0)]
    public class HealthData : ScriptableObject
    {
        [SerializeField] private int _maxHealth = 100;
        
        public int MaxHealth => _maxHealth;
    }
}