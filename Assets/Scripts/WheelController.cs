using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardWheel
{
    public class WheelController : MonoBehaviour
    {
        [SerializeField] private List<WheelMovementData> _movementChain;
        [SerializeField] private Transform _wheelTransform;
        [SerializeField] private List<WheelRewardContainer> _rewardContainers;
        
        private float _currentWheelAngle = 0f;
        private bool _isSpinning = false;
        private int _chainIndex = 0;
        private int _currentRewardIndex = 0;
        private List<WheelReward> _rewardsReordered;
        private GameController _gameController;

        private const int _rewardCount = 8;

        public void Init(WheelRewardSelection rewardPool, Dictionary<RewardType, Sprite> spriteMap, GameController gameController)
        {
            _gameController = gameController;
            _rewardsReordered = rewardPool.Rewards.OrderBy(_ => Random.Range(0f, 1f)).ToList();
            
            for (var i = 0; i < _rewardCount; i++)
            {
                var reward = _rewardsReordered[i];
                var sprite = spriteMap[reward.RewardType];
                var amount = reward.RewardAmount;
                _rewardContainers[i].Init(reward.RewardType, sprite, amount);
            }
        }

        private void TurnWheel()
        {
            if (_isSpinning) return;
            
            _isSpinning = true;
            
            ApplyMovement(_movementChain);
        }
        
        private void ApplyMovement(List<WheelMovementData> chain)
        {
            _currentWheelAngle %= 360f;
            if (_currentWheelAngle < 0f)
            {
                _currentWheelAngle += 360f;
            }
            var data = chain[_chainIndex];
            var turnAmount = GetTurnAmountForMovement(out var turnCount);
            var targetAngle = _currentWheelAngle + turnAmount;
            var startAngle = _currentWheelAngle;
            var duration = Mathf.Abs(turnAmount / data.TurnSpeed);
            var curve = data.MovementCurve;
            _currentRewardIndex += turnCount;
            DOVirtual.Float(0f, 1f, duration, t =>
            {
                _currentWheelAngle = Mathf.Lerp(startAngle, targetAngle, curve.Evaluate(t));
                var wheelRot = Quaternion.Euler(new Vector3(0f, 0f, _currentWheelAngle));
                _wheelTransform.rotation = wheelRot;
            }).OnComplete(() =>
            {
                _chainIndex++;
                if (_chainIndex < chain.Count)
                {
                    ApplyMovement(chain);
                }
                else
                {
                    OnWheelRotationOver();
                }
            });

            float GetTurnAmountForMovement(out int turnCount)
            {
                turnCount = Random.Range(data.MinTurnCount, data.MaxTurnCount + 1);
                return turnCount * WheelMovementData.TurnAmountPerReward;
            }
        }

        private void OnWheelRotationOver()
        {
            _isSpinning = false;
            _chainIndex = 0;
            _currentRewardIndex %= _rewardCount;
            if (_currentRewardIndex < 0)
            {
                _currentRewardIndex += _rewardCount;
            }
            
            _gameController.GiveReward(_rewardsReordered[_currentRewardIndex]);
        }
        


        private void OnEnable()
        {
            SpinButton.OnSpinButtonClicked += TurnWheel;
        }

        private void OnDisable()
        {
            SpinButton.OnSpinButtonClicked -= TurnWheel;
        }
    }
}