using UnityEngine;
using Dreamers.Gameplay.World;

namespace Dreamers.Data
{
    /// <summary>
    /// ScriptableObject defining the Subway Surfers Environment Asset Pack configuration.
    /// Manages track segments, curved tunnel portals, commuter trains with rooftop ramps,
    /// railway signal gantries, and street props (benches, trash bins, billboards).
    /// </summary>
    [CreateAssetMenu(fileName = "SO_Env_SubwaySurfersAssets", menuName = "Dreamers/Data/Subway Surfers Asset Pack")]
    public class SubwayEnvironmentAssetData : ScriptableObject
    {
        [Header("Railway Corridor Identity")]
        [SerializeField] private string _environmentId = "subway_corridor_assets";
        [SerializeField] private string _displayName = "Subway Railway Corridor";

        [Header("Atmosphere & Lighting")]
        [SerializeField] private Color _skyColor = new Color(0.98f, 0.95f, 0.78f); // Golden morning
        [SerializeField] private Color _fogColor = new Color(0.98f, 0.95f, 0.78f);
        [SerializeField] private float _fogDensity = 0.015f;
        [SerializeField] private Color _sunLightColor = new Color(1.0f, 0.93f, 0.83f);
        [SerializeField] private float _sunIntensity = 1.4f;

        [Header("Track & Tunnel Prefabs")]
        [SerializeField] private TrackSegment _standardTrackPrefab;
        [SerializeField] private TrackSegment _stationPlatformTrackPrefab;
        [SerializeField] private TrackSegment _subwayTunnelTrackPrefab;

        [Header("Train & Obstacle Prefabs")]
        [SerializeField] private GameObject _parkedTrainRampPrefab;
        [SerializeField] private GameObject _oncomingMetroTrainPrefab;
        [SerializeField] private GameObject _chevronBufferStopPrefab;
        [SerializeField] private GameObject _lowHurdlePrefab;

        [Header("Railway & Street Props")]
        [SerializeField] private GameObject _catenaryGantryPrefab;
        [SerializeField] private GameObject _railwaySignalLightPrefab;
        [SerializeField] private GameObject _stationBenchPrefab;
        [SerializeField] private GameObject _recyclingBinPrefab;
        [SerializeField] private GameObject _cityDirectionSignPrefab;
        [SerializeField] private GameObject _motivationalPosterPrefab;

        #region Public Getters
        public string EnvironmentId => _environmentId;
        public string DisplayName => _displayName;
        public Color SkyColor => _skyColor;
        public Color FogColor => _fogColor;
        public float FogDensity => _fogDensity;
        public Color SunLightColor => _sunLightColor;
        public float SunIntensity => _sunIntensity;
        public TrackSegment StandardTrackPrefab => _standardTrackPrefab;
        public TrackSegment StationPlatformTrackPrefab => _stationPlatformTrackPrefab;
        public TrackSegment SubwayTunnelTrackPrefab => _subwayTunnelTrackPrefab;
        public GameObject ParkedTrainRampPrefab => _parkedTrainRampPrefab;
        public GameObject OncomingMetroTrainPrefab => _oncomingMetroTrainPrefab;
        public GameObject ChevronBufferStopPrefab => _chevronBufferStopPrefab;
        public GameObject LowHurdlePrefab => _lowHurdlePrefab;
        public GameObject CatenaryGantryPrefab => _catenaryGantryPrefab;
        public GameObject RailwaySignalLightPrefab => _railwaySignalLightPrefab;
        public GameObject StationBenchPrefab => _stationBenchPrefab;
        public GameObject RecyclingBinPrefab => _recyclingBinPrefab;
        #endregion
    }
}
