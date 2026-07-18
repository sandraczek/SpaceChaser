using VContainer;
using VContainer.Unity;
using UnityEngine;
using SpaceChaser.Core.Player;
using SpaceChaser.Core;
using SpaceChaser.Core.Building;
using SpaceChaser.Core.Registry;

public class GameplayLifetimeScope : LifetimeScope
{
    [SerializeField] private BuildRegistry _buildRegistry;
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _buildParent;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_playerConfig);

        _buildRegistry.Editor_FindAllAssets();
        _buildRegistry.Initialize();
        builder.RegisterInstance(_buildRegistry).As<IAssetRegistry<BuildData>>();

        builder.RegisterEntryPoint<BuildFactory>(Lifetime.Scoped)
        .AsImplementedInterfaces()
        .WithParameter(_buildParent);
        builder.Register<BuildService>(Lifetime.Scoped).AsImplementedInterfaces();

        builder.Register<PlayerProvider>(Lifetime.Scoped)
        .AsImplementedInterfaces()
        .WithParameter(_playerPrefab);

        builder.RegisterEntryPoint<GameplayEntryPoint>(Lifetime.Scoped);
    }


    /* todo

    - think about registry: is it okay for jam?



    */
}
