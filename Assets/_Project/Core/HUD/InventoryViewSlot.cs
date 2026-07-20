using System;
using System.Collections.Generic;
using SpaceChaser.Core.Inputs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace SpaceChaser.Core.HUD
{
    public class InventoryViewSlot : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Image _image;

        private event Action _onSelect;

        public void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }
        private void Awake()
        {
            //_iconRenderer = GetComponentInChildren<Image>();
        }
        public void Initialize(Action onSelect)
        {
            _onSelect = onSelect;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _onSelect?.Invoke();
            }
        }
    }
}