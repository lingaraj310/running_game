using UnityEngine;
using Dreamers.Utilities;

namespace Dreamers.Gameplay.Player
{
    /// <summary>
    /// Responsible exclusively for horizontal 3-lane movement and interpolation.
    /// Clamps movement between left (-2m), center (0m), and right (+2m) lanes with smooth cubic interpolation.
    /// </summary>
    public class LaneMovementMotor
    {
        private int _currentLaneIndex = GameConstants.DEFAULT_LANE_INDEX;
        private int _targetLaneIndex = GameConstants.DEFAULT_LANE_INDEX;

        private float _laneWidth = GameConstants.LANE_WIDTH;
        private float _switchDuration = GameConstants.LANE_SWITCH_DURATION;

        private float _startLaneX;
        private float _targetLaneX;
        private float _switchProgress = 1.0f;

        public int CurrentLaneIndex => _targetLaneIndex;
        public bool IsChangingLanes => _switchProgress < 1.0f;
        public float CurrentTargetX => GetLaneXCoordinate(_targetLaneIndex);

        public LaneMovementMotor(float laneWidth = GameConstants.LANE_WIDTH, float switchDuration = GameConstants.LANE_SWITCH_DURATION)
        {
            _laneWidth = laneWidth;
            _switchDuration = switchDuration;
            _currentLaneIndex = GameConstants.DEFAULT_LANE_INDEX;
            _targetLaneIndex = GameConstants.DEFAULT_LANE_INDEX;
            _targetLaneX = GetLaneXCoordinate(_targetLaneIndex);
            _startLaneX = _targetLaneX;
        }

        public void Configure(float laneWidth, float switchDuration)
        {
            _laneWidth = laneWidth;
            _switchDuration = Mathf.Max(0.05f, switchDuration);
        }

        /// <summary>
        /// Requests a transition to the adjacent left lane.
        /// </summary>
        public bool MoveLeft(float currentPosX)
        {
            if (_targetLaneIndex > GameConstants.LANE_INDEX_LEFT)
            {
                SwitchToLane(_targetLaneIndex - 1, currentPosX);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Requests a transition to the adjacent right lane.
        /// </summary>
        public bool MoveRight(float currentPosX)
        {
            if (_targetLaneIndex < GameConstants.LANE_INDEX_RIGHT)
            {
                SwitchToLane(_targetLaneIndex + 1, currentPosX);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Directly forces or smoothly sets the active lane.
        /// </summary>
        public void SetLane(int newLaneIndex, float currentPosX, bool immediate = false)
        {
            newLaneIndex = Mathf.Clamp(newLaneIndex, GameConstants.LANE_INDEX_LEFT, GameConstants.LANE_INDEX_RIGHT);
            if (immediate)
            {
                _currentLaneIndex = newLaneIndex;
                _targetLaneIndex = newLaneIndex;
                _targetLaneX = GetLaneXCoordinate(newLaneIndex);
                _startLaneX = _targetLaneX;
                _switchProgress = 1.0f;
            }
            else
            {
                SwitchToLane(newLaneIndex, currentPosX);
            }
        }

        private void SwitchToLane(int newLaneIndex, float currentPosX)
        {
            _currentLaneIndex = _targetLaneIndex;
            _targetLaneIndex = newLaneIndex;
            _startLaneX = currentPosX;
            _targetLaneX = GetLaneXCoordinate(_targetLaneIndex);
            _switchProgress = 0.0f;
        }

        /// <summary>
        /// Updates horizontal X position based on elapsed time and smooth cubic easing.
        /// </summary>
        public float UpdateHorizontalPosition(float currentX, float deltaTime)
        {
            if (_switchProgress >= 1.0f)
            {
                return _targetLaneX;
            }

            _switchProgress += deltaTime / _switchDuration;
            float easedT = MathExtensions.EaseOutCubic(_switchProgress);
            float newX = Mathf.LerpUnclamped(_startLaneX, _targetLaneX, easedT);

            if (_switchProgress >= 1.0f)
            {
                newX = _targetLaneX;
                _currentLaneIndex = _targetLaneIndex;
            }

            return newX;
        }

        private float GetLaneXCoordinate(int laneIndex)
        {
            return (laneIndex - GameConstants.LANE_INDEX_CENTER) * _laneWidth;
        }
    }
}
