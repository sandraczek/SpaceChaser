using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;
using VContainer.Unity;

namespace SpaceChaser.Core.Building
{
    public interface IBuildFactory
    {
        public Build Create(BuildData data, Vector3 position);

        public UniTask PrewarmPoolAsync(BuildData data, CancellationToken cancellation);

    }
}