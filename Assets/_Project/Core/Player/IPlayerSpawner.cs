using UnityEngine;

namespace SpaceChaser.Core.Player
{
    public interface IPlayerSpawner
    {
        void SpawnPlayer(Vector2 spawnPosition);
        void UnregisterPlayer();
    }
}