using UnityEngine;

namespace Dreamers.UI
{
    /// <summary>
    /// Coordinates the 3D Interactive CSE Student Lab in the Main Menu:
    /// - 360-degree rotation drag controls
    /// - Workstation monitors and RGB tower lighting
    /// - Character outfit and hoverboard swapping
    /// </summary>
    public class StudentLabShowcase : MonoBehaviour
    {
        [Header("Showcase Platform")]
        [SerializeField] private Transform _characterPedestal;
        [SerializeField] private float _rotationSpeed = 45f;
        [SerializeField] private bool _autoRotate = true;

        [Header("Lab Props")]
        [SerializeField] private Renderer[] _monitorScreens;
        [SerializeField] private Light _rgbTowerLight;

        private void Update()
        {
            if (_autoRotate && _characterPedestal != null)
            {
                _characterPedestal.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
            }

            // RGB color cycling for student PC rig
            if (_rgbTowerLight != null)
            {
                float hue = Mathf.PingPong(Time.time * 0.2f, 1f);
                _rgbTowerLight.color = Color.HSVToRGB(hue, 0.8f, 1f);
            }
        }

        public void ManualRotate(float deltaX)
        {
            if (_characterPedestal != null)
            {
                _characterPedestal.Rotate(Vector3.up, -deltaX * 0.5f);
            }
        }
    }
}
