using Assets.Modules.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Modules
{
    public class Footer : MonoBehaviour
    {
        private void Awake()
        {
            _applyButton.onClick.AddListener(OnApply);
            _cancelButton.onClick.AddListener(OnCancel);

            _resetToDefaultsButton.onClick.AddListener(Options.ShowConfirmationResetToDefaults);

            Options.ConfirmationPopupGameObject.transform.Find("Accept_Background").Find("Accept").GetComponent<Button>().onClick.AddListener(() =>
            {
                Configuration.ResetToDefaults();
                Options.HideConfirmationWindow();
            });
            Options.ConfirmationPopupGameObject.transform.Find("Cancel_Background").Find("Cancel").GetComponent<Button>().onClick.AddListener(Options.HideConfirmationWindow);
        }

        public void OnApply() => Configuration.RequestedPlayerSettings.Apply();
        public void OnCancel() => Options.SetActive(false);




#pragma warning disable 649
        [SerializeField]
        private Button _applyButton,
            _cancelButton,
            _resetToDefaultsButton;
#pragma warning restore 649
    }
}
