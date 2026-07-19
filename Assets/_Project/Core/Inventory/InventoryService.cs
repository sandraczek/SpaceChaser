using System;
using System.Collections.Generic;
using NUnit.Framework;
using SpaceChaser.Core.Building;
using SpaceChaser.Core.Player;
using Unity.VisualScripting;
using UnityEngine;
using VContainer.Unity;

namespace SpaceChaser.Core.Inventory
{
    public class InventoryService : IInventoryService, IStartable, IDisposable
    {
        private PlayerInventory _inventory;
        private readonly IPlayerProvider _player;

        public InventoryService(IPlayerProvider player)
        {
            _player = player;
        }
        public void Start()
        {
            _player.OnPlayerRegistered += HandlePlayerRegistered;
        }

        public void Dispose()
        {
            _player.OnPlayerRegistered -= HandlePlayerRegistered;
        }


        public void HandlePlayerRegistered()
        {
            _inventory = new();
            _inventory.DebugInitialize();                           // DEBUG
        }

        public bool Has(IReadOnlyList<ItemAmount> items)
        {
            foreach (ItemAmount item in items)
            {
                if (!_inventory.Has(item.Item.Id, item.Amount)) return false;
            }

            return true;
        }

        public void Remove(IReadOnlyList<ItemAmount> items)
        {
            foreach (ItemAmount item in items)
            {
                _inventory.Remove(item.Item.Id, item.Amount);
            }
        }
        public void Add(IReadOnlyList<ItemAmount> items)
        {
            foreach (ItemAmount item in items)
            {
                _inventory.Add(item.Item.Id, item.Amount);
            }
        }
        public void Add(IReadOnlyList<ItemAmount> items, float reduce)
        {
            foreach (ItemAmount item in items)
            {
                _inventory.Add(item.Item.Id, Mathf.RoundToInt(item.Amount * reduce));
            }
        }
    }
}