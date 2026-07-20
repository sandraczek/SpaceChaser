using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace SpaceChaser.Core.HUD
{
    public class InventoryView : MonoBehaviour
    {
        private GameConfig _config;
        private InventoryViewSlot _cursorSlot;
        private List<InventoryViewSlot> _slots = new(12);

        public event Action<int> OnSelected;

        [Inject]
        public void Construct(GameConfig config)
        {
            _config = config;
        }

        private void Awake()
        {
            GetComponentsInChildren(_slots);
            if (_slots.Count < _config.BuildingSlots + _config.StrutSlots + _config.FoundationSlots + 1)
                Debug.LogError("Too few inventory slots!");

            _slots = _slots.OrderBy(x => x.transform.position.x).ToList();

            _cursorSlot = _slots[0];
            _slots.RemoveAt(0);
        }

        private void Start()
        {
            _cursorSlot.Initialize(() => OnSelected?.Invoke(-1));
            for (int i = 0; i < _slots.Count; i++)
            {
                int j = i;
                _slots[j].Initialize(() => OnSelected?.Invoke(j));
            }
        }

        public void SetOnIndex(int index, Sprite sprite)
        {
            _slots[index].SetSprite(sprite);
        }
    }
}