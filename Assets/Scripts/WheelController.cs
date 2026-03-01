using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardWheel
{
    public class WheelController : MonoBehaviour
    {
        [SerializeField] private List<WheelMovementData> _movementChain;
        [SerializeField] private Transform _wheelTransform;
        
        public static event Action OnSpinButtonClicked;

        private float _currentWheelAngle = 0f;
        private bool _isSpinning = false;
        private int _chainIndex = 0;
        private List<AngleIntervalUnit> _rewardAngleIntervals = new();

        private const int _rewardCount = 8;
        
        private void Awake()
        {
            OnSpinButtonClicked = null;
            InitAngleIntervals();
        }

        private void InitAngleIntervals()
        {
            var angleDiff = 360f / _rewardCount;
            var halfRadius = angleDiff / 2f;
            for (var i = 0; i < _rewardCount; i++)
            {
                _rewardAngleIntervals.Add(new AngleIntervalUnit(i * angleDiff, halfRadius));
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
            var turnAmount = GetTurnAmountForMovement();
            var targetAngle = _currentWheelAngle + turnAmount;
            var startAngle = _currentWheelAngle;
            var duration = Mathf.Abs(turnAmount / data.TurnSpeed);
            var curve = data.MovementCurve;
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

            float GetTurnAmountForMovement()
            {
                var turnCount = Random.Range(data.MinTurnCount, data.MaxTurnCount + 1);
                return turnCount * WheelMovementData.TurnAmountPerReward;

            }
        }

        private void OnWheelRotationOver()
        {
            _isSpinning = false;
            _chainIndex = 0;
        }
        
        public static void TriggerSpinButtonClicked()
        {
            OnSpinButtonClicked?.Invoke();
        }

        private void OnEnable()
        {
            OnSpinButtonClicked += TurnWheel;
        }

        private void OnDisable()
        {
            OnSpinButtonClicked -= TurnWheel;
        }
    }

    public struct AngleIntervalUnit
    {
        public float Min;
        public float Max;
        public float Center;

        public AngleIntervalUnit(float center, float halfRadius)
        {
            Center = center;
            Min = Center - halfRadius;
            Max = Center + halfRadius;
        }
    }
}