using System;
using SpaceChaser.Core.Highscore;
using SpaceChaser.Core.Player;
using VContainer.Unity;

namespace SpaceChaser.Core.Death
{
    public interface IDeathService
    {

        public event Action OnDeath;
    }
}