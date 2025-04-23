using System;
using StateMachineSystem.GameStateSystem;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;
using UnityEngine.UI;

namespace AudioSystem
{
    /// <summary>
    /// Script that controls Sound Menu
    /// Three sliders are initialized with volume from SoundService
    /// On event of changing value, each slider's handler sets new value as corresponding volume in SoundService
    /// </summary>
    public class SoundMenu : MonoBehaviour
    {
        [Serializable]
        public struct SoundSlider
        {
            public SoundType type;
            public Slider slider;
        }
        
        [SerializeField] private Canvas _canvas;
        [SerializeField] private SoundSlider[] _sliders;

        private StateMachineSystem.InputController _inputController;
        private ISoundService _soundService;
        
        public bool Enabled
        {
            get => _canvas.enabled;
            set
            {
                if (_canvas.enabled == value)
                    return;
                _canvas.enabled = value;
            }
        }

        private void Start()
        {
            _soundService = ServiceLocator.Instance.GetService<ISoundService>();

            for (int i = 0; i < _sliders.Length; ++i)
            {
                SoundType soundType = _sliders[i].type;
                Slider slider = _sliders[i].slider;
                slider.SetValueWithoutNotify(_soundService.GetVolume(soundType));
                slider.onValueChanged.AddListener((value) => SliderHandler(soundType, value));
            }

            Enabled = false;
        }

        private void SliderHandler(SoundType soundType, float value)
        {
            _soundService.SetVolume(soundType, value);
        }
    }
}