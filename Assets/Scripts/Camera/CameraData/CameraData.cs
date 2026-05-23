using UnityEngine;

namespace Game.Camera
{
    [CreateAssetMenu(fileName = "New Camera Data", menuName = "Game/Camera", order = 0)]
    public class CameraData : ScriptableObject
    {
        public Vector3 Offset = new(5f, 3f, -10f);
        public Vector2 SmoothTime = new(0.1f, 0.1f);
        public float RotationInDegrees = 30.0f; 
    }
}