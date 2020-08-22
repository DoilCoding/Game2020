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

            _resetToDefaultsButton.onClick.AddListener(ShowConfirmationResetToDefaults);

            _confirmationContainerGameObject.transform.Find("Accept_Background").Find("Accept").GetComponent<Button>().onClick.AddListener(() =>
            {
                Configuration.ResetToDefaults();
                HideConfirmationResetToDefaults();
            });
            _confirmationContainerGameObject.transform.Find("Cancel_Background").Find("Cancel").GetComponent<Button>().onClick.AddListener(HideConfirmationResetToDefaults);
        }

        public void OnApply() => Configuration.RequestedPlayerSettings.Apply();
        public void OnCancel() => _menuContainerGameObject.SetActive(false);

        public void ShowConfirmationResetToDefaults() => _confirmationContainerGameObject.SetActive(true);
        public void HideConfirmationResetToDefaults() => _confirmationContainerGameObject.SetActive(false);


#pragma warning disable 649
        [SerializeField] private GameObject _menuContainerGameObject, _confirmationContainerGameObject;
        [SerializeField]
        private Button _applyButton,
            _cancelButton,
            _resetToDefaultsButton;
#pragma warning restore 649
    }
}
