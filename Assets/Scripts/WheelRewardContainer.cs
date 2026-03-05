using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardWheel
{
    public class WheelRewardContainer : MonoBehaviour
    {
        [SerializeField] private Image _rewardImage;
        [SerializeField] private TextMeshProUGUI _amountText;

        private RewardType _rewardType;
        
        public void Init(RewardType rewardType, Sprite sprite, int amount)
        {
            _rewardType = rewardType;
            _rewardImage.sprite = sprite;
            _amountText.text = amount > 0 ? "x " + amount :  string.Empty;
        }
    }
}