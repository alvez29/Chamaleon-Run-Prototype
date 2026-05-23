using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Game.Player.Data
{
    [CreateAssetMenu(fileName = "New Player Stats", menuName = "Game/Player/Stats")]
    public class PlayerStats : ScriptableObject
    {
        [Header("Movement")]
        [Range(0f, 100f)]
        public float RunSpeed = 10f;
        [Range(0f, 100f)]
        public float RunAcceleration = 10f;

        [Header("Jumping")]
        [Range(0f, 100f)]
        public float InitialJumpHeight = 3f;
        [Range(0f, 100f)]
        public float DoubleJumpHeight = 2f;
        
        [Range(0f, 100f)]
        public float FallGravityFactor = 10f;
        
        [Range(0f, 100f)]
        public float JumpGravityFactor = 1f;
        
        public float BaseGravity = 9.8f;
        
        [Header("Ground Detection")]
        [Range(0f, 20f)]
        public float GroundCheckDistance = 0.1f;
        public float GroundCheckRadius = 0.1f;
        public Vector3 GroundCheckOffset = Vector3.up * 0.1f;
        public LayerMask GroundLayer;
    }
}
