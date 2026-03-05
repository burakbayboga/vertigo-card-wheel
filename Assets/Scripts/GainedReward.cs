using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardWheel
{
    public class GainedReward : MonoBehaviour
    {
        [HideInInspector] public RewardType RewardType;

        [SerializeField] private GameObject _collectibleHitEffect;
        [SerializeField] private Image _rewardImage;
        [SerializeField] private TextMeshProUGUI _totalAmountText;
        [SerializeField] private Animator _animator;

        private int _totalAmount;
        private readonly int _collectibleHitTrigger = Animator.StringToHash("CollectibleHitTrigger");

        private void OnValidate()
        {
            _animator = GetComponent<Animator>();
        }
        
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

        public Transform GetTargetTransformForCollectible()
        {
            return _rewardImage.transform;
        }
        
        public void OnCollectibleArrive(int amount)
        {
            _totalAmount += amount;
            UpdateText();
            _animator.SetTrigger(_collectibleHitTrigger);
            Destroy(Instantiate(_collectibleHitEffect, transform), 1.5f);
        }

        private void UpdateText()
        {
            _totalAmountText.text = "x " + _totalAmount;
        }
    }
}