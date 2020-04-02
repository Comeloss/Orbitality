using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Views.UI
{
    public class ButtonsView : ReactiveViewBase
    {
#pragma warning disable 0649
        [SerializeField] private Button _playPuaseButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private TextMeshProUGUI _buttonText;
#pragma warning restore 0649

        private readonly string _playText = "Play";
        private readonly string _pauseText = "Pause";
        private readonly string _orbitalitySaveText = "OrbitalitySave";
        
        protected override void OnEnable()
        {
            ObserveEntityWithComponents(
                GameMatcher.GamePlayState,
                entity => entity.gamePlayState,
                additionalModifiers: ObserveFlags.DisposeOnDisable).Subscribe(UpdateButtonText);
            
            _playPuaseButton.onClick.AddListener(SwitchGamePlayState);
            _loadButton.onClick.AddListener(LoadGame);
            _saveButton.onClick.AddListener(SaveGame);
        }

        private void LoadGame()
        {
            C.input.CreateEntity().ReplaceLoad(_orbitalitySaveText);
        }

        private void SaveGame()
        {
            C.input.CreateEntity().ReplaceNewSave(_orbitalitySaveText);
        }

        private void UpdateButtonText(GamePlayStateComponent component)
        {
            switch (component.CurrentState)
            {
                case GamePlayStateComponent.GamePlayStateType.Play:
                    SetButtonText(_playText);
                    break;
                case GamePlayStateComponent.GamePlayStateType.Pause:
                    SetButtonText(_pauseText);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetButtonText(string text)
        {
            if (!_buttonText)
            {
                Debug.LogError("[PauseButtonView] Button reference is null");
                return;
            }
            
            _buttonText.SetText(text);
        }

        private void SwitchGamePlayState()
        {
            C.input.CreateEntity().isSwitchGamePlayState = true;
        }

        protected override void OnDisable()
        {
            _playPuaseButton.onClick.RemoveAllListeners();
            _loadButton.onClick.RemoveAllListeners();
            _saveButton.onClick.RemoveAllListeners();
        }
    }
}
