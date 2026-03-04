using System.Collections.Generic;
using UnityEngine;

namespace CardWheel
{
    [CreateAssetMenu(fileName = "WheelType_", menuName = "ScriptableObjects/Wheel Type Data")]
    public class WheelTypeData : ScriptableObject
    {
        public WheelType WheelType;
        public List<WheelRewardSelection> WheelRewardSelections;
    }

    public enum WheelType
    {
        Standard = 0,
        Silver = 1,
        Golden = 2
    }
}