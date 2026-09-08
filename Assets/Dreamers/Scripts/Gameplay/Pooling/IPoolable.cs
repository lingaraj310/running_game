namespace Dreamers.Gameplay.Pooling
{
    /// <summary>
    /// Lifecycle interface for objects managed by the Object Pooling system.
    /// Used by obstacles, collectibles, track segments, and VFX to reset state upon reuse.
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// Called when the object is retrieved from the pool and activated in the scene.
        /// </summary>
        void OnSpawnFromPool();

        /// <summary>
        /// Called when the object is deactivated and returned to the pool.
        /// </summary>
        void OnReturnToPool();
    }
}
