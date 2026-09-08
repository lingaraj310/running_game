using UnityEngine;
using Dreamers.Gameplay.Pooling;

namespace Dreamers.Gameplay.World
{
    /// <summary>
    /// Component attached to modular track segment prefabs.
    /// Manages track length, connection sockets, and recycling lifecycle.
    /// </summary>
    public class TrackSegment : MonoBehaviour, IPoolable
    {
        [Header("Segment Dimensions")]
        [SerializeField] private float _length = 30.0f;
        [SerializeField] private Transform _endConnectionPoint;

        [Header("Spawn Sockets")]
        [SerializeField] private Transform[] _laneLeftSockets;
        [SerializeField] private Transform[] _laneCenterSockets;
        [SerializeField] private Transform[] _laneRightSockets;

        public float Length => _length;
        public Vector3 EndPosition => _endConnectionPoint != null ? _endConnectionPoint.position : transform.position + Vector3.forward * _length;

        public void OnSpawnFromPool()
        {
            // Reset any temporary dynamic props
        }

        public void OnReturnToPool()
        {
            // Clear spawned attachments
        }
    }
}
