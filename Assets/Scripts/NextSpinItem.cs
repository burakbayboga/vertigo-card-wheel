using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardWheel
{
    public class NextSpinItem : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _rariyText;
        [SerializeField] private TextMeshProUGUI _spinNoText;
        [SerializeField] private List<Sprite> _wheelSprites;
        [SerializeField] private List<Color> _rarityColors;
        
        public void SetSpin(WheelType wheelType, int spinNo)
        {
            _iconImage.sprite = _wheelSprites[(int)wheelType];
            _rariyText.text = wheelType.ToString();
            _rariyText.color = _rarityColors[(int)wheelType];
            _spinNoText.text = spinNo.ToString();
        }
    }
}