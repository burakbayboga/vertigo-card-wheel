using UnityEngine;
using UnityEngine.UI;

namespace CardWheel
{
    public class SpinButton : MonoBehaviour
    { 
        private Button _button;

        private void OnValidate()
        {
            _button = GetComponent<Button>();
        }

        private void Awake()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            WheelController.TriggerSpinButtonClicked();
        }
    }
}