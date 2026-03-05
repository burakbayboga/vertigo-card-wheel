using System;
using UnityEngine;
using UnityEngine.UI;

public class ReviveButton : MonoBehaviour
{
    public static event Action OnReviveClicked;
    
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
        OnReviveClicked?.Invoke();
    }
}
