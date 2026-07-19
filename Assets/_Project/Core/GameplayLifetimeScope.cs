using VContainer;
using VContainer.Unity;
using UnityEngine;
using SpaceChaser.Core.Player;
using SpaceChaser.Core;
using SpaceChaser.Core.Building;
using SpaceChaser.Core.Registry;
using SpaceChaser.Core.Inventory;
using SpaceChaser.Core.Highscore;
using SpaceChaser.Core.Cam;
using System.Collections.Generic;
using SpaceChaser.Core.Islands;

public class GameplayLifetimeScope : LifetimeScope
{
    [SerializeField] private BuildRegistry _buildRegistry;
    [SerializeField] private StrutRegistry _strutRegistry;
    [SerializeField] private FoundationRegistry _foundationRegistry;
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _buildParent;
    [SerializeField] private BuildPreview _buildPreview;
    [SerializeField] private List<Island> _islands;
    [SerializeField] private HighscoreView _highscoreLine;
    [SerializeField] private CameraFollow _camera;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_playerConfig);

        _buildRegistry.Editor_FindAllAssets();
        _buildRegistry.Initialize();
        builder.RegisterInstance(_buildRegistry).As<IAssetRegistry<BuildData>>();
        _strutRegistry.Editor_FindAllAssets();
        _strutRegistry.Initialize();
        builder.RegisterInstance(_strutRegistry).As<IAssetRegistry<StrutData>>();
        _foundationRegistry.Editor_FindAllAssets();
        _foundationRegistry.Initialize();
        builder.RegisterInstance(_foundationRegistry).As<IAssetRegistry<FoundationData>>();

        builder.RegisterComponent(_buildPreview);

        builder.Register<PlayerProvider>(Lifetime.Scoped)
        .AsImplementedInterfaces()
        .WithParameter(_playerPrefab);

        builder.RegisterComponent(_camera);

        builder.RegisterEntryPoint<BuildFactory>(Lifetime.Scoped)
        .AsImplementedInterfaces()
        .WithParameter(_buildParent);
        builder.RegisterEntryPoint<StrutFactory>(Lifetime.Scoped)
        .AsImplementedInterfaces()
        .WithParameter(_buildParent);
        builder.RegisterEntryPoint<FoundationFactory>(Lifetime.Scoped)
        .AsImplementedInterfaces()
        .WithParameter(_buildParent);
        builder.Register<BuildService>(Lifetime.Scoped).AsImplementedInterfaces();

        foreach (var island in _islands)
            builder.RegisterComponent(island);
        builder.RegisterEntryPoint<IslandFactory>(Lifetime.Scoped).AsImplementedInterfaces();

        builder.RegisterEntryPoint<InventoryService>(Lifetime.Scoped).AsImplementedInterfaces();

        builder.RegisterComponent(_highscoreLine);

        builder.RegisterEntryPoint<GameplayEntryPoint>(Lifetime.Scoped);
    }


    /* todo





    */
}
