namespace Dreamers.Core
{
    /// <summary>
    /// Base contract for all systems registered within the ServiceLocator.
    /// Ensures consistent lifecycle management across core systems.
    /// </summary>
    public interface IGameService
    {
        void Initialize();
        void Shutdown();
    }
}
