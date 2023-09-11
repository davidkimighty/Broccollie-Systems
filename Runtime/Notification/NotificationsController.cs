using System;
using Broccollie.UI;
using TMPro;
using UnityEngine;

namespace Broccollie.System
{
    public class NotificationsController : MonoBehaviour
    {
        [SerializeField] private NotificationsEventChannel _eventChannel;

        [Header("Full Screen")]
        [SerializeField] private PanelUI _fullScreenPanel;
        [SerializeField] private TMP_Text _fullScreenMessageText;

        [Header("Popup Confirm")]
        [SerializeField] private PanelUI _popupConfirmPanel;
        [SerializeField] private TMP_Text _popupConfirmMessageText;
        [SerializeField] private ButtonUI _popupConfirmButton;

        private void Awake()
        {
            _popupConfirmButton.OnSelect += (eventArgs) => PopupConfirmedAsync();
        }

        private void OnEnable()
        {
            _eventChannel.OnFullscreenMessage += ShowFullscreenMessageAsync;
            _eventChannel.OnPopupConfirmMessage += ShowPopupConfirmMessageAsync;
        }

        private void OnDisable()
        {
            _eventChannel.OnFullscreenMessage -= ShowFullscreenMessageAsync;
            _eventChannel.OnPopupConfirmMessage -= ShowPopupConfirmMessageAsync;
        }

        #region Subscribers
        private async void ShowFullscreenMessageAsync(bool state, string message)
        {
            if (_fullScreenMessageText != null)
                _fullScreenMessageText.text = message;
            await _fullScreenPanel.SetActiveAsync(state);
        }

        private async void ShowPopupConfirmMessageAsync(bool state, string message)
        {
            if (_popupConfirmMessageText != null)
                _popupConfirmMessageText.text = message;
            await _popupConfirmPanel.SetActiveAsync(state);

        }

        private async void PopupConfirmedAsync()
        {
            if (_popupConfirmMessageText != null)
                _popupConfirmMessageText.text = string.Empty;
            await _popupConfirmPanel.SetActiveAsync(false);
        }

        #endregion
    }

    public class FullScreenMessage : IDisposable
    {
        private NotificationsEventChannel _eventChannel;

        public FullScreenMessage(NotificationsEventChannel eventChannel, string message)
        {
            _eventChannel = eventChannel;
            _eventChannel.RequestFullscreenMessage(true, message);
        }

        public void Dispose()
        {
            if (_eventChannel == null) return;
            _eventChannel.RequestFullscreenMessage(false);
        }
    }
}
