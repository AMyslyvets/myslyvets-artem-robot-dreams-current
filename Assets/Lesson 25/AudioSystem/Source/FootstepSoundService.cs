using System;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;

namespace AudioSystem
{
    /// <summary>
    /// Implementation of IFootstepSoundService
    /// Straightforward wrapper of FootSoundLibrary
    /// </summary>
    public class FootstepSoundService : MonoServiceBase, IFootstepSoundService
    {
        [SerializeField] private Terrain _terrain;
        [SerializeField] private FootSoundLibrary _footSoundLibrary;

        public override Type Type { get; } = typeof(IFootstepSoundService);

        protected override void Awake()
        {
            base.Awake();
            _footSoundLibrary.Init(_terrain);
        }

        public AudioClip GetFootstepSound(PhysicMaterial physicMaterial, Vector3 worldPosition)
        {
            return _footSoundLibrary.GetFootstepSound(physicMaterial, worldPosition);
        }
    }
}