using System;
using DefendFlag;
using UnityEngine;

namespace AudioSystem
{
    /// <summary>
    /// Script that subscribes to HitScanGun in order to play sound when reload event occurs
    /// </summary>
    public class ReloadSoundController : MonoBehaviour
    {
        [SerializeField] private HitScanGunClips _gun;
        [SerializeField] private AudioSource _source;
        
        private void Start()
        {
            _gun.OnReload += ReloadHandler;
        }

        private void ReloadHandler(bool began)
        {
            if (!began)
                return;
            if (_source.isPlaying)
                _source.Stop();
            _source.Play();
        }
    }
}