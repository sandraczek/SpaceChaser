using SpaceChaser.Core.Registry;
using UnityEngine;

namespace SpaceChaser.Core.Building
{
    public class BuildService : IBuildService
    {
        private IBuildFactory _factory;
        public BuildService(IBuildFactory factory)
        {
            _factory = factory;
        }

        public void Build(Vector2 pos, BuildData data)
        {
            _factory.Create(data, pos);
        }
    }
}