using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CardWheel
{
    public class RewardCollectible : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private AnimationCurve _moveCurve;
        [SerializeField] private AnimationCurve _arcCurve;
        [SerializeField] private Vector2 _arcVector;
        
        [SerializeField] private Image _rewardImage;

        public void Init(Sprite rewardSprite, GainedReward targetGainedReward, int increaseAmount)
        {
            _rewardImage.sprite = rewardSprite;

            DOVirtual.DelayedCall(0.01f, () =>
            {
                transform.SetParent(targetGainedReward.transform);

                var rectT = GetComponent<RectTransform>();
                var startPos = rectT.anchoredPosition;
                var targetPos = Vector2.zero;
                var duration = Vector2.Distance(startPos, targetPos) /  _moveSpeed;
            
                DOVirtual.Float(0f, 1f,  duration, t =>
                {
                    var basePos = Vector2.Lerp(startPos, targetPos, _moveCurve.Evaluate(t));
                    var arcOffset = _arcVector * _arcCurve.Evaluate(t);
                    rectT.anchoredPosition = basePos + arcOffset;
                }).OnComplete(() =>
                {
                    targetGainedReward.OnCollectibleArrive(increaseAmount);
                    Destroy(gameObject);
                });
            });
        }
    }
}