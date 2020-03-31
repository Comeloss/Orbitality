using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Views.UI
{
    public class PauseButtonView : ReactiveViewBase
    {
#pragma warning disable 0649
        [SerializeField] private Button _playPuaseButton;
        [SerializeField] private TextMeshProUGUI _buttonText;
#pragma warning restore 0649

        private readonly string _playText = "Play";
        private readonly string _pauseText = "Pause";
        
        protected override void OnEnable()
        {
            ObserveEntityWithComponents(
                GameMatcher.GamePlayState,
                entity => entity.gamePlayState,
                additionalModifiers: ObserveFlags.DisposeOnDisable).Subscribe(UpdateButtonText);
            
            _playPuaseButton.onClick.AddListener(SwitchGamePlayState);
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
            Contexts.sharedInstance.input.CreateEntity().isSwitchGamePlayState = true;
        }

        protected override void OnDisable()
        {
            _playPuaseButton.onClick.RemoveAllListeners();
        }
    }
}
