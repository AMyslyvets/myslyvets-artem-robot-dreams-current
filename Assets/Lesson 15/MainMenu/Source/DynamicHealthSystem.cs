using System.Collections.Generic;
using Dummies;
using UnityEngine;

namespace MainMenu
{
    public class DynamicHealthSystem : HealthSystem
    {
        private Dictionary<Collider, IHealth> _characters = new();

        public IHealth this[Collider collider]
        {
            get
            {
                _ = _characters.TryGetValue(collider, out IHealth health);
                return health;
            }
        }
        
        public void AddCharacter(IHealth character)
        {
            _characters.Add(character.Collider, character);
            // Important, Liskov principle broken here
            character.OnDeath += () => CharacterDeathHandler((Dummies.Health)character);
        }

        public void RemoveCharacter(IHealth character)
        {
            _characters.Remove(character.Collider);
        }

        public override bool GetHealth(Collider characterController, out Dummies.Health health)
        {
            if (base.GetHealth(characterController, out health))
                return true;
            IHealth dynamicHealth = this[characterController];
            bool exists = dynamicHealth != null;
            if (exists) // Second break of Liskov principle
                health = (Dummies.Health)dynamicHealth;
            return exists;
        }
    }
}