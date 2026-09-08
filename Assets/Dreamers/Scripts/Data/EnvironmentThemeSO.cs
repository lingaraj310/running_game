using UnityEngine;
using Dreamers.Gameplay.World;

namespace Dreamers.Data
{
    /// <summary>
    /// Configuration for 3D environment biomes (Campus, Cyber City, Quantum Server Farm).
    /// Defines skybox color, lighting palettes, track segment prefabs, and atmospheric fog.
    /// </summary>
    [CreateAssetMenu(fileName = "SO_Env_NewEnvironment", menuName = "Dreamers/Data/Environment Theme")]
    public class EnvironmentThemeSO : ScriptableObject
    {
        [Header("Environment Identity")]
        [SerializeField] private string _themeId;
        [SerializeField] private string _displayName;

        [Header("Atmosphere & Lighting")]
        [SerializeField] private Color _skyColor = new Color(0.04f, 0.06f, 0.12f);
        [SerializeField] private Color _fogColor = new Color(0.04f, 0.06f, 0.12f);
        [SerializeField] private float _fogDensity = 0.02f;
        [SerializeField] private Color _ambientLightColor = new Color(0.8f, 0.9f, 1.0f);
        [SerializeField] private Color _neonGlowColor = new Color(0.0f, 0.82f, 1.0f);

        [Header("Segment & Prop Prefabs")]
        [SerializeField] private TrackSegment[] _trackSegments;
        [SerializeField] private GameObject[] _backgroundSceneryPrefabs;
        [SerializeField] private GameObject[] _billboardPrefabs;

        #region Public Getters
        public string ThemeId => _themeId;
        public string DisplayName => _displayName;
        public Color SkyColor => _skyColor;
        public Color FogColor => _fogColor;
        public float FogDensity => _fogDensity;
        public Color AmbientLightColor => _ambientLightColor;
        public Color NeonGlowColor => _neonGlowColor;
        public TrackSegment[] TrackSegments => _trackSegments;
        public GameObject[] BackgroundSceneryPrefabs => _backgroundSceneryPrefabs;
        #endregion
    }
}
