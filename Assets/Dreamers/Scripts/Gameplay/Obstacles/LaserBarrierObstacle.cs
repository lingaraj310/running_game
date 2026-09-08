using UnityEngine;

namespace Dreamers.Gameplay.Obstacles
{
    /// <summary>
    /// Cyber laser barrier hazard.
    /// Emits a glowing laser beam that player must slide underneath.
    /// </summary>
    public class LaserBarrierObstacle : MonoBehaviour
    {
        [Header("Laser Beam Visuals")]
        [SerializeField] private LineRenderer _laserLine;
        [SerializeField] private Light _laserGlowLight;
        [SerializeField] private ParticleSystem _sparkParticles;

        [Header("Beam Dimensions")]
        [SerializeField] private float _beamHeight = 1.6f;
        [SerializeField] private float _beamWidth = 2.2f;

        private void Update()
        {
            // Subtle laser flicker
            if (_laserGlowLight != null)
            {
                _laserGlowLight.intensity = 1.5f + Mathf.PingPong(Time.time * 8f, 0.5f);
            }
        }
    }
}
