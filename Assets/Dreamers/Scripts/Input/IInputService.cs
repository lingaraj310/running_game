using System;
using Dreamers.Core;

namespace Dreamers.Input
{
    /// <summary>
    /// Abstraction for user input across Mobile touch/swipe, Standalone keyboard, or gamepad.
    /// Decouples PlayerController from direct Unity Input / UnityEngine.InputSystem calls.
    /// </summary>
    public interface IInputService : IGameService
    {
        event Action OnMoveLeftRequested;
        event Action OnMoveRightRequested;
        event Action OnJumpRequested;
        event Action OnSlideRequested;
        event Action OnPauseRequested;

        bool IsEnabled { get; set; }
        void Tick(float deltaTime);
    }
}
