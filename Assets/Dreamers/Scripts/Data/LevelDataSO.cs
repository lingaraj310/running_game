using System.Collections.Generic;
using UnityEngine;

namespace Dreamers.Data
{
    public enum GameMode
    {
        Story,
        Endless,
        Challenge
    }

    /// <summary>
    /// Static configuration for a specific level or challenge stage in Dreamers.
    /// Defines objectives, environment theme, difficulty multipliers, and obstacle pools.
    /// </summary>
    [CreateAssetMenu(fileName = "SO_Level_NewLevel", menuName = "Dreamers/Data/Level Data")]
    public class LevelDataSO : ScriptableObject
    {
        [Header("Level Identification")]
        [SerializeField] private string _levelId;
        [SerializeField] private string _levelTitle;
        [SerializeField] private string _associatedCharacterId;
        [SerializeField] private int _chapterNumber = 1;
        [SerializeField] private int _levelNumber = 1;
        [SerializeField] private GameMode _gameMode = GameMode.Story;

        [Header("Objective Targets (Story & Challenge Mode)")]
        [SerializeField] private float _targetDistanceMeters = 1000f;
        [SerializeField] private int _targetCoins = 100;
        [SerializeField] private int _targetScore = 5000;

        [Header("Environment & Obstacle Pools")]
        [SerializeField] private string _environmentThemeName = "CampusCourtyard";
        [SerializeField] private GameObject _environmentThemePrefab;
        [SerializeField] private List<ObstacleDataSO> _allowedObstacles = new List<ObstacleDataSO>();

        [Header("Difficulty Multipliers")]
        [SerializeField] private float _speedMultiplier = 1.0f;
        [SerializeField] private float _obstacleSpawnRateMultiplier = 1.0f;

        [Header("Rewards & Progression")]
        [SerializeField] private int _coinReward = 250;
        [SerializeField] private int _dreamProgressPoints = 10;

        #region Public Getters
        public string LevelId => _levelId;
        public string LevelTitle => _levelTitle;
        public string AssociatedCharacterId => _associatedCharacterId;
        public int ChapterNumber => _chapterNumber;
        public int LevelNumber => _levelNumber;
        public GameMode Mode => _gameMode;

        public float TargetDistanceMeters => _targetDistanceMeters;
        public int TargetCoins => _targetCoins;
        public int TargetScore => _targetScore;

        public string EnvironmentThemeName => _environmentThemeName;
        public GameObject EnvironmentThemePrefab => _environmentThemePrefab;
        public IReadOnlyList<ObstacleDataSO> AllowedObstacles => _allowedObstacles;

        public float SpeedMultiplier => _speedMultiplier;
        public float ObstacleSpawnRateMultiplier => _obstacleSpawnRateMultiplier;

        public int CoinReward => _coinReward;
        public int DreamProgressPoints => _dreamProgressPoints;
        #endregion
    }
}
