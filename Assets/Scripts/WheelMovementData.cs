using UnityEngine;

namespace CardWheel
{
    [CreateAssetMenu(fileName = "WheelMovementData_", menuName = "ScriptableObjects/New Wheel Movement Data")]
    public class WheelMovementData : ScriptableObject
    {
        public const float TurnAmountPerReward = 45f;
        
        public int MinTurnCount;
        public int MaxTurnCount;
        public float TurnSpeed;
        public AnimationCurve MovementCurve;
    }
}