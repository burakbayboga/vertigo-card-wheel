using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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
        
        public void OnCollectibleArrive(int amount)
        {
            _totalAmount += amount;
            _totalAmountText.text = "x " + _totalAmount;
        }
    }
}