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
    public interface IFoundationFactory
    {
        public Foundation Create(FoundationData data, Vector3 position, float rotation);

        public UniTask PrewarmPoolAsync(FoundationData data, CancellationToken cancellation);

    }
}