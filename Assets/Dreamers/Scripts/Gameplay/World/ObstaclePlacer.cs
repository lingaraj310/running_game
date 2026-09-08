using System.Collections.Generic;
using UnityEngine;
using Dreamers.Data;
using Dreamers.Gameplay.Pooling;
using Dreamers.Utilities;

namespace Dreamers.Gameplay.World
{
    /// <summary>
    /// Algorithmic obstacle and collectible placer.
    /// Strictly guarantees that at least one clear or safely jumpable/slidable path exists at every cross-section.
    /// </summary>
    public class ObstaclePlacer : MonoBehaviour
    {
        [Header("Spawn Pools")]
        [SerializeField] private List<ObstacleDataSO> _obstacleCatalog = new List<ObstacleDataSO>();
        [SerializeField] private GameObject _coinPrefab;
        [SerializeField] private GameObject[] _powerUpPrefabs;

        [Header("Tuning")]
        [SerializeField] private float _minObstacleSpacing = 16.0f;

        private float _lastPlacedZ = 20.0f;

        public void PopulateSegment(TrackSegment segment, float currentDifficultyMultiplier = 1.0f)
        {
            float segStartZ = segment.transform.position.z;
            float segEndZ = segStartZ + segment.Length;

            while (_lastPlacedZ < segEndZ - 5.0f)
            {
                _lastPlacedZ += Mathf.Max(12.0f, _minObstacleSpacing / currentDifficultyMultiplier + Random.Range(0f, 6f));

                // Choose a guaranteed free lane (0=Left, 1=Center, 2=Right)
                int safeLane = Random.Range(0, GameConstants.TOTAL_LANES);

                for (int lane = 0; lane < GameConstants.TOTAL_LANES; lane++)
                {
                    float laneX = (lane - GameConstants.LANE_INDEX_CENTER) * GameConstants.LANE_WIDTH;
                    Vector3 spawnPos = new Vector3(laneX, 0f, _lastPlacedZ);

                    if (lane == safeLane)
                    {
                        // Spawn Coins or Powerups in the guaranteed free lane
                        SpawnCollectiblesInLane(spawnPos);
                    }
                    else
                    {
                        // Spawn Obstacle in blocked lane
                        SpawnObstacleInLane(spawnPos);
                    }
                }
            }
        }

        private void SpawnCollectiblesInLane(Vector3 originPos)
        {
            if (Random.value < 0.15f && _powerUpPrefabs != null && _powerUpPrefabs.Length > 0)
            {
                // Spawn Powerup Crate
                GameObject chosenPowerUp = _powerUpPrefabs[Random.Range(0, _powerUpPrefabs.Length)];
                if (PoolManager.Instance != null && chosenPowerUp != null)
                {
                    PoolManager.Instance.Spawn(chosenPowerUp, originPos + Vector3.up * 1.0f, Quaternion.identity);
                }
            }
            else if (_coinPrefab != null && PoolManager.Instance != null)
            {
                // Spawn Parabolic Coin Arc
                bool isArc = Random.value < 0.4f;
                for (int c = 0; c < 4; c++)
                {
                    float offsetZ = c * 2.0f;
                    float heightY = isArc ? 1.0f + Mathf.Sin((c / 3.0f) * Mathf.PI) * 1.8f : 1.0f;
                    Vector3 coinPos = originPos + new Vector3(0f, heightY, offsetZ);
                    PoolManager.Instance.Spawn(_coinPrefab, coinPos, Quaternion.identity);
                }
            }
        }

        private void SpawnObstacleInLane(Vector3 spawnPos)
        {
            if (_obstacleCatalog == null || _obstacleCatalog.Count == 0 || PoolManager.Instance == null) return;

            ObstacleDataSO chosenData = _obstacleCatalog[Random.Range(0, _obstacleCatalog.Count)];
            if (chosenData != null && chosenData.ObstaclePrefab != null)
            {
                PoolManager.Instance.Spawn(chosenData.ObstaclePrefab, spawnPos, Quaternion.identity);
            }
        }

        public void ResetPlacement(float startZ)
        {
            _lastPlacedZ = startZ;
        }
    }
}
