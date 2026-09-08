using UnityEngine;

namespace Dreamers.Utilities
{
    /// <summary>
    /// Reusable mathematical and vector extension methods for smooth runner interpolation and bounds calculations.
    /// </summary>
    public static class MathExtensions
    {
        /// <summary>
        /// Returns a new Vector3 with the X component modified.
        /// </summary>
        public static Vector3 WithX(this Vector3 v, float x) => new Vector3(x, v.y, v.z);

        /// <summary>
        /// Returns a new Vector3 with the Y component modified.
        /// </summary>
        public static Vector3 WithY(this Vector3 v, float y) => new Vector3(v.x, y, v.z);

        /// <summary>
        /// Returns a new Vector3 with the Z component modified.
        /// </summary>
        public static Vector3 WithZ(this Vector3 v, float z) => new Vector3(v.x, v.y, z);

        /// <summary>
        /// Smoothly maps a value from one range [inMin, inMax] to another range [outMin, outMax].
        /// </summary>
        public static float Remap(float value, float inMin, float inMax, float outMin, float outMax, bool clamp = true)
        {
            if (Mathf.Approximately(inMax, inMin))
            {
                return outMin;
            }

            float t = (value - inMin) / (inMax - inMin);
            if (clamp)
            {
                t = Mathf.Clamp01(t);
            }

            return Mathf.LerpUnclamped(outMin, outMax, t);
        }

        /// <summary>
        /// Cubic ease-out calculation for responsive lane-switch snapping.
        /// </summary>
        public static float EaseOutCubic(float t)
        {
            t = Mathf.Clamp01(t);
            float f = t - 1.0f;
            return f * f * f + 1.0f;
        }

        /// <summary>
        /// Cubic ease-in-out calculation for smooth transitions.
        /// </summary>
        public static float EaseInOutCubic(float t)
        {
            t = Mathf.Clamp01(t);
            return t < 0.5f ? 4.0f * t * t * t : 1.0f - Mathf.Pow(-2.0f * t + 2.0f, 3.0f) / 2.0f;
        }
    }
}
