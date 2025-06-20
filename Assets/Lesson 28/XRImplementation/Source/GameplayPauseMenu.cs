using StateMachineSystem.GameStateSystem;
using StateMachineSystem.ServiceLocatorSystem;
using UnityEngine;
using UnityEngine.UI;


namespace XRImplementation
{
    public class GameplayPauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _canvas;
        
        [SerializeField] private UIPokeInteractable _confrimButton;
        [SerializeField] private UIPokeInteractable _cancelButton;

        public bool Enabled
        {
            get => _canvas.activeSelf;
            set
            {
                if (_canvas.activeSelf == value)
                    return;
                _canvas.SetActive(value);
                ServiceLocator.Instance.GetService<IGameStateProvider>()?.SetGameState(value ? GameState.Paused : GameState.Gameplay);
            }
        }
        
        private void Awake()
        {
            _confrimButton.onClick += ConfirmButtonHandler;
            _cancelButton.onClick += CancelButtonHandler;
        }

        private void Start()
        {
            ServiceLocator.Instance.GetService<StateMachineSystem.InputController>().OnEscape += EscapeHandler;
            Enabled = false;
        }

        private void EscapeHandler()
        {
            Enabled = !Enabled;
        }

        private void ConfirmButtonHandler()
        {
            ServiceLocator.Instance.GetService<IGameStateProvider>().SetGameState(GameState.MainMenu);
        }

        private void CancelButtonHandler()
        {
            Enabled = false;
        }
    }
}