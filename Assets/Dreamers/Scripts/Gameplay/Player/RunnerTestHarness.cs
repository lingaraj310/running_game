using UnityEngine;
using Dreamers.Core;
using Dreamers.Input;
using Dreamers.Utilities;

namespace Dreamers.Gameplay.Player
{
    /// <summary>
    /// Test harness for Phase 1 playable runner verification in Unity.
    /// Sets up input, instantiates lane ground visualizers, coordinates the camera, and renders debug HUD info.
    /// </summary>
    public class RunnerTestHarness : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Camera.CameraFollower _cameraFollower;

        [Header("Ground Generation")]
        [SerializeField] private bool _generateTestTrack = true;
        [SerializeField] private int _trackSegmentsCount = 10;
        [SerializeField] private float _segmentLength = 30f;

        private IInputService _inputService;
        private Transform _trackContainer;

        private void Awake()
        {
            // Register input service in ServiceLocator if not already registered
            if (!ServiceLocator.TryGet<IInputService>(out _inputService))
            {
                #if UNITY_ANDROID && !UNITY_EDITOR
                _inputService = new MobileSwipeInputService();
                #else
                _inputService = new StandaloneKeyboardInputService();
                #endif
                ServiceLocator.Register<IInputService>(_inputService);
            }

            if (_generateTestTrack)
            {
                BuildTestTrack();
            }
        }

        private void Start()
        {
            if (_playerController == null)
            {
                _playerController = FindFirstObjectByType<PlayerController>();
            }

            if (_cameraFollower == null)
            {
                _cameraFollower = FindFirstObjectByType<Camera.CameraFollower>();
            }

            if (_cameraFollower != null && _playerController != null)
            {
                _cameraFollower.SetTarget(_playerController.transform);
            }
        }

        private void Update()
        {
            _inputService?.Tick(Time.deltaTime);

            // Infinite track extender for endless testing
            if (_playerController != null && _trackContainer != null)
            {
                float playerZ = _playerController.transform.position.z;
                foreach (Transform segment in _trackContainer)
                {
                    if (segment.position.z < playerZ - _segmentLength)
                    {
                        segment.position += Vector3.forward * (_trackSegmentsCount * _segmentLength);
                    }
                }
            }
        }

        private void BuildTestTrack()
        {
            _trackContainer = new GameObject("TestTrack_Container").transform;
            _trackContainer.SetParent(transform);

            for (int i = 0; i < _trackSegmentsCount; i++)
            {
                GameObject segment = GameObject.CreatePrimitive(PrimitiveType.Cube);
                segment.name = $"TrackSegment_{i}";
                segment.transform.SetParent(_trackContainer);
                segment.transform.position = new Vector3(0f, -0.5f, i * _segmentLength + (_segmentLength * 0.5f));
                segment.transform.localScale = new Vector3(GameConstants.LANE_WIDTH * 3.5f, 1f, _segmentLength);

                // Give it a dark road color
                Renderer rend = segment.GetComponent<Renderer>();
                if (rend != null)
                {
                    rend.material.color = (i % 2 == 0) ? new Color(0.15f, 0.15f, 0.18f) : new Color(0.18f, 0.18f, 0.22f);
                }
            }
        }

        private void OnGUI()
        {
            if (_playerController == null) return;

            GUIStyle boxStyle = new GUIStyle(GUI.skin.box)
            {
                alignment = TextAnchor.UpperLeft,
                fontSize = 14
            };

            GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 16,
                fontStyle = FontStyle.Bold
            };
            titleStyle.normal.textColor = Color.yellow;

            GUILayout.BeginArea(new Rect(20, 20, 340, 240), boxStyle);
            GUILayout.Label("🎮 DREAMERS — Phase 1 Prototype", titleStyle);
            GUILayout.Space(5);

            string laneStr = _playerController.CurrentLaneIndex switch
            {
                0 => "LEFT (-2.0m)",
                1 => "CENTER (0.0m)",
                2 => "RIGHT (+2.0m)",
                _ => "UNKNOWN"
            };

            GUILayout.Label($"<b>State:</b> <color=cyan>{_playerController.CurrentState}</color>");
            GUILayout.Label($"<b>Active Lane:</b> {laneStr}");
            GUILayout.Label($"<b>Forward Speed:</b> {_playerController.ForwardSpeed:F1} m/s");
            GUILayout.Label($"<b>Grounded:</b> {_playerController.IsGrounded} | <b>Sliding:</b> {_playerController.IsSliding}");
            GUILayout.Label($"<b>Position:</b> {_playerController.transform.position}");
            GUILayout.Space(8);

            GUILayout.Label("<b>Controls (Editor / PC):</b>");
            GUILayout.Label("• <b>A / Left:</b> Move Left | <b>D / Right:</b> Move Right");
            GUILayout.Label("• <b>Space / W:</b> Jump | <b>S / Down:</b> Slide");
            GUILayout.Label("• <b>Touch:</b> Swipe Left/Right/Up/Down");

            GUILayout.EndArea();
        }
    }
}
