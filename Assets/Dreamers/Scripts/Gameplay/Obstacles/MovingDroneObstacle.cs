using UnityEngine;

namespace Dreamers.Gameplay.Obstacles
{
    /// <summary>
    /// Moving security drone obstacle that patrols laterally across lanes or hovers with vertical oscillation.
    /// Can be slid under when hovering high, or dodged across lanes.
    /// </summary>
    public class MovingDroneObstacle : MonoBehaviour
    {
        [Header("Patrol Motion")]
        [SerializeField] private float _patrolWidth = 2.2f;
        [SerializeField] private float _patrolSpeed = 2.5f;
        [SerializeField] private bool _oscillateVertically = false;

        [Header("Visual Parts")]
        [SerializeField] private Transform _rotorLeft;
        [SerializeField] private Transform _rotorRight;
        [SerializeField] private Light _searchlight;

        private float _originX;
        private float _originY;
        private float _timeOffset;

        private void Awake()
        {
            _originX = transform.position.x;
            _originY = transform.position.y;
            _timeOffset = Random.Range(0f, 10f);
        }

        private void Update()
        {
            float t = Time.time * _patrolSpeed + _timeOffset;

            // Lateral sweep across lanes
            float targetX = _originX + Mathf.Sin(t) * _patrolWidth;
            float targetY = _originY + (_oscillateVertically ? Mathf.Sin(t * 2f) * 0.3f : 0f);

            transform.position = new Vector3(targetX, targetY, transform.position.z);

            // Spin rotors
            float rotorSpin = Time.deltaTime * 1200f;
            if (_rotorLeft != null) _rotorLeft.Rotate(Vector3.up, rotorSpin);
            if (_rotorRight != null) _rotorRight.Rotate(Vector3.up, -rotorSpin);
        }
    }
}
