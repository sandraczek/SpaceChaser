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
    public interface IStrutFactory
    {
        public Strut Create(StrutData data, Vector3 position, float rotation);

        public UniTask PrewarmPoolAsync(StrutData data, CancellationToken cancellation);

    }
}