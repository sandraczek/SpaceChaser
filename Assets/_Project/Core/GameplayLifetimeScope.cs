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
using UnityEditor;
using SpaceChaser.Core.Death;
using SpaceChaser.Core.HUD;

public class GameplayLifetimeScope : LifetimeScope
{
    [SerializeField] private BuildRegistry _buildRegistry;
    [SerializeField] private StrutRegistry _strutRegistry;
    [SerializeField] private FoundationRegistry _foundationRegistry;
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _poolParent;
    [SerializeField] private BuildPreview _buildPreview;
    [SerializeField] private List<Island> _islands;
    [SerializeField] private HighscoreView _highscoreLine;
    [SerializeField] private CameraFollow _camera;
    [SerializeField] private IslandMarker _islandMarker;
    [SerializeField] private InventoryView _inventoryView;
    protected override void Configure(IContainerBuilder builder)
    {
#if UNITY_EDITOR
        FindAllIslands();
        foreach (var island in _islands)
        {
            island.FindResources();
        }
        _buildRegistry.Editor_FindAllAssets();
        _strutRegistry.Editor_FindAllAssets();
        _foundationRegistry.Editor_FindAllAssets();
#endif
        builder.RegisterInstance(_playerConfig);

        _buildRegistry.Initialize();
        builder.RegisterInstance(_buildRegistry).As<IAssetRegistry<BuildData>>();
        _strutRegistry.Initialize();
        builder.RegisterInstance(_strutRegistry).As<IAssetRegistry<StrutData>>();
        _foundationRegistry.Initialize();
        builder.RegisterInstance(_foundationRegistry).As<IAssetRegistry<FoundationData>>();

        builder.RegisterComponent(_buildPreview);

        builder.Register<PlayerProvider>(Lifetime.Scoped)
        .AsImplementedInterfaces()
        .WithParameter(_playerPrefab);

        builder.RegisterComponent(_camera).AsImplementedInterfaces();

        builder.RegisterEntryPoint<BuildFactory>(Lifetime.Scoped)
        .AsImplementedInterfaces()
        .WithParameter(_poolParent);
        builder.RegisterEntryPoint<StrutFactory>(Lifetime.Scoped)
        .AsImplementedInterfaces()
        .WithParameter(_poolParent);
        builder.RegisterEntryPoint<FoundationFactory>(Lifetime.Scoped)
        .AsImplementedInterfaces()
        .WithParameter(_poolParent);
        builder.Register<BuildService>(Lifetime.Scoped).AsImplementedInterfaces();

        builder.RegisterInstance(_islands).As<IReadOnlyList<Island>>();
        builder.RegisterEntryPoint<IslandFactory>(Lifetime.Scoped)
        .AsImplementedInterfaces()
        .WithParameter(_poolParent);
        builder.RegisterEntryPoint<IslandService>(Lifetime.Scoped).AsImplementedInterfaces();

        builder.RegisterEntryPoint<InventoryService>(Lifetime.Scoped).AsImplementedInterfaces();

        builder.RegisterEntryPoint<DeathService>(Lifetime.Scoped).AsImplementedInterfaces();

        builder.RegisterComponent(_highscoreLine);
        builder.RegisterComponent(_islandMarker);
        builder.RegisterComponent(_inventoryView).AsSelf();
        // builder.RegisterBuildCallback(resolver =>
        // {
        //     resolver.InjectGameObject(_inventoryView.gameObject);
        // });


        builder.RegisterEntryPoint<GameplayEntryPoint>(Lifetime.Scoped);


    }


    /* todo

        -- features --
        - make islands go-through from the bottom
        - maybe resources from death ??
        - maybe death screen? (i think not)
        - world boundaries for player
        - ui for displaying resources
        - ui for builds (f.e. cost)
        - loading screen
        - main menu
        - settings menu
        - tutorial

        -- bugs --
        ~ foundation removing -> bfs

        -- game feel --
        - arrow could not jump with player
        - maybe build check for collisions with player
        - render layers (f.e. foundation with floot)
        - maybe fix the way player gets stuck in the floor after falling? not really important


    */


    [ContextMenu("Find all islands")]
    public void FindAllIslands()
    {
        _islands ??= new();

        _islands.Clear();

        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");

        foreach (string guid in prefabGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            if (prefab != null && prefab.TryGetComponent(out Island island))
            {
                _islands.Add(island);
            }
        }
    }
}
