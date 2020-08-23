using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Assets.Modules
{
    public class Options : MonoBehaviour
    {
        private void Awake()
        {
            _self = this.gameObject;
            KeybindingPopupGameObject = _keybindingPopupGameObject;
            ConfirmationPopupGameObject = _confirmationPopupGameObject;
            _self.SetActive(false);
        }

        public static bool Active => _self != null && _self.activeSelf;

        public static void SetActive(bool active) => _self.SetActive(active);

        // reset to default popup
        public static void ShowConfirmationResetToDefaults() => ConfirmationPopupGameObject.SetActive(true);
        public static void HideConfirmationWindow() => ConfirmationPopupGameObject.SetActive(false);
        public static bool IsConfirmationPopupOpen => ConfirmationPopupGameObject.activeSelf;


        // key binding popup
        public static void ShowKeybindingPopup() => KeybindingPopupGameObject.SetActive(true);
        public static void HideKeybindingWindow() => KeybindingPopupGameObject.SetActive(false);
        public static bool IsKeybindingPopupOpen => KeybindingPopupGameObject.activeSelf;

        private static GameObject _self;
        public static GameObject KeybindingPopupGameObject, ConfirmationPopupGameObject;

#pragma warning disable 649
        [SerializeField]
        private GameObject
            _keybindingPopupGameObject,
            _confirmationPopupGameObject;
#pragma warning restore 649
    }
}
