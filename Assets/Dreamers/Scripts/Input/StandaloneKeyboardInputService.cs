using System;
using UnityEngine;

namespace Dreamers.Input
{
    /// <summary>
    /// Standalone keyboard input service for testing in the Unity Editor or desktop development builds.
    /// Maps key presses to discrete runner actions.
    /// </summary>
    public class StandaloneKeyboardInputService : IInputService
    {
        public event Action OnMoveLeftRequested;
        public event Action OnMoveRightRequested;
        public event Action OnJumpRequested;
        public event Action OnSlideRequested;
        public event Action OnPauseRequested;

        public bool IsEnabled { get; set; } = true;

        public void Initialize() { }
        public void Shutdown() { }

        public void Tick(float deltaTime)
        {
            if (!IsEnabled)
            {
                return;
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.A) || UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
            {
                OnMoveLeftRequested?.Invoke();
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.D) || UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
            {
                OnMoveRightRequested?.Invoke();
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Space) || UnityEngine.Input.GetKeyDown(KeyCode.W) || UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
            {
                OnJumpRequested?.Invoke();
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.S) || UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
            {
                OnSlideRequested?.Invoke();
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                OnPauseRequested?.Invoke();
            }
        }
    }
}
