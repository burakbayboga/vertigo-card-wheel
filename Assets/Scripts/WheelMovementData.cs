using UnityEngine;

[CreateAssetMenu(fileName = "WheelMovementData_", menuName = "ScriptableObjects/New Wheel Movement Data")]
public class WheelMovementData : ScriptableObject
{
    public float MinTurnAmount;
    public float MaxTurnAmount;
    public float TurnSpeed;
    public AnimationCurve MovementCurve;
}
