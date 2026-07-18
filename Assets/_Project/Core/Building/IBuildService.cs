

using UnityEngine;

namespace SpaceChaser.Core.Building
{
    public interface IBuildService
    {
        void Build(Vector2 pos, BuildData data);
    }
}