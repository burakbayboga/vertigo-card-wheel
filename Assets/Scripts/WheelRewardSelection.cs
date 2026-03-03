using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardWheel
{
    [CreateAssetMenu(fileName = "WheelRewardSelection_", menuName = "ScriptableObjects/Wheel Reward Selection")]
    public class WheelRewardSelection : ScriptableObject
    {
        public List<WheelReward> Rewards;
    }

    [Serializable]
    public struct WheelReward
    {
        public RewardType RewardType;
        public int RewardAmount;
    }
}