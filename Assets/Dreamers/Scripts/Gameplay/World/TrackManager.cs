using System.Collections.Generic;
using UnityEngine;
using Dreamers.Gameplay.Pooling;
using Dreamers.Gameplay.Player;

namespace Dreamers.Gameplay.World
{
    /// <summary>
    /// Spawns, anchors, translates, and recycles continuous 3D track chunks ahead of the running player.
    /// Integrates with PoolManager to ensure zero runtime GC allocations.
    /// </summary>
    public class TrackManager : MonoBehaviour
    {
        [Header("Track Prefabs")]
        [SerializeField] private TrackSegment[] _trackSegmentPrefabs;
        [SerializeField] private int _activeSegmentsCount = 7;
        [SerializeField] private ObstaclePlacer _obstaclePlacer;

        [Header("Target Runner")]
        [SerializeField] private PlayerController _player;

        private readonly Queue<TrackSegment> _activeTrackQueue = new Queue<TrackSegment>();
        private float _nextSpawnZ = 0f;

        private void Start()
        {
            if (_player == null)
            {
                _player = FindFirstObjectByType<PlayerController>();
            }

            InitializeTrack();
        }

        public void InitializeTrack()
        {
            ClearTrack();
            _nextSpawnZ = 0f;

            if (_obstaclePlacer != null)
            {
                _obstaclePlacer.ResetPlacement(25.0f);
            }

            for (int i = 0; i < _activeSegmentsCount; i++)
            {
                SpawnNextSegment();
            }
        }

        private void Update()
        {
            if (_player == null || _activeTrackQueue.Count == 0) return;

            // When player passes the first segment, recycle it to the front
            TrackSegment oldestSegment = _activeTrackQueue.Peek();
            if (_player.transform.position.z > oldestSegment.transform.position.z + oldestSegment.Length + 10.0f)
            {
                RecycleOldestSegment();
                SpawnNextSegment();
            }
        }

        private void SpawnNextSegment()
        {
            if (_trackSegmentPrefabs == null || _trackSegmentPrefabs.Length == 0) return;

            TrackSegment chosenPrefab = _trackSegmentPrefabs[Random.Range(0, _trackSegmentPrefabs.Length)];
            Vector3 spawnPosition = new Vector3(0f, 0f, _nextSpawnZ);

            TrackSegment segmentInstance;
            if (PoolManager.Instance != null)
            {
                segmentInstance = PoolManager.Instance.Spawn(chosenPrefab, spawnPosition, Quaternion.identity, transform);
            }
            else
            {
                segmentInstance = Instantiate(chosenPrefab, spawnPosition, Quaternion.identity, transform);
            }

            _nextSpawnZ += segmentInstance.Length;
            _activeTrackQueue.Enqueue(segmentInstance);

            if (_obstaclePlacer != null)
            {
                _obstaclePlacer.PopulateSegment(segmentInstance);
            }
        }

        private void RecycleOldestSegment()
        {
            if (_activeTrackQueue.Count == 0) return;

            TrackSegment oldest = _activeTrackQueue.Dequeue();
            if (PoolManager.Instance != null)
            {
                PoolManager.Instance.Despawn(oldest);
            }
            else
            {
                Destroy(oldest.gameObject);
            }
        }

        public void ClearTrack()
        {
            while (_activeTrackQueue.Count > 0)
            {
                RecycleOldestSegment();
            }
        }
    }
}
