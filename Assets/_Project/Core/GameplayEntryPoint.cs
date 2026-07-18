using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SpaceChaser.Core.Building;
using SpaceChaser.Core.Player;
using SpaceChaser.Core.Registry;
using VContainer.Unity;


namespace SpaceChaser.Core
{
    public sealed class GameplayEntryPoint : IAsyncStartable
    {
        private readonly IPlayerSpawner _playerSpawner;
        private readonly IBuildFactory _buildFactory;
        private readonly IAssetRegistry<BuildData> _buildRegistry;
        public GameplayEntryPoint(IPlayerSpawner playerSpawner, IBuildFactory buildFactory, IAssetRegistry<BuildData> buildRegistry)
        {
            _playerSpawner = playerSpawner;
            _buildFactory = buildFactory;
            _buildRegistry = buildRegistry;
        }
        public async UniTask StartAsync(CancellationToken token)
        {
            await _buildFactory.PrewarmPoolAsync(_buildRegistry.Get(new("bathtub_build")), token);

            _playerSpawner.SpawnPlayer(new(0, 0));

            await UniTask.WaitForEndOfFrame();
        }
    }
}