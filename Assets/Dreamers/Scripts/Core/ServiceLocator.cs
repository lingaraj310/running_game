using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dreamers.Core
{
    /// <summary>
    /// Lightweight, zero-overhead Service Locator for runtime dependency resolution.
    /// Provides safe registration, querying, and teardown for core domain services
    /// (e.g. IInputService, ISaveService, IAudioService).
    /// </summary>
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> Services = new Dictionary<Type, object>();

        /// <summary>
        /// Registers a service of type T. Throws an InvalidOperationException if already registered.
        /// </summary>
        public static void Register<T>(T service) where T : class
        {
            Type type = typeof(T);
            if (Services.ContainsKey(type))
            {
                Debug.LogWarning($"[ServiceLocator] Service of type '{type.Name}' is already registered. Overwriting with new instance.");
                Services[type] = service;
                return;
            }

            Services.Add(type, service);
            if (service is IGameService gameService)
            {
                gameService.Initialize();
            }
        }

        /// <summary>
        /// Retrieves a registered service of type T. Logs error and returns null if not found.
        /// </summary>
        public static T Get<T>() where T : class
        {
            Type type = typeof(T);
            if (Services.TryGetValue(type, out object service))
            {
                return (T)service;
            }

            Debug.LogError($"[ServiceLocator] Failed to resolve service of type '{type.Name}'. Ensure it is registered in Bootstrapper.");
            return null;
        }

        /// <summary>
        /// Safe non-throwing lookup for optional services.
        /// </summary>
        public static bool TryGet<T>(out T service) where T : class
        {
            Type type = typeof(T);
            if (Services.TryGetValue(type, out object rawService))
            {
                service = (T)rawService;
                return true;
            }

            service = null;
            return false;
        }

        /// <summary>
        /// Unregisters and shuts down a service of type T.
        /// </summary>
        public static void Unregister<T>() where T : class
        {
            Type type = typeof(T);
            if (Services.TryGetValue(type, out object rawService))
            {
                if (rawService is IGameService gameService)
                {
                    gameService.Shutdown();
                }

                Services.Remove(type);
            }
        }

        /// <summary>
        /// Clears and shuts down all registered services. Useful during scene unload or full test teardown.
        /// </summary>
        public static void Reset()
        {
            foreach (var kvp in Services)
            {
                if (kvp.Value is IGameService gameService)
                {
                    try
                    {
                        gameService.Shutdown();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"[ServiceLocator] Exception shutting down service {kvp.Key.Name}: {ex}");
                    }
                }
            }

            Services.Clear();
        }
    }
}
