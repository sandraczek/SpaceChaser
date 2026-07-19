using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceChaser.Core.Islands
{
    public interface IIslandService
    {
        public void SetupIslands();
        public Vector2 NextIslandPosition { get; }
    }
}