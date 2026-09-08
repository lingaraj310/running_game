using UnityEngine;

namespace Dreamers.Utilities
{
    /// <summary>
    /// Provides reusable, preallocated buffers and non-allocating physics query helper methods.
    /// Eliminates garbage collection allocations during ground detection, obstacle sweeps, and trigger checks on Android.
    /// </summary>
    public static class NonAllocPhysics
    {
        private const int MAX_COLLIDER_BUFFER = 16;
        private const int MAX_RAYCAST_BUFFER = 8;

        private static readonly Collider[] ColliderBuffer = new Collider[MAX_COLLIDER_BUFFER];
        private static readonly RaycastHit[] RaycastHitBuffer = new RaycastHit[MAX_RAYCAST_BUFFER];

        /// <summary>
        /// Performs a non-allocating sphere cast returning the closest valid hit.
        /// </summary>
        public static bool SphereCastSingle(Vector3 origin, float radius, Vector3 direction, float maxDistance, int layerMask, out RaycastHit hit)
        {
            hit = default;
            int count = Physics.SphereCastNonAlloc(origin, radius, direction, RaycastHitBuffer, maxDistance, layerMask, QueryTriggerInteraction.Ignore);
            if (count == 0)
            {
                return false;
            }

            float closestDistance = float.MaxValue;
            int closestIndex = -1;

            for (int i = 0; i < count; i++)
            {
                if (RaycastHitBuffer[i].distance < closestDistance)
                {
                    closestDistance = RaycastHitBuffer[i].distance;
                    closestIndex = i;
                }
            }

            if (closestIndex >= 0)
            {
                hit = RaycastHitBuffer[closestIndex];
                return true;
            }

            return false;
        }

        /// <summary>
        /// Performs a non-allocating raycast returning the closest hit.
        /// </summary>
        public static bool RaycastSingle(Vector3 origin, Vector3 direction, float maxDistance, int layerMask, out RaycastHit hit)
        {
            hit = default;
            int count = Physics.RaycastNonAlloc(origin, direction, RaycastHitBuffer, maxDistance, layerMask, QueryTriggerInteraction.Ignore);
            if (count == 0)
            {
                return false;
            }

            float closestDistance = float.MaxValue;
            int closestIndex = -1;

            for (int i = 0; i < count; i++)
            {
                if (RaycastHitBuffer[i].distance < closestDistance)
                {
                    closestDistance = RaycastHitBuffer[i].distance;
                    closestIndex = i;
                }
            }

            if (closestIndex >= 0)
            {
                hit = RaycastHitBuffer[closestIndex];
                return true;
            }

            return false;
        }

        /// <summary>
        /// Non-allocating overlap box check. Returns number of colliders found and fills passed or static buffer.
        /// </summary>
        public static int OverlapBox(Vector3 center, Vector3 halfExtents, Quaternion orientation, int layerMask, Collider[] results = null)
        {
            Collider[] targetBuffer = results ?? ColliderBuffer;
            return Physics.OverlapBoxNonAlloc(center, halfExtents, targetBuffer, orientation, layerMask, QueryTriggerInteraction.Collide);
        }
    }
}
