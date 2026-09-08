using UnityEngine;
using Dreamers.Utilities;

namespace Dreamers.Camera
{
    /// <summary>
    /// Smooth follow camera tailored for mobile portrait endless runners.
    /// Locks forward Z tracking to the player while smoothly damping lateral X lane switches and vertical Y jumps.
    /// </summary>
    public class CameraFollower : MonoBehaviour
    {
        [Header("Target & Offsets")]
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _offset = new Vector3(0f, 4.5f, -6.5f);
        [SerializeField] private float _lookAtHeightOffset = 1.5f;

        [Header("Smooth Damping")]
        [SerializeField] private float _smoothSpeedX = 12.0f;
        [SerializeField] private float _smoothSpeedY = 6.0f;

        private Vector3 _currentVelocity;

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private void LateUpdate()
        {
            if (_target == null) return;

            Vector3 targetPos = _target.position;

            // Z is locked tightly with the forward runner, X and Y interpolate smoothly
            float targetX = Mathf.Lerp(transform.position.x, targetPos.x + _offset.x, Time.deltaTime * _smoothSpeedX);
            float targetY = Mathf.Lerp(transform.position.y, targetPos.y + _offset.y, Time.deltaTime * _smoothSpeedY);
            float targetZ = targetPos.z + _offset.z;

            transform.position = new Vector3(targetX, targetY, targetZ);

            // Look slightly ahead of the character
            Vector3 lookTarget = targetPos + Vector3.up * _lookAtHeightOffset;
            transform.LookAt(lookTarget);
        }
    }
}
