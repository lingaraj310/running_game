using System;
using UnityEngine;
using Dreamers.Utilities;

namespace Dreamers.Input
{
    /// <summary>
    /// Mobile touch swipe detector for Android.
    /// Analyzes touch phase, delta vector, and DPI-scaled thresholds to dispatch discrete runner actions.
    /// </summary>
    public class MobileSwipeInputService : IInputService
    {
        public event Action OnMoveLeftRequested;
        public event Action OnMoveRightRequested;
        public event Action OnJumpRequested;
        public event Action OnSlideRequested;
        public event Action OnPauseRequested;

        public bool IsEnabled { get; set; } = true;

        private readonly float _minSwipeDistancePixels;
        private readonly float _maxSwipeDuration;

        private Vector2 _touchStartPosition;
        private float _touchStartTime;
        private int _trackedFingerId = -1;
        private bool _isTrackingSwipe;

        public MobileSwipeInputService(float minSwipeDistanceInches = GameConstants.DEFAULT_MIN_SWIPE_DISTANCE_INCHES, float maxSwipeDuration = GameConstants.MAX_SWIPE_TIME_SECONDS)
        {
            float dpi = Screen.dpi > 0 ? Screen.dpi : 160f; // Standard baseline fallback
            _minSwipeDistancePixels = minSwipeDistanceInches * dpi;
            _maxSwipeDuration = maxSwipeDuration;
        }

        public void Initialize()
        {
            ResetTouchState();
        }

        public void Shutdown()
        {
            ResetTouchState();
        }

        public void Tick(float deltaTime)
        {
            if (!IsEnabled || UnityEngine.Input.touchCount == 0)
            {
                if (_isTrackingSwipe && UnityEngine.Input.touchCount == 0)
                {
                    ResetTouchState();
                }
                return;
            }

            for (int i = 0; i < UnityEngine.Input.touchCount; i++)
            {
                Touch touch = UnityEngine.Input.GetTouch(i);

                if (!_isTrackingSwipe && touch.phase == TouchPhase.Began)
                {
                    _trackedFingerId = touch.fingerId;
                    _touchStartPosition = touch.position;
                    _touchStartTime = Time.unscaledTime;
                    _isTrackingSwipe = true;
                    break;
                }

                if (_isTrackingSwipe && touch.fingerId == _trackedFingerId)
                {
                    if (touch.phase == TouchPhase.Ended)
                    {
                        ProcessSwipeEnd(touch.position);
                        ResetTouchState();
                        break;
                    }
                    else if (touch.phase == TouchPhase.Canceled)
                    {
                        ResetTouchState();
                        break;
                    }
                }
            }
        }

        private void ProcessSwipeEnd(Vector2 touchEndPosition)
        {
            float duration = Time.unscaledTime - _touchStartTime;
            if (duration > _maxSwipeDuration)
            {
                return; // Dragged too slowly to count as a quick swipe
            }

            Vector2 delta = touchEndPosition - _touchStartPosition;
            if (delta.magnitude < _minSwipeDistancePixels)
            {
                return; // Movement below minimum swipe threshold
            }

            // Determine dominant direction
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                if (delta.x > 0)
                {
                    OnMoveRightRequested?.Invoke();
                }
                else
                {
                    OnMoveLeftRequested?.Invoke();
                }
            }
            else
            {
                if (delta.y > 0)
                {
                    OnJumpRequested?.Invoke();
                }
                else
                {
                    OnSlideRequested?.Invoke();
                }
            }
        }

        private void ResetTouchState()
        {
            _trackedFingerId = -1;
            _isTrackingSwipe = false;
        }
    }
}
