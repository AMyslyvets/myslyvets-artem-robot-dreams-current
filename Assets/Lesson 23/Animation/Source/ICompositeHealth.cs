using System;

namespace Animation
{
    public interface ICompositeHealth
    {
        event Action OnDeath;
        event Action<int> OnTakeDamage;
        event Action<int> OnHealthChanged;
        event Action<float> OnHealthChanged01;

        int HealthValue { get; }
        bool IsAlive { get; }

        float HealthValue01 { get; }
        int MaxHealthValue { get; }

        void TakeDamage(int damage);

        void Heal(int heal);

        void SetHealth(int health);
    }
}