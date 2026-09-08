using System;
using UnityEngine;
using Dreamers.Core;
using Dreamers.Data;
using Dreamers.Input;
using Dreamers.Utilities;

namespace Dreamers.Gameplay.Player
{
    /// <summary>
    /// Core Player Controller for Dreamers — Run Beyond Limits.
    /// Coordinates input consumption, lane movement, jumping, sliding, and physics translation
    /// without entangling UI, score, story, or audio dependencies.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Static Character Configuration")]
        [SerializeField] private CharacterDataSO _characterData;

        [Header("Runtime Tuning Overrides")]
        [SerializeField] private float _forwardSpeed = GameConstants.DEFAULT_RUN_SPEED;
        [SerializeField] private float _laneWidth = GameConstants.LANE_WIDTH;
        [SerializeField] private float _laneSwitchDuration = GameConstants.LANE_SWITCH_DURATION;
        [SerializeField] private float _jumpForce = GameConstants.JUMP_FORCE;
        [SerializeField] private float _gravityScale = GameConstants.GRAVITY_SCALE;
        [SerializeField] private float _slideDuration = GameConstants.SLIDE_DURATION;

        // Subsystems
        private CharacterController _characterController;
        private LaneMovementMotor _laneMotor;
        private JumpSlideMotor _jumpSlideMotor;
        private IInputService _inputService;

        // State
        private PlayerState _currentState = PlayerState.Running;
        private bool _isMovementEnabled = true;

        #region Events
        public event Action<PlayerState> OnStateChanged;
        public event Action<int> OnLaneChanged;
        public event Action OnJumped;
        public event Action OnSlid;
        #endregion

        #region Public Properties
        public PlayerState CurrentState => _currentState;
        public float ForwardSpeed { get => _forwardSpeed; set => _forwardSpeed = Mathf.Max(0f, value); }
        public int CurrentLaneIndex => _laneMotor != null ? _laneMotor.CurrentLaneIndex : GameConstants.DEFAULT_LANE_INDEX;
        public bool IsSliding => _jumpSlideMotor != null && _jumpSlideMotor.IsSliding;
        public bool IsGrounded => _jumpSlideMotor != null && _jumpSlideMotor.IsGrounded;
        public CharacterDataSO CharacterData => _characterData;
        #endregion

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _laneMotor = new LaneMovementMotor(_laneWidth, _laneSwitchDuration);
            _jumpSlideMotor = new JumpSlideMotor(_characterController, _jumpForce, _gravityScale, _slideDuration);

            if (_characterData != null)
            {
                ApplyCharacterData(_characterData);
            }
        }

        private void Start()
        {
            BindInputService();
        }

        private void OnEnable()
        {
            if (_inputService != null)
            {
                SubscribeToInput();
            }
        }

        private void OnDisable()
        {
            UnsubscribeFromInput();
        }

        public void ApplyCharacterData(CharacterDataSO data)
        {
            if (data == null) return;

            _characterData = data;
            _forwardSpeed = data.BaseRunSpeed;
            _laneSwitchDuration = data.LaneSwitchDuration;
            _jumpForce = data.JumpForce;
            _gravityScale = data.GravityScale;
            _slideDuration = data.SlideDuration;

            _laneMotor.Configure(_laneWidth, _laneSwitchDuration);
            _jumpSlideMotor.Configure(_jumpForce, _gravityScale, _slideDuration);
        }

        private void BindInputService()
        {
            if (ServiceLocator.TryGet<IInputService>(out var registeredService))
            {
                _inputService = registeredService;
            }
            else
            {
                // Fallback for standalone/editor testing if Bootstrapper hasn't run
                #if UNITY_ANDROID && !UNITY_EDITOR
                _inputService = new MobileSwipeInputService();
                #else
                _inputService = new StandaloneKeyboardInputService();
                #endif
                _inputService.Initialize();
            }

            SubscribeToInput();
        }

        private void SubscribeToInput()
        {
            if (_inputService == null) return;
            _inputService.OnMoveLeftRequested += HandleMoveLeft;
            _inputService.OnMoveRightRequested += HandleMoveRight;
            _inputService.OnJumpRequested += HandleJump;
            _inputService.OnSlideRequested += HandleSlide;
        }

        private void UnsubscribeFromInput()
        {
            if (_inputService == null) return;
            _inputService.OnMoveLeftRequested -= HandleMoveLeft;
            _inputService.OnMoveRightRequested -= HandleMoveRight;
            _inputService.OnJumpRequested -= HandleJump;
            _inputService.OnSlideRequested -= HandleSlide;
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            // Tick input service if manually owned
            _inputService?.Tick(deltaTime);

            if (!_isMovementEnabled || _currentState == PlayerState.Dead)
            {
                return;
            }

            // 1. Calculate Target Horizontal X
            float currentX = transform.position.x;
            float newX = _laneMotor.UpdateHorizontalPosition(currentX, deltaTime);
            float horizontalDelta = (newX - currentX) / Mathf.Max(deltaTime, 0.0001f);

            // 2. Calculate Vertical Velocity
            float verticalVelocity = _jumpSlideMotor.UpdateVerticalPhysics(_characterController.isGrounded, deltaTime);

            // 3. Assemble Full Velocity Vector
            Vector3 movementVelocity = new Vector3(horizontalDelta, verticalVelocity, _forwardSpeed);

            // 4. Move Character via CharacterController
            _characterController.Move(movementVelocity * deltaTime);

            // 5. Update State Model
            EvaluatePlayerState();
        }

        private void EvaluatePlayerState()
        {
            PlayerState newState;

            if (_jumpSlideMotor.IsSliding)
            {
                newState = PlayerState.Sliding;
            }
            else if (_jumpSlideMotor.IsJumping)
            {
                newState = PlayerState.Jumping;
            }
            else if (_jumpSlideMotor.IsFalling)
            {
                newState = PlayerState.Falling;
            }
            else
            {
                newState = PlayerState.Running;
            }

            if (newState != _currentState && _currentState != PlayerState.Dead && _currentState != PlayerState.Stumbling)
            {
                _currentState = newState;
                OnStateChanged?.Invoke(_currentState);
            }
        }

        #region Input Handlers
        private void HandleMoveLeft()
        {
            if (!_isMovementEnabled || _currentState == PlayerState.Dead) return;

            if (_laneMotor.MoveLeft(transform.position.x))
            {
                OnLaneChanged?.Invoke(_laneMotor.CurrentLaneIndex);
            }
        }

        private void HandleMoveRight()
        {
            if (!_isMovementEnabled || _currentState == PlayerState.Dead) return;

            if (_laneMotor.MoveRight(transform.position.x))
            {
                OnLaneChanged?.Invoke(_laneMotor.CurrentLaneIndex);
            }
        }

        private void HandleJump()
        {
            if (!_isMovementEnabled || _currentState == PlayerState.Dead) return;

            if (_jumpSlideMotor.Jump())
            {
                OnJumped?.Invoke();
            }
        }

        private void HandleSlide()
        {
            if (!_isMovementEnabled || _currentState == PlayerState.Dead) return;

            if (_jumpSlideMotor.Slide())
            {
                OnSlid?.Invoke();
            }
        }
        #endregion

        #region Public Control API
        public void SetMovementEnabled(bool isEnabled)
        {
            _isMovementEnabled = isEnabled;
        }

        public void TriggerStumble(float duration = 1.0f)
        {
            if (_currentState == PlayerState.Dead) return;

            _currentState = PlayerState.Stumbling;
            OnStateChanged?.Invoke(_currentState);
        }

        public void Die()
        {
            _currentState = PlayerState.Dead;
            _isMovementEnabled = false;
            OnStateChanged?.Invoke(_currentState);
        }

        public void ResetPosition(Vector3 startPosition)
        {
            transform.position = startPosition;
            _laneMotor.SetLane(GameConstants.DEFAULT_LANE_INDEX, startPosition.x, true);
            _jumpSlideMotor.Reset();
            _currentState = PlayerState.Running;
            _isMovementEnabled = true;
            OnStateChanged?.Invoke(_currentState);
        }
        #endregion
    }
}
