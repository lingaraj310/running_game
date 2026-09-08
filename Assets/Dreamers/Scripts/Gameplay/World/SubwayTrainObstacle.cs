using UnityEngine;
using Dreamers.Gameplay.Player;

namespace Dreamers.Gameplay.World
{
    /// <summary>
    /// Implements Subway Surfers style commuter train obstacle.
    /// Supports stationary parked trains with climbable front ramps,
    /// rooftop running collider surfaces, and oncoming moving trains.
    /// </summary>
    public class SubwayTrainObstacle : MonoBehaviour
    {
        [Header("Train Configuration")]
        [SerializeField] private bool _isOncoming = false;
        [SerializeField] private float _oncomingSpeed = 12.0f;
        [SerializeField] private float _trainLength = 16.0f;
        [SerializeField] private float _roofHeight = 3.2f;

        [Header("Ramp & Roof Surfaces")]
        [SerializeField] private Collider _frontRampCollider;
        [SerializeField] private Collider _roofTopCollider;
        [SerializeField] private Transform _roofCoinsAnchor;

        [Header("Lighting & Visuals")]
        [SerializeField] private Light[] _headlights;
        [SerializeField] private Renderer _graffitiMeshRenderer;

        private void Update()
        {
            if (_isOncoming)
            {
                // Translate oncoming train towards the player along -Z
                transform.Translate(Vector3.back * (_oncomingSpeed * Time.deltaTime), Space.World);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerController>(out PlayerController player))
            {
                // Verify if player entered from the top roof or front obstacle hitbox
                Vector3 relativePos = transform.InverseTransformPoint(player.transform.position);

                if (relativePos.y >= _roofHeight - 0.4f)
                {
                    // Player is safely running along the train rooftop
                }
                else
                {
                    // Player collided with train front or side
                }
            }
        }

        public void SetupTrain(bool oncoming, float speed)
        {
            _isOncoming = oncoming;
            _oncomingSpeed = speed;

            if (_headlights != null)
            {
                foreach (var light in _headlights)
                {
                    if (light != null) light.enabled = oncoming;
                }
            }
        }
    }
}
