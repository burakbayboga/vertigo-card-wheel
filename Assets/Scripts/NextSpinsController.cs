using System.Collections.Generic;
using UnityEngine;

namespace CardWheel
{
    public class NextSpinsController : MonoBehaviour
    {
        [SerializeField] private GameObject _nextSpinItemPrefab;
        [SerializeField] private int _itemsToShowCount;
        
        private List<NextSpinItem> _nextSpinItems;

        public void Init()
        {
            _nextSpinItems = new();
            for (var i = 0; i < _itemsToShowCount; i++)
            {
                var newItem = Instantiate(_nextSpinItemPrefab, transform).GetComponent<NextSpinItem>();
                _nextSpinItems.Add(newItem);
            }
        }

        public void UpdateItems(int currentWheelIndex)
        {
            for (var i = 0; i < _nextSpinItems.Count; i++)
            {
                var indexForItem = currentWheelIndex + i + 1;
                WheelType wheelType;
                if (indexForItem % GameController.SuperZoneInterval == 0)
                {
                    wheelType = WheelType.Golden;
                }
                else if (indexForItem % GameController.SafeZoneInterval == 0)
                {
                    wheelType = WheelType.Silver;
                }
                else
                {
                    wheelType = WheelType.Standard;
                }
                _nextSpinItems[i].SetSpin(wheelType, indexForItem);
            }
        }
    }
}