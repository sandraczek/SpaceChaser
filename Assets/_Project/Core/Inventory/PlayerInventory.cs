using System.Collections.Generic;
using SpaceChaser.Core.Registry;
using UnityEngine;

namespace SpaceChaser.Core.Inventory
{
    public class PlayerInventory
    {
        private readonly Dictionary<AssetId, int> _inventory = new();


        public void DebugInitialize()
        {
            _inventory.Add(new("metal"), 1000);
            _inventory.Add(new("plastic"), 1000);
            _inventory.Add(new("wood"), 1000);
        }
        public void Add(AssetId id, int amount)
        {
            if (!_inventory.ContainsKey(id))
                _inventory.Add(id, 0);
            _inventory[id] += amount;

        }
        public void Remove(AssetId id, int amount)
        {
            if (!_inventory.ContainsKey(id))
                Debug.LogWarning("Tried to remove a item that is not in the inventory");
            _inventory[id] -= amount;
        }
        public bool Has(AssetId id, int amount)
        {
            if (!_inventory.ContainsKey(id))
                return false;
            return _inventory[id] >= amount;
        }
    }
}