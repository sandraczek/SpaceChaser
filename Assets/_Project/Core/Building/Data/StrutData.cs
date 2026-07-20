using System.Collections.Generic;
using SpaceChaser.Core.Registry;
using UnityEngine;

namespace SpaceChaser.Core.Building
{
    [CreateAssetMenu(menuName = "Building/Strut")]
    public class StrutData : DataAsset
    {
        public string DisplayName;
        public List<ItemAmount> Recipe;

        public Strut Prefab;

        public Sprite Icon;
    }
}