using System;
using UnityEngine;
using UnityEngine.UI;

namespace CardWheel
{
    public class GiveUpButton : MonoBehaviour
    {
        public static event Action OnGiveUpClicked;
        
        [SerializeField] private Button _button;

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
            OnGiveUpClicked?.Invoke();
        }
    }
}