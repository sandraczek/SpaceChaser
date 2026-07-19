using System.Collections.Generic;
using SpaceChaser.Core.Registry;
using UnityEngine;

namespace SpaceChaser.Core.Building
{
    [CreateAssetMenu(menuName = "Islands/Resource")]
    public class ResourceData : ScriptableObject
    {
        public string DisplayName;
        public List<ItemAmount> Resources;
        public Resource Prefab;
    }
}