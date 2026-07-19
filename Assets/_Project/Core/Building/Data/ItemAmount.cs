using System;
using System.Collections.Generic;
using SpaceChaser.Core.Inventory;
using UnityEngine;

namespace SpaceChaser.Core.Building
{
    [Serializable]
    public struct ItemAmount
    {
        public ItemData Item;
        public int Amount;

        public ItemAmount(ItemData item, int amount)
        {
            Item = item;
            Amount = amount;
        }
    }
}