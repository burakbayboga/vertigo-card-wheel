using System.Collections.Generic;
using UnityEngine;

namespace CardWheel
{
    public class GainedRewardsController : MonoBehaviour
    {
        [SerializeField] private Transform _rewardsContainer;
        [SerializeField] private GameObject _gainedRewardPrefab;

        private List<GainedReward> _gainedRewards = new();
        private Dictionary<RewardType, Sprite> _spriteMap;

        public void Init(Dictionary<RewardType, Sprite> spriteMap)
        {
            _spriteMap = spriteMap;
        }
        
        public GainedReward GetTargetForRewardCollectible(RewardType rewardType)
        {
            foreach (var gainedReward in _gainedRewards)
            {
                if (gainedReward.RewardType == rewardType)
                {
                    return gainedReward;
                }
            }
            
            var newGainedReward = Instantiate(_gainedRewardPrefab, _rewardsContainer).GetComponent<GainedReward>();
            newGainedReward.Init(rewardType, _spriteMap[rewardType]);
            _gainedRewards.Add(newGainedReward);
            return newGainedReward;
        }
        
        private void OnGiveUpClicked()
        {
            foreach (var reward in _gainedRewards)
            {
                Destroy(reward.gameObject);
            }
            
            _gainedRewards.Clear();
        }

        private void OnEnable()
        {
            GiveUpButton.OnGiveUpClicked += OnGiveUpClicked;
        }

        private void OnDisable()
        {
            GiveUpButton.OnGiveUpClicked -= OnGiveUpClicked;
        }
    }
}