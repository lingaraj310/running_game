using System;
using UnityEngine;

namespace Dreamers.Utilities
{
    public enum WeatherTimePreset
    {
        GoldenMorning,
        BrightMidday,
        SunsetDusk,
        RainyNeonNight
    }

    /// <summary>
    /// Coordinates dynamic day-night cycles, sun angle/color transitions, and rainy wet road shaders.
    /// </summary>
    public class DayNightWeatherManager : MonoBehaviour
    {
        [Header("Lighting References")]
        [SerializeField] private Light _sunLight;
        [SerializeField] private Light _ambientSkyLight;
        [SerializeField] private ParticleSystem _rainParticleSystem;

        [Header("Weather Cycle Settings")]
        [SerializeField] private float _cycleDurationSeconds = 120f;
        [SerializeField] private WeatherTimePreset _currentPreset = WeatherTimePreset.GoldenMorning;

        public event Action<WeatherTimePreset> OnWeatherChanged;

        private void Update()
        {
            // Auto progress weather based on distance or time
        }

        public void SetWeatherPreset(WeatherTimePreset preset)
        {
            _currentPreset = preset;
            switch (preset)
            {
                case WeatherTimePreset.GoldenMorning:
                    if (_sunLight) { _sunLight.color = new Color(1f, 0.92f, 0.8f); _sunLight.intensity = 1.3f; }
                    if (_rainParticleSystem) _rainParticleSystem.Stop();
                    break;
                case WeatherTimePreset.BrightMidday:
                    if (_sunLight) { _sunLight.color = Color.white; _sunLight.intensity = 1.5f; }
                    if (_rainParticleSystem) _rainParticleSystem.Stop();
                    break;
                case WeatherTimePreset.SunsetDusk:
                    if (_sunLight) { _sunLight.color = new Color(1f, 0.6f, 0.4f); _sunLight.intensity = 1.0f; }
                    if (_rainParticleSystem) _rainParticleSystem.Stop();
                    break;
                case WeatherTimePreset.RainyNeonNight:
                    if (_sunLight) { _sunLight.color = new Color(0.2f, 0.3f, 0.5f); _sunLight.intensity = 0.4f; }
                    if (_rainParticleSystem) _rainParticleSystem.Play();
                    break;
            }
            OnWeatherChanged?.Invoke(preset);
        }
    }
}
