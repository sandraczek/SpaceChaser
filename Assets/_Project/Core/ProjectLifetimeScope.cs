using VContainer;
using VContainer.Unity;
using UnityEngine;
using SpaceChaser.Core.Inputs;

namespace SpaceChaser.Core
{
    public sealed class ProjectLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private InputReader _inputReader;


        protected override void Configure(IContainerBuilder builder)
        {
            _inputReader.Initialize();
            builder.RegisterInstance<IInputReader>(_inputReader);

            builder.RegisterInstance(_gameConfig);
        }
    }
}
