using UnityEngine;

namespace SpaceChaser.Core
{
    [CreateAssetMenu(menuName = "Game/Config")]
    public class GameConfig : ScriptableObject
    {

        public Vector2 SpawnPoint;
        public float FloorY;

        [Header("Controls")]
        public float RotationPerSecond;

        [Header("Building")]
        public float SalvageRate;
        public int BuildingSlots;
        public int StrutSlots;
        public int FoundationSlots;
        public float BuildingTime;
        public float SalvagingTime;
        public float BuildingDistance;

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

        [Header("Death")]
        public float DeathDistance;
    }
}