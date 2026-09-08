using UnityEngine;

namespace Dreamers.Gameplay.World
{
    /// <summary>
    /// Overhead Catenary Truss Gantry with suspended railway signal boxes,
    /// power line insulator drop cables, and animated switching signal lights.
    /// </summary>
    public class RailwaySignalGantry : MonoBehaviour
    {
        [Header("Truss Components")]
        [SerializeField] private Transform _leftColumn;
        [SerializeField] private Transform _rightColumn;
        [SerializeField] private Transform _crossbarTruss;

        [Header("Signal Light Boxes")]
        [SerializeField] private Renderer[] _signalLightLenses;
        [SerializeField] private Light[] _signalPointLights;

        private float _switchTimer = 0f;

        private void Update()
        {
            _switchTimer += Time.deltaTime;
            if (_switchTimer > 4.0f)
            {
                _switchTimer = 0f;
                ToggleSignals();
            }
        }

        private void ToggleSignals()
        {
            if (_signalPointLights == null) return;

            foreach (var light in _signalPointLights)
            {
                if (light != null)
                {
                    light.color = (Random.value > 0.5f) ? Color.green : Color.red;
                }
            }
        }
    }
}
