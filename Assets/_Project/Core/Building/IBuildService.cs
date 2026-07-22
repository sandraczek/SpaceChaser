

using System.Collections.Generic;
using UnityEngine;

namespace SpaceChaser.Core.Building
{
    public interface IBuildService
    {
        public bool Build(BuildData data, Vector2 pos, float rotation);
        public bool BuildStrut(StrutData data, Vector2 pos, float rotation);

        public bool BuildFoundation(FoundationData data, Vector2 pos, float rotation);

        public bool RemoveStrut(Strut strut);

        public bool RemoveBuild(Build build);

        public bool RemoveFoundation(Foundation foundation);
        public bool RemoveResource(Resource resource);
    }
}