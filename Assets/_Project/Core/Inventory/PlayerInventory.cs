using System.Collections.Generic;

namespace SpaceChaser.Core.Inventory
{
    public class PlayerInventory
    {
        private Dictionary<int, int> _inventory;

        public void Add(int id, int amount)
        {
            _inventory[id] += amount;
        }
        public void Remove(int id, int amount)
        {
            _inventory[id] -= amount;
        }
        public bool Has(int id, int amount)
        {
            return _inventory[id] >= amount;
        }
    }
}