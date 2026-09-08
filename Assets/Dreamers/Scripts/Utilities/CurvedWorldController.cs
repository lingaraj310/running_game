using UnityEngine;

namespace Dreamers.Utilities
{
    /// <summary>
    /// Updates the global vertex bending parameters for the Curved World shader.
    /// Controls the horizon drop (BendY) and lateral turn curvature (BendX).
    /// </summary>
    [ExecuteAlways]
    public class CurvedWorldController : MonoBehaviour
    {
        [Header("Curvature Settings")]
        [Tooltip("Downward curvature creating the horizon drop.")]
        [Range(-5.0f, 15.0f)]
        [SerializeField] private float _bendY = 1.8f;

        [Tooltip("Lateral curvature creating banking corners.")]
        [Range(-10.0f, 10.0f)]
        [SerializeField] private float _bendX = 0.0f;

        [Header("Target Tracking")]
        [SerializeField] private Transform _originTransform;

        private static readonly int CurvedWorldOriginId = Shader.PropertyToID("_CurvedWorldOrigin");
        private static readonly int CurvedWorldBendXId = Shader.PropertyToID("_CurvedWorldBendX");
        private static readonly int CurvedWorldBendYId = Shader.PropertyToID("_CurvedWorldBendY");

        public float BendY { get => _bendY; set => _bendY = value; }
        public float BendX { get => _bendX; set => _bendX = value; }

        private void LateUpdate()
        {
            Vector3 originPos = _originTransform != null ? _originTransform.position : transform.position;

            Shader.SetGlobalVector(CurvedWorldOriginId, originPos);
            Shader.SetGlobalFloat(CurvedWorldBendXId, _bendX);
            Shader.SetGlobalFloat(CurvedWorldBendYId, _bendY);
        }
    }
}
