using System.Collections.Generic;
using SpaceChaser.Core.Building;

namespace SpaceChaser.Core.Inventory
{
    public interface IInventoryService
    {
        public bool Has(IReadOnlyList<ItemAmount> items);
        public void Remove(IReadOnlyList<ItemAmount> items);
        public void Add(IReadOnlyList<ItemAmount> items);
        public void Add(IReadOnlyList<ItemAmount> items, float reduce);
    }
}