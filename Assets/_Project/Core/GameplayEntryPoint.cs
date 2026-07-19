using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SpaceChaser.Core.Building;
using SpaceChaser.Core.Islands;
using SpaceChaser.Core.Player;
using SpaceChaser.Core.Registry;
using VContainer.Unity;


namespace SpaceChaser.Core
{
    public sealed class GameplayEntryPoint : IAsyncStartable
    {
        private readonly IPlayerSpawner _playerSpawner;
        private readonly IBuildFactory _buildFactory;
        private readonly IStrutFactory _strutFactory;
        private readonly IFoundationFactory _foundationFactory;
        private readonly IIslandFactory _islandFactory;
        private readonly IAssetRegistry<BuildData> _buildRegistry;
        private readonly IAssetRegistry<StrutData> _strutRegistry;
        private readonly IAssetRegistry<FoundationData> _foundationRegistry;
        public GameplayEntryPoint(
            IPlayerSpawner playerSpawner,
            IBuildFactory buildFactory,
            IStrutFactory strutFactory,
            IFoundationFactory foundationFactory,
            IIslandFactory islandFactory,
            IAssetRegistry<BuildData> buildRegistry,
            IAssetRegistry<StrutData> strutRegistry,
            IAssetRegistry<FoundationData> foundationRegistry
            )
        {
            _playerSpawner = playerSpawner;
            _buildFactory = buildFactory;
            _strutFactory = strutFactory;
            _foundationFactory = foundationFactory;
            _islandFactory = islandFactory;
            _buildRegistry = buildRegistry;
            _strutRegistry = strutRegistry;
            _foundationRegistry = foundationRegistry;
        }
        public async UniTask StartAsync(CancellationToken token)
        {
            foreach (var build in _buildRegistry.Assets)
                await _buildFactory.PrewarmPoolAsync(build, token);
            foreach (var strut in _strutRegistry.Assets)
                await _strutFactory.PrewarmPoolAsync(strut, token);
            foreach (var foundation in _foundationRegistry.Assets)
                await _foundationFactory.PrewarmPoolAsync(foundation, token);

            await _islandFactory.PrewarmPoolsAsync(token);

            _playerSpawner.SpawnPlayer(new(0, 5));

            await UniTask.WaitForEndOfFrame();
        }
    }
}