using UnityEngine;
using UnityEngine.UI;

namespace Assets.Testing
{
    public class UI_august_Options_Header : MonoBehaviour
    {
        private void Awake()
        {
            AddSliderBehaviour();
            AddButtonBehaviour();
        }

        private void AddSliderBehaviour()
        {
            var _ = _sliderGameObject.GetComponent<Slider>();
            _activeWindow = _generalGameObject;
            _.value = 0;
            _.onValueChanged.AddListener(OnIndexChange);
        }
        private void AddButtonBehaviour()
        {
            _generalButton.onClick.AddListener(() => OnIndexChange(0));
            _graphicsButton.onClick.AddListener(() => OnIndexChange(1));
            _audioButton.onClick.AddListener(() => OnIndexChange(2));
            _inputButton.onClick.AddListener(() => OnIndexChange(3));
            _keybindingButton.onClick.AddListener(() => OnIndexChange(4));
        }

        private void OnIndexChange(float _)
        {
            switch (_)
            {
                case 0:
                    _activeWindow.SetActive(false);
                    _activeWindow = _generalGameObject;
                    _activeWindow.SetActive(true);
                    break;
                case 1:
                    _activeWindow.SetActive(false);
                    _activeWindow = _graphicsGameObject;
                    _activeWindow.SetActive(true);
                    break;
                case 2:
                    _activeWindow.SetActive(false);
                    _activeWindow = _audioGameObject;
                    _activeWindow.SetActive(true);
                    break;
                case 3:
                    _activeWindow.SetActive(false);
                    _activeWindow = _inputGameObject;
                    _activeWindow.SetActive(true);
                    break;
                case 4:
                    _activeWindow.SetActive(false);
                    _activeWindow = _keybindingsGameObject;
                    _activeWindow.SetActive(true);
                    break;
            }

            _sliderGameObject.GetComponent<Slider>().value = _;
        }

#pragma warning disable 649
        [SerializeField]
        private GameObject _generalGameObject,
            _graphicsGameObject,
            _audioGameObject,
            _inputGameObject,
            _keybindingsGameObject,
            _sliderGameObject;

        [SerializeField] private Button _generalButton,
            _graphicsButton,
            _audioButton,
            _inputButton,
            _keybindingButton;
#pragma warning restore 649

        private GameObject _activeWindow;
    }
}
