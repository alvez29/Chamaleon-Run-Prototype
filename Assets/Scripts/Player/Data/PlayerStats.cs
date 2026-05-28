using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Game.Player.Data
{
    [CreateAssetMenu(fileName = "New Player Stats", menuName = "Game/Player/Stats")]
    public class PlayerStats : ScriptableObject
    {
        [Header("Movement")] [Range(0f, 100f)] [SerializeField]
        private float m_runSpeed = 40f;

        [Range(0f, 100f)] private float m_runAcceleration = 10f;

        [Header("Jumping")] [Range(0f, 100f)] [SerializeField]
        private float m_initialJumpHeight = 11f;

        [Range(0f, 100f)] [SerializeField] private float m_doubleJumpHeight = 6.4f;

        [Range(0f, 100f)] [SerializeField] private float m_fallGravityFactor = 8f;

        [Range(0f, 100f)] [SerializeField] private float m_jumpGravityFactor = 3f;

        [SerializeField] private float m_baseGravity = 9.8f;

        [Header("Ground Detection")] [Range(0f, 20f)] [SerializeField]
        private float m_groundCheckDistance = 0.1f;

        [SerializeField] private float m_groundCheckRadius = 0.3f;
        [SerializeField] private Vector3 m_groundCheckOffset = Vector3.up * 0.1f;
        [SerializeField] private LayerMask m_groundLayer;

        public float RunSpeed => m_runSpeed;
        public float RunAcceleration => m_runAcceleration;
        public float InitialJumpHeight => m_initialJumpHeight;
        public float DoubleJumpHeight => m_doubleJumpHeight;
        public float FallGravityFactor => m_fallGravityFactor;
        public float JumpGravityFactor => m_jumpGravityFactor;
        public float BaseGravity => m_baseGravity;
        public float GroundCheckDistance => m_groundCheckDistance;
        public float GroundCheckRadius => m_groundCheckRadius;
        public Vector3 GroundCheckOffset => m_groundCheckOffset;
        public LayerMask GroundLayer => m_groundLayer;
    }
}