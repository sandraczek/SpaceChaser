using System.Collections.Generic;
using SpaceChaser.Core.Registry;
using UnityEngine;

namespace SpaceChaser.Core.Building
{
    [CreateAssetMenu(menuName = "Building/Foundation")]
    public class FoundationData : DataAsset
    {
        public string DisplayName;
        public List<ItemAmount> Recipe;

        public Foundation Prefab;

        public Sprite Icon;
    }
}