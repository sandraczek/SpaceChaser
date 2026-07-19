using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using SpaceChaser.Core.Registry;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;
using VContainer.Unity;

namespace SpaceChaser.Core.Islands
{
    public class IslandFactory : IIslandFactory, IDisposable
    {
        private readonly IObjectResolver _resolver;
        private readonly List<IObjectPool<Island>> _pools = new();
        private readonly IReadOnlyList<Island> _islands;

        private readonly int _defaultPoolSize = 10;
        private readonly int _maxPoolSize = 100;
        private readonly int _prewarmBatchSize = 5;
        private readonly Transform _parent;
        private bool _initialized = false;

        public IslandFactory(IObjectResolver resolver, IReadOnlyList<Island> islands, Transform parent)
        {
            _resolver = resolver;
            _parent = parent;
            _islands = islands;
        }

        public void Dispose()
        {
            foreach (var pool in _pools)
            {
                pool.Clear();
            }
            _pools.Clear();
        }

        public Island Create(Vector3 position)
        {
            if (!_initialized)
            {
                Debug.LogWarning("Island Factory uninitialized!");
                return null;
            }
            if (_pools.Count == 0)
            {
                Debug.LogWarning("No islands in factory");
                return null;
            }

            var pool = _pools[UnityEngine.Random.Range(0, _pools.Count)];

            Island island = pool.Get();
            island.transform.position = position;
            island.Initialize(() =>
                {
                    pool.Release(island);
                });
            return island;
        }

        private IObjectPool<Island> CreatePool(Island island)
        {
            GameObject poolParent = new($"Pool_{island.name}");
            poolParent.transform.SetParent(_parent);

            return new ObjectPool<Island>(
                createFunc: () =>
                {
                    var b = _resolver.Instantiate(island, poolParent.transform);
                    return b;
                },
                actionOnGet: b => b.gameObject.SetActive(true),
                actionOnRelease: b => b.gameObject.SetActive(false),
                actionOnDestroy: b => { if (b != null) UnityEngine.Object.Destroy(b.gameObject); },
                defaultCapacity: _defaultPoolSize,
                maxSize: _maxPoolSize
            );
        }

        public async UniTask PrewarmPoolsAsync(CancellationToken cancellation)
        {
            var prewarmedObjects = new List<Island>(_defaultPoolSize);
            foreach (var pool in _pools)
            {
                pool.Clear();
            }
            _pools.Clear();

            foreach (var island in _islands)
            {
                var pool = CreatePool(island);
                _pools.Add(pool);

                for (int i = 0; i < _defaultPoolSize; i++)
                {
                    prewarmedObjects.Add(pool.Get());
                    if ((i + 1) % _prewarmBatchSize == 0)
                        await UniTask.Yield(cancellation);
                }

                foreach (var b in prewarmedObjects)
                {
                    pool.Release(b);
                }
                prewarmedObjects.Clear();
            }

            _initialized = true;
        }
    }
}