using System;
using UnityEngine;

namespace SpaceChaser.Core.Player
{
    public interface IPlayerProvider
    {
        Transform Transform { get; }
        bool IsPlayerSpawned { get; }
        event Action OnPlayerRegistered;
        event Action OnPlayerUnregistered;
    }
}