
using System.Collections.Generic;
using UnityEditor.Search;

namespace SpaceChaser.Core.Registry
{
    public interface IAssetRegistry<T> where T : DataAsset
    {
        T Get(AssetId id);
        IReadOnlyList<T> Assets { get; }
    }
}