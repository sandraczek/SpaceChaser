using System.Collections.Generic;
using SpaceChaser.Core.Registry;
using UnityEngine;

namespace SpaceChaser.Core.Building
{
    [CreateAssetMenu(menuName = "Building/Build")]
    public class BuildData : DataAsset
    {
        public string DisplayName;
        public List<ItemAmount> Recipe;
        public float Mass;
        public Build Prefab;
    }
}