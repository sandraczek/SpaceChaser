using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;
using VContainer.Unity;

namespace SpaceChaser.Core.Islands
{
    public interface IIslandFactory
    {
        public Island Create(Vector3 position);
        public UniTask PrewarmPoolsAsync(CancellationToken cancellation);
    }
}