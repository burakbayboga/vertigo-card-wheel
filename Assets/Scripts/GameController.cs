using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace CardWheel
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private List<WheelTypeData> _standardWheels;
        [SerializeField] private List<WheelTypeData> _silverWheels;
        [SerializeField] private List<WheelTypeData> _goldenWheels;
        [SerializeField] private WheelController _wheelController;
        [SerializeField] private WheelRewardData _rewardData;
        [SerializeField] private WheelRewardSelection _rewardSelection;
        [SerializeField] private Transform _rewardCollectibleSpawnRef;
        [SerializeField] private GameObject _rewardCollectiblePrefab;
        [SerializeField] private GainedRewardsController _gainedRewardsController;
        [SerializeField] private GameObject _bombPickedPanel;

        [SerializeField] private int _maxCollectibleAmount;
        [SerializeField] private float _collectibleSpawnDelay;
        [SerializeField] private int _safeZoneInterval;
        [SerializeField] private int _superZoneInterval;
        
        private Dictionary<RewardType, Sprite> _spriteMap;
        private int _currentWheelIndex;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            _spriteMap = new();
            foreach (var rewardDataItem in _rewardData.WheelRewardDataItems)
            {
                _spriteMap[rewardDataItem.Type] = rewardDataItem.Sprite;
            }
        }
        
        private void Start()
        {
            _gainedRewardsController.Init(_spriteMap);
            _wheelController.Init(_spriteMap, this);
            InitNextWheel();
        }

        private void InitNextWheel()
        {
            _currentWheelIndex++;
            List<WheelTypeData> wheelTypePool;
            if (_currentWheelIndex % _superZoneInterval == 0)
            {
                wheelTypePool = _goldenWheels;
            }
            else if (_currentWheelIndex % _safeZoneInterval == 0)
            {
                wheelTypePool = _silverWheels;
            }
            else
            {
                wheelTypePool = _standardWheels;
            }
            
            var wheelTypeData = wheelTypePool[Random.Range(0, wheelTypePool.Count)];
            var rewardPool = wheelTypeData.WheelRewardSelections[Random.Range(0, wheelTypeData.WheelRewardSelections.Count)];
            _wheelController.PrepareForSpin(rewardPool, wheelTypeData.WheelType);
        }

        public void GiveReward(WheelReward reward)
        {
            if (reward.RewardType == RewardType.Bomb)
            {
                OnBombPicked();
                return;
            }
            
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
            
            InitNextWheel();
        }

        private void OnBombPicked()
        {
            _bombPickedPanel.SetActive(true);
        }
        
        private void OnGiveUpClicked()
        {
            _bombPickedPanel.SetActive(false);
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