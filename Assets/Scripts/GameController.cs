using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace CardWheel
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private WheelController _wheelController;
        [SerializeField] private WheelRewardData _rewardData;
        [SerializeField] private WheelRewardSelection _rewardSelection;
        [SerializeField] private Transform _rewardCollectibleSpawnRef;
        [SerializeField] private GameObject _rewardCollectiblePrefab;
        [SerializeField] private GainedRewardsController _gainedRewardsController;

        [SerializeField] private int _maxCollectibleAmount;
        [SerializeField] private float _collectibleSpawnDelay;
        
        private Dictionary<RewardType, Sprite> _spriteMap;

        private void Awake()
        {
            _spriteMap = new();
            foreach (var rewardDataItem in _rewardData.WheelRewardDataItems)
            {
                _spriteMap[rewardDataItem.Type] = rewardDataItem.Sprite;
            }
        }
        
        private void Start()
        {
            _wheelController.Init(_rewardSelection, _spriteMap, this);
            _gainedRewardsController.Init(_spriteMap);
        }

        public void GiveReward(WheelReward reward)
        {
            var targetGainedReward = _gainedRewardsController.GetTargetForRewardCollectible(reward.RewardType);
            var sprite = _spriteMap[reward.RewardType];
            var currentCollectibleAmount = Mathf.Min(_maxCollectibleAmount, reward.RewardAmount);
            var baseIncreaseAmount = reward.RewardAmount /  currentCollectibleAmount;
            var lastIncreaseOffset = reward.RewardAmount % currentCollectibleAmount;
            
            for (var i = 0; i < currentCollectibleAmount; i++)
            {
                DOVirtual.DelayedCall(i * _collectibleSpawnDelay, () =>
                {
                    var increaseAmount = baseIncreaseAmount;
                    if (i == reward.RewardAmount - 1)
                    {
                        increaseAmount += lastIncreaseOffset;
                    }
                    var collectible = Instantiate(_rewardCollectiblePrefab, _rewardCollectibleSpawnRef)
                        .GetComponent<RewardCollectible>();
                    collectible.Init(sprite, targetGainedReward, increaseAmount);
                });
            }
        }
    }
}