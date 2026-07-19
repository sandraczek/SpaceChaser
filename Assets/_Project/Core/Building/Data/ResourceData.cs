using System.Collections.Generic;
using SpaceChaser.Core.Registry;
using UnityEngine;

namespace SpaceChaser.Core.Building
{
    [CreateAssetMenu(menuName = "Building/Resource")]
    public class ResourceData : DataAsset
    {
        public string DisplayName;
        public List<ItemAmount> Recipe;
        public Resource Prefab;
    }
}