using System.Collections.Generic;
using UnityEngine;
using Dreamers.Core;

namespace Dreamers.Gameplay.Pooling
{
    /// <summary>
    /// Central manager for all runtime object pools.
    /// Provides simple Spawn and Despawn APIs for obstacles, track segments, collectibles, and VFX.
    /// </summary>
    public class PoolManager : MonoBehaviour, IGameService
    {
        private static PoolManager _instance;
        public static PoolManager Instance => _instance;

        private readonly Dictionary<int, ObjectPool> _prefabPools = new Dictionary<int, ObjectPool>();
        private readonly Dictionary<int, ObjectPool> _instanceToPoolMap = new Dictionary<int, ObjectPool>();

        [SerializeField] private int _defaultInitialCapacity = 10;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            ServiceLocator.Register<PoolManager>(this);
        }

        public void Initialize() { }

        public void Shutdown()
        {
            ClearAll();
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                ServiceLocator.Unregister<PoolManager>();
                _instance = null;
            }
        }

        /// <summary>
        /// Prewarms a pool for a given prefab with a specified capacity.
        /// </summary>
        public void Prewarm(GameObject prefab, int count)
        {
            if (prefab == null) return;

            int key = prefab.GetInstanceID();
            if (!_prefabPools.ContainsKey(key))
            {
                GameObject container = new GameObject($"Pool_{prefab.name}");
                container.transform.SetParent(transform);
                _prefabPools.Add(key, new ObjectPool(prefab, count, container.transform));
            }
        }

        /// <summary>
        /// Spawns an instance of the specified prefab from its pool.
        /// </summary>
        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (prefab == null)
            {
                Debug.LogError("[PoolManager] Attempted to spawn a null prefab.");
                return null;
            }

            int key = prefab.GetInstanceID();
            if (!_prefabPools.TryGetValue(key, out ObjectPool pool))
            {
                GameObject container = new GameObject($"Pool_{prefab.name}");
                container.transform.SetParent(transform);
                pool = new ObjectPool(prefab, _defaultInitialCapacity, container.transform);
                _prefabPools.Add(key, pool);
            }

            GameObject instance = pool.Get(position, rotation, parent);
            if (instance != null)
            {
                int instanceKey = instance.GetInstanceID();
                _instanceToPoolMap[instanceKey] = pool;
            }

            return instance;
        }

        /// <summary>
        /// Generic component-friendly spawn method.
        /// </summary>
        public T Spawn<T>(T prefabComponent, Vector3 position, Quaternion rotation, Transform parent = null) where T : Component
        {
            GameObject go = Spawn(prefabComponent.gameObject, position, rotation, parent);
            return go != null ? go.GetComponent<T>() : null;
        }

        /// <summary>
        /// Returns an active instance back to its originating pool.
        /// </summary>
        public void Despawn(GameObject instance)
        {
            if (instance == null) return;

            int instanceKey = instance.GetInstanceID();
            if (_instanceToPoolMap.TryGetValue(instanceKey, out ObjectPool pool))
            {
                pool.Return(instance);
                _instanceToPoolMap.Remove(instanceKey);
            }
            else
            {
                Debug.LogWarning($"[PoolManager] No pool registered for '{instance.name}'. Destroying object.");
                Destroy(instance);
            }
        }

        /// <summary>
        /// Component-friendly despawn overload.
        /// </summary>
        public void Despawn<T>(T component) where T : Component
        {
            if (component != null)
            {
                Despawn(component.gameObject);
            }
        }

        /// <summary>
        /// Clears all pools and destroys cached objects.
        /// </summary>
        public void ClearAll()
        {
            foreach (var pool in _prefabPools.Values)
            {
                pool.Clear();
            }
            _prefabPools.Clear();
            _instanceToPoolMap.Clear();
        }
    }
}
