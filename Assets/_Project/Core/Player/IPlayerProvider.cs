using System;

namespace SpaceChaser.Core.Player
{
    public interface IPlayerProvider
    {
        bool IsPlayerSpawned { get; }
        event Action OnPlayerRegistered;
        event Action OnPlayerUnregistered;
    }
}