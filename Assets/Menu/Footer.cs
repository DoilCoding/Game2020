using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Menu
{
    public class Footer : MonoBehaviour
    {
        private void Awake()
        {
            _applyButton.onClick.AddListener(OnApply);
            _cancelButton.onClick.AddListener(OnCancel);
            _resetToDefaultsButton.onClick.AddListener(OnResetToDefaults);
        }

        public void OnApply() => Configuration.RequestedPlayerSettings.Apply();
        public void OnCancel() => _menuContainerGameObject.SetActive(false);

        public void OnResetToDefaults()
        {
            // TODO: needs confirmation button which does the following
            Configuration.ResetToDefaults();
        }


#pragma warning disable 649
        [SerializeField] private GameObject _menuContainerGameObject;
        [SerializeField]
        private Button _applyButton,
            _cancelButton,
            _resetToDefaultsButton;
#pragma warning restore 649
    }
}
