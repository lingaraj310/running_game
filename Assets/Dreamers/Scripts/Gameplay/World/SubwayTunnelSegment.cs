using UnityEngine;

namespace Dreamers.Gameplay.World
{
    /// <summary>
    /// Subway Tunnel segment with curved brick/concrete portal arches,
    /// caution yellow/black hazard striping, and 3-aspect railway signal lights.
    /// </summary>
    public class SubwayTunnelSegment : TrackSegment
    {
        [Header("Tunnel Arch Architecture")]
        [SerializeField] private GameObject _portalArchMesh;
        [SerializeField] private GameObject _tunnelInteriorTube;

        [Header("Railway Signal Lanterns")]
        [SerializeField] private Light[] _signalLights;
        [SerializeField] private Color _greenSignalColor = new Color(0.1f, 0.9f, 0.2f);
        [SerializeField] private Color _redSignalColor = new Color(0.95f, 0.1f, 0.1f);

        private void Start()
        {
            ApplySignalStates();
        }

        public void ApplySignalStates()
        {
            if (_signalLights == null) return;

            for (int i = 0; i < _signalLights.Length; i++)
            {
                if (_signalLights[i] != null)
                {
                    // Green light for clear lane, red light for blocked lane
                    bool isClear = Random.value > 0.4f;
                    _signalLights[i].color = isClear ? _greenSignalColor : _redSignalColor;
                }
            }
        }
    }
}
