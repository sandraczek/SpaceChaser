using System;
using UnityEngine;

namespace SpaceChaser.Core.Building
{
    public class Build : MonoBehaviour
    {
        public BuildData Data;
        private Action _onDestroy;
        public void Initialize(Action onDestroy)
        {
            _onDestroy = onDestroy;
        }

        private void HandleDeath()
        {
            _onDestroy.Invoke();
        }
    }
}