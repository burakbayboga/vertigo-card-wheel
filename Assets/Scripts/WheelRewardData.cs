using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardWheel
{
    [CreateAssetMenu(fileName = "WheelRewardData", menuName = "ScriptableObjects/Wheel Reward Data")]
    public class WheelRewardData : ScriptableObject
    {
        public List<WheelRewardDataItem> WheelRewardDataItems;
    }

    [Serializable]
    public struct WheelRewardDataItem
    {
        public RewardType Type;
        public Sprite Sprite;
    }
}