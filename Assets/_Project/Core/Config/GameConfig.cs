using UnityEngine;

namespace SpaceChaser.Core
{
    [CreateAssetMenu(menuName = "Game/Config")]
    public class GameConfig : ScriptableObject
    {

        public float FloorY;

        [Header("Building")]
        public float SalvageRate;
        public int InventorySlots;

        [Header("Islands")]
        public float AverageDistance;
        public float DistanceFlactuation;

        public float IslandMaxXFlactuation;
        public float IslandMinX;
        public float IslandMaxX;
        public float MarkerMaxDistance;
        public float MarkerMaxPlayerDistance;

        [Header("Camera")]
        public Vector2 MaxBounds;
        public Vector2 MinBounds;
    }
}