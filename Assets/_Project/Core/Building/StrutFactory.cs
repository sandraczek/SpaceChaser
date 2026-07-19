using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using SpaceChaser.Core.Registry;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;
using VContainer.Unity;

namespace SpaceChaser.Core.Building
{
    public class StrutFactory : IStrutFactory, IDisposable
    {
        private readonly IObjectResolver _resolver;
        private readonly Dictionary<AssetId, IObjectPool<Strut>> _pools = new();

        private readonly int _defaultPoolSize = 10;
        private readonly int _maxPoolSize = 100;
        private readonly int _prewarmBatchSize = 5;
        private readonly Transform _parent;

        public StrutFactory(IObjectResolver resolver, Transform buildParent)
        {
            _resolver = resolver;
            _parent = buildParent;
        }

        public void Dispose()
        {
            foreach (var pool in _pools.Values)
            {
                pool.Clear();
            }
            _pools.Clear();
        }

        public Strut Create(StrutData data, Vector3 position, float rotation)
        {
            if (!_pools.TryGetValue(data.Id, out IObjectPool<Strut> pool))
            {
                pool = CreatePool(data.Prefab);
                _pools[data.Id] = pool;
            }

            Strut b = pool.Get();
            b.transform.position = position;
            b.transform.rotation = Quaternion.Euler(0f, 0f, rotation);
            b.Initialize(() =>
                {
                    pool.Release(b);
                });
            return b;
        }

        private IObjectPool<Strut> CreatePool(Strut prefab)
        {
            GameObject poolParent = new($"Pool_{prefab.Data.DisplayName}");
            poolParent.transform.SetParent(_parent);

            return new ObjectPool<Strut>(
                createFunc: () =>
                {
                    var b = _resolver.Instantiate(prefab, poolParent.transform);
                    b.name = prefab.Data.DisplayName;
                    return b;
                },
                actionOnGet: b => b.gameObject.SetActive(true),
                actionOnRelease: b => b.gameObject.SetActive(false),
                actionOnDestroy: b => { if (b != null) UnityEngine.Object.Destroy(b.gameObject); },
                defaultCapacity: _defaultPoolSize,
                maxSize: _maxPoolSize
            );
        }

        public async UniTask PrewarmPoolAsync(StrutData data, CancellationToken cancellation)
        {
            var prewarmedObjects = new List<Strut>(_defaultPoolSize);

            if (!_pools.ContainsKey(data.Id))
                _pools[data.Id] = CreatePool(data.Prefab);

            for (int i = 0; i < _defaultPoolSize; i++)
            {
                prewarmedObjects.Add(_pools[data.Id].Get());
                if ((i + 1) % _prewarmBatchSize == 0)
                    await UniTask.Yield(cancellation);
            }

            foreach (var b in prewarmedObjects)
            {
                _pools[data.Id].Release(b);
            }
        }
    }
}