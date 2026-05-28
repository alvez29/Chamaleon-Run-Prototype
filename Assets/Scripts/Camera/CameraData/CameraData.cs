using UnityEngine;

namespace Game.Camera
{
    [CreateAssetMenu(fileName = "New Camera Data", menuName = "Game/Camera", order = 0)]
    public class CameraData : ScriptableObject
    {
        [SerializeField] private Vector3 m_offset = new(5f, 3f, -10f);
        [SerializeField] private Vector2 m_smoothTime = new(0.1f, 0.1f);
        [SerializeField] private float m_rotationInDegrees = 30.0f;

        public Vector3 Offset => m_offset;
        public Vector2 SmoothTime => m_smoothTime;
        public float RotationInDegrees => m_rotationInDegrees;
    }
}