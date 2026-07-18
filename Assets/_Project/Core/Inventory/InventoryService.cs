using System;
using SpaceChaser.Core.Player;
using VContainer.Unity;

namespace SpaceChaser.Core.Inventory
{
    public class InventoryService : IInventoryService, IStartable, IDisposable
    {
        private PlayerInventory _inventory;
        private IPlayerProvider _player;

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
        }
    }
}