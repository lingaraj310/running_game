using System;
using System.Collections.Generic;
using UnityEngine;
using Dreamers.Core;
using Dreamers.Data;

namespace Dreamers.Gameplay.PowerUps
{
    public enum PowerUpType
    {
        Magnet,
        Jetpack,
        ScoreMultiplier,
        SuperSneakers
    }

    /// <summary>
    /// Coordinates active power-up timers, effects, and visual/audio notifications during gameplay.
    /// </summary>
    public class PowerUpManager : MonoBehaviour, IGameService
    {
        private static PowerUpManager _instance;
        public static PowerUpManager Instance => _instance;

        private readonly Dictionary<PowerUpType, float> _activePowerUpTimers = new Dictionary<PowerUpType, float>();

        public event Action<PowerUpType, float> OnPowerUpActivated;
        public event Action<PowerUpType> OnPowerUpExpired;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            ServiceLocator.Register<PowerUpManager>(this);
        }

        public void Initialize()
        {
            _activePowerUpTimers.Clear();
        }

        public void Shutdown()
        {
            _activePowerUpTimers.Clear();
        }

        public void ActivatePowerUp(PowerUpType type, float duration)
        {
            _activePowerUpTimers[type] = duration;
            OnPowerUpActivated?.Invoke(type, duration);
        }

        public bool IsPowerUpActive(PowerUpType type)
        {
            return _activePowerUpTimers.TryGetValue(type, out float timer) && timer > 0f;
        }

        public float GetRemainingTime(PowerUpType type)
        {
            return _activePowerUpTimers.TryGetValue(type, out float timer) ? timer : 0f;
        }

        private void Update()
        {
            if (_activePowerUpTimers.Count == 0) return;

            float dt = Time.deltaTime;
            List<PowerUpType> expiredKeys = null;

            foreach (var key in _activePowerUpTimers.Keys)
            {
                _activePowerUpTimers[key] -= dt;
                if (_activePowerUpTimers[key] <= 0f)
                {
                    expiredKeys ??= new List<PowerUpType>();
                    expiredKeys.Add(key);
                }
            }

            if (expiredKeys != null)
            {
                for (int i = 0; i < expiredKeys.Count; i++)
                {
                    PowerUpType key = expiredKeys[i];
                    _activePowerUpTimers.Remove(key);
                    OnPowerUpExpired?.Invoke(key);
                }
            }
        }
    }
}
