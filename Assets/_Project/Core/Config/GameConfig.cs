using UnityEngine;

namespace SpaceChaser.Core
{
    [CreateAssetMenu(menuName = "Game/Config")]
    public class GameConfig : ScriptableObject
    {
        [Header("Building")]
        public float SalvageRate;
        public int InventorySlots;

        [Header("Islands")]
        public float AverageDistance;
        public float DistanceFlactuation;

        [Header("Camera")]
        public Vector2 MaxBounds;
        public Vector2 MinBounds;
    }
}