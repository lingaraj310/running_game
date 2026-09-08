using System.Collections.Generic;
using UnityEngine;

namespace Dreamers.Gameplay.Pooling
{
    /// <summary>
    /// High-performance, zero-allocation stack pool for a specific GameObject prefab.
    /// Eliminates runtime Instantiate/Destroy spikes on Android.
    /// </summary>
    public class ObjectPool
    {
        private readonly GameObject _prefab;
        private readonly Transform _parentContainer;
        private readonly Stack<GameObject> _inactiveObjects;
        private readonly bool _canExpand;

        public int TotalCreated { get; private set; }
        public int InactiveCount => _inactiveObjects.Count;

        public ObjectPool(GameObject prefab, int initialCapacity, Transform parentContainer = null, bool canExpand = true)
        {
            _prefab = prefab;
            _parentContainer = parentContainer;
            _canExpand = canExpand;
            _inactiveObjects = new Stack<GameObject>(initialCapacity);

            Prewarm(initialCapacity);
        }

        private void Prewarm(int count)
        {
            for (int i = 0; i < count; i++)
            {
                CreateNewInstance();
            }
        }

        private GameObject CreateNewInstance()
        {
            GameObject instance = Object.Instantiate(_prefab, _parentContainer);
            instance.SetActive(false);
            _inactiveObjects.Push(instance);
            TotalCreated++;
            return instance;
        }

        /// <summary>
        /// Retrieves an instance from the pool, activates it, and notifies IPoolable listeners.
        /// </summary>
        public GameObject Get(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            GameObject instance;

            if (_inactiveObjects.Count > 0)
            {
                instance = _inactiveObjects.Pop();
            }
            else if (_canExpand)
            {
                instance = Object.Instantiate(_prefab, _parentContainer);
                TotalCreated++;
            }
            else
            {
                Debug.LogWarning($"[ObjectPool] Pool for '{_prefab.name}' exhausted and cannot expand.");
                return null;
            }

            Transform t = instance.transform;
            t.SetParent(parent != null ? parent : _parentContainer);
            t.SetPositionAndRotation(position, rotation);
            instance.SetActive(true);

            IPoolable[] poolables = instance.GetComponentsInChildren<IPoolable>(true);
            for (int i = 0; i < poolables.Length; i++)
            {
                poolables[i].OnSpawnFromPool();
            }

            return instance;
        }

        /// <summary>
        /// Returns an instance to the pool, deactivates it, and notifies IPoolable listeners.
        /// </summary>
        public void Return(GameObject instance)
        {
            if (instance == null) return;

            IPoolable[] poolables = instance.GetComponentsInChildren<IPoolable>(true);
            for (int i = 0; i < poolables.Length; i++)
            {
                poolables[i].OnReturnToPool();
            }

            instance.SetActive(false);
            if (_parentContainer != null)
            {
                instance.transform.SetParent(_parentContainer);
            }

            _inactiveObjects.Push(instance);
        }

        /// <summary>
        /// Destroys all pooled instances and clears the stack.
        /// </summary>
        public void Clear()
        {
            while (_inactiveObjects.Count > 0)
            {
                GameObject obj = _inactiveObjects.Pop();
                if (obj != null)
                {
                    Object.Destroy(obj);
                }
            }
            TotalCreated = 0;
        }
    }
}
