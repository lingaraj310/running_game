using UnityEngine;
using Dreamers.Utilities;

namespace Dreamers.Gameplay.Player
{
    /// <summary>
    /// Handles vertical jump mechanics and horizontal slide crouching.
    /// Manages vertical velocity, gravity application, ground snapping, and collider dimensions.
    /// </summary>
    public class JumpSlideMotor
    {
        private float _jumpForce = GameConstants.JUMP_FORCE;
        private float _gravityScale = GameConstants.GRAVITY_SCALE;
        private float _slideDuration = GameConstants.SLIDE_DURATION;

        private float _verticalVelocity;
        private bool _isGrounded = true;
        private bool _isSliding;
        private float _slideTimer;

        // CharacterController dimensions
        private CharacterController _characterController;
        private float _originalHeight = 2.0f;
        private Vector3 _originalCenter = new Vector3(0f, 1.0f, 0f);

        public bool IsGrounded => _isGrounded;
        public bool IsJumping => !_isGrounded && _verticalVelocity > 0;
        public bool IsFalling => !_isGrounded && _verticalVelocity <= 0;
        public bool IsSliding => _isSliding;
        public float VerticalVelocity => _verticalVelocity;

        public JumpSlideMotor(CharacterController characterController, float jumpForce = GameConstants.JUMP_FORCE, float gravityScale = GameConstants.GRAVITY_SCALE, float slideDuration = GameConstants.SLIDE_DURATION)
        {
            _characterController = characterController;
            _jumpForce = jumpForce;
            _gravityScale = gravityScale;
            _slideDuration = slideDuration;

            if (_characterController != null)
            {
                _originalHeight = _characterController.height;
                _originalCenter = _characterController.center;
            }
        }

        public void Configure(float jumpForce, float gravityScale, float slideDuration)
        {
            _jumpForce = jumpForce;
            _gravityScale = gravityScale;
            _slideDuration = slideDuration;
        }

        /// <summary>
        /// Attempts to initiate a jump. If sliding, immediately cancels slide into jump.
        /// </summary>
        public bool Jump()
        {
            if (_isGrounded)
            {
                if (_isSliding)
                {
                    EndSlide();
                }

                _verticalVelocity = _jumpForce;
                _isGrounded = false;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Initiates a slide. If in air, applies downward fast-fall velocity.
        /// </summary>
        public bool Slide()
        {
            if (!_isGrounded)
            {
                // Fast-fall downward
                _verticalVelocity = -_jumpForce * 1.5f;
                return true;
            }

            if (!_isSliding)
            {
                StartSlide();
                return true;
            }
            else
            {
                // Refresh slide duration if swiped down again
                _slideTimer = _slideDuration;
                return true;
            }
        }

        private void StartSlide()
        {
            _isSliding = true;
            _slideTimer = _slideDuration;

            if (_characterController != null)
            {
                _characterController.height = _originalHeight * GameConstants.SLIDE_COLLIDER_HEIGHT_RATIO;
                _characterController.center = new Vector3(_originalCenter.x, _originalCenter.y * GameConstants.SLIDE_COLLIDER_HEIGHT_RATIO, _originalCenter.z);
            }
        }

        private void EndSlide()
        {
            _isSliding = false;
            _slideTimer = 0f;

            if (_characterController != null)
            {
                _characterController.height = _originalHeight;
                _characterController.center = _originalCenter;
            }
        }

        /// <summary>
        /// Updates vertical physics and slide timer.
        /// </summary>
        public float UpdateVerticalPhysics(bool controllerIsGrounded, float deltaTime)
        {
            // Slide timer countdown
            if (_isSliding)
            {
                _slideTimer -= deltaTime;
                if (_slideTimer <= 0f)
                {
                    EndSlide();
                }
            }

            // Ground & Gravity resolution
            if (controllerIsGrounded)
            {
                if (_verticalVelocity < 0f)
                {
                    // Small negative force to keep character securely glued to floor slopes
                    _verticalVelocity = -2.0f;
                    _isGrounded = true;
                }
            }
            else
            {
                _isGrounded = false;
                _verticalVelocity -= _gravityScale * deltaTime;
            }

            return _verticalVelocity;
        }

        /// <summary>
        /// Resets motor state back to default running.
        /// </summary>
        public void Reset()
        {
            _verticalVelocity = 0f;
            _isGrounded = true;
            EndSlide();
        }
    }
}
