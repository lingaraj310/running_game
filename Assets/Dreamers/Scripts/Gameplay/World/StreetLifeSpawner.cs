using System.Collections.Generic;
using UnityEngine;

namespace Dreamers.Gameplay.World
{
    /// <summary>
    /// Spawns ambient real-world environmental elements:
    /// - Scattering pigeons that take flight on player approach
    /// - Construction zones with blinking arrow signs and steam vents
    /// - Roadside traffic and parked delivery vans
    /// </summary>
    public class StreetLifeSpawner : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject _pigeonFlockPrefab;
        [SerializeField] private GameObject _steamVentPrefab;
        [SerializeField] private GameObject _constructionSignPrefab;

        [Header("Detection")]
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private float _scatterDistance = 8.0f;

        private readonly List<GameObject> _activePigeons = new List<GameObject>();

        public void SpawnPigeonsAt(Vector3 position)
        {
            if (_pigeonFlockPrefab == null) return;
            GameObject flock = Instantiate(_pigeonFlockPrefab, position, Quaternion.identity);
            _activePigeons.Add(flock);
        }

        private void Update()
        {
            if (_playerTransform == null || _activePigeons.Count == 0) return;

            for (int i = _activePigeons.Count - 1; i >= 0; i--)
            {
                GameObject flock = _activePigeons[i];
                if (flock == null) continue;

                float dist = Vector3.Distance(_playerTransform.position, flock.transform.position);
                if (dist < _scatterDistance)
                {
                    // Scatter birds upward and away
                    flock.transform.position += (Vector3.up * 6f + Vector3.forward * 4f) * Time.deltaTime;
                    if (flock.transform.position.y > 15f)
                    {
                        Destroy(flock);
                        _activePigeons.RemoveAt(i);
                    }
                }
            }
        }
    }
}
