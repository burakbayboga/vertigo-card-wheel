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
        [SerializeField] private NextSpinsController _nextSpinsController;

        [SerializeField] private int _maxCollectibleAmount;
        [SerializeField] private float _collectibleSpawnDelay;
        public static int SafeZoneInterval = 5;
        public static int SuperZoneInterval = 30;
        
        private Dictionary<RewardType, Sprite> _spriteMap;
        private int _currentWheelIndex;
        private Dictionary<RewardType, int> _currentInventory = new();
        private int _reviveGoldCost = 25;

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
            _nextSpinsController.Init();
            InitNextWheel();
        }

        private void InitNextWheel()
        {
            _wheelController.ResetWheel();
            _currentWheelIndex++;
            List<WheelTypeData> wheelTypePool;
            if (_currentWheelIndex % SuperZoneInterval == 0)
            {
                wheelTypePool = _goldenWheels;
            }
            else if (_currentWheelIndex % SafeZoneInterval == 0)
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
            _nextSpinsController.UpdateItems(_currentWheelIndex);
        }

        public void GiveReward(WheelReward reward)
        {
            if (reward.RewardType == RewardType.Bomb)
            {
                OnBombPicked();
                return;
            }

            if (!_currentInventory.TryAdd(reward.RewardType, reward.RewardAmount))
            {
                _currentInventory[reward.RewardType] += reward.RewardAmount;
            }
            
            var targetGainedReward = _gainedRewardsController.GetTargetForRewardCollectible(reward.RewardType);
            var sprite = _spriteMap[reward.RewardType];
            var currentCollectibleAmount = Mathf.Min(_maxCollectibleAmount, reward.RewardAmount);
            var baseIncreaseAmount = reward.RewardAmount /  currentCollectibleAmount;
            var lastIncreaseOffset = reward.RewardAmount % currentCollectibleAmount;
            
            for (var i = 0; i < currentCollectibleAmount; i++)
            {
                var increaseAmount = baseIncreaseAmount;
                var isLastCollectible = i == currentCollectibleAmount - 1;
                if (isLastCollectible)
                {
                    increaseAmount += lastIncreaseOffset;
                }
                DOVirtual.DelayedCall(i * _collectibleSpawnDelay, () =>
                {
                    var collectible = Instantiate(_rewardCollectiblePrefab, _rewardCollectibleSpawnRef)
                        .GetComponent<RewardCollectible>();
                    collectible.Init(sprite, targetGainedReward, increaseAmount);
                    if (isLastCollectible)
                    {
                        InitNextWheel();
                    }
                });
            }
        }

        private void OnBombPicked()
        {
            _bombPickedPanel.SetActive(true);
        }
        
        private void OnGiveUpClicked()
        {
            _bombPickedPanel.SetActive(false);
            _currentWheelIndex = 0;
            InitNextWheel();
        }

        private void OnReviveClicked()
        {
            if (_currentInventory.TryGetValue(RewardType.Gold, out var goldAmount))
            {
                if (goldAmount < _reviveGoldCost)
                {
                    return;
                }
            }
            else
            {
                return;
            }
            
            _bombPickedPanel.SetActive(false);
            _currentInventory[RewardType.Gold] -= _reviveGoldCost;
            _gainedRewardsController.SetRewardAmount(RewardType.Gold, _currentInventory[RewardType.Gold]);
            InitNextWheel();
        }

        private void OnEnable()
        {
            GiveUpButton.OnGiveUpClicked += OnGiveUpClicked;
            ReviveButton.OnReviveClicked += OnReviveClicked;
        }

        private void OnDisable()
        {
            GiveUpButton.OnGiveUpClicked -= OnGiveUpClicked;
            ReviveButton.OnReviveClicked -= OnReviveClicked;
        }
    }
}