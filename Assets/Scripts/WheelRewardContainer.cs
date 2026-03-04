using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardWheel
{
    public class WheelRewardContainer : MonoBehaviour
    {
        [SerializeField] private List<Color> _wheelTypeColors;
        [SerializeField] private Image _rewardImage;
        [SerializeField] private TextMeshProUGUI _amountText;
        [SerializeField] private Image _bgImage;

        private RewardType _rewardType;
        
        public void Init(RewardType rewardType, Sprite sprite, int amount, WheelType wheelType)
        {
            _rewardType = rewardType;
            _rewardImage.sprite = sprite;
            _amountText.text = amount > 0 ? "x " + amount :  string.Empty;
            _bgImage.color = _wheelTypeColors[(int)wheelType];
        }
    }
}