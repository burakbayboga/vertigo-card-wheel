using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardWheel
{
    public class GainedReward : MonoBehaviour
    {
        [HideInInspector] public RewardType RewardType;
        
        [SerializeField] private Image _rewardImage;
        [SerializeField] private TextMeshProUGUI _totalAmountText;

        private int _totalAmount;

        public void Init(RewardType rewardType, Sprite rewardSprite)
        {
            _rewardImage.sprite = rewardSprite;
            RewardType = rewardType;
        }

        public void SetAmount(int amount)
        {
            _totalAmount = amount;
            UpdateText();
        }
        
        public void OnCollectibleArrive(int amount)
        {
            _totalAmount += amount;
            UpdateText();
        }

        private void UpdateText()
        {
            _totalAmountText.text = "x " + _totalAmount;
        }
    }
}