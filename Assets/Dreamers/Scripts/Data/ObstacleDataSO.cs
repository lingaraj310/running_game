using UnityEngine;

namespace Dreamers.Data
{
    public enum ObstacleClearanceType
    {
        JumpOver,       // Low hurdle: Must jump over
        SlideUnder,     // High barrier: Must slide under
        DodgeLane,      // Full height block: Must switch lanes
        ImpassableWall  // Multi-lane or hazard
    }

    public enum ObstacleDamageType
    {
        Lethal,         // Immediately ends run
        StumbleSlowdown // Slows player down / removes multiplier
    }

    /// <summary>
    /// Static configuration for hazards and hurdles in Dreamers.
    /// </summary>
    [CreateAssetMenu(fileName = "SO_Obs_NewObstacle", menuName = "Dreamers/Data/Obstacle Data")]
    public class ObstacleDataSO : ScriptableObject
    {
        [Header("Obstacle Identity")]
        [SerializeField] private string _obstacleId;
        [SerializeField] private string _obstacleName;

        [Header("Clearance & Mechanics")]
        [SerializeField] private ObstacleClearanceType _clearanceType = ObstacleClearanceType.DodgeLane;
        [SerializeField] private ObstacleDamageType _damageType = ObstacleDamageType.Lethal;
        [Range(1, 3)]
        [SerializeField] private int _laneWidthOccupied = 1;

        [Header("Visual Reference")]
        [SerializeField] private GameObject _obstaclePrefab;

        #region Public Getters
        public string ObstacleId => _obstacleId;
        public string ObstacleName => _obstacleName;
        public ObstacleClearanceType ClearanceType => _clearanceType;
        public ObstacleDamageType DamageType => _damageType;
        public int LaneWidthOccupied => _laneWidthOccupied;
        public GameObject ObstaclePrefab => _obstaclePrefab;
        #endregion
    }
}
