using UnityEngine;

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
        [Range(0f, 200f)]
        public float InitialJumpForce = 12f;
        [Range(0f, 200f)]
        public float DoubleJumpForce = 8f;
        
        [Tooltip("Multiplier applied to gravity when falling to make the jump feel heavier/faster.")]
        [Range(0f, 100f)]
        public float FallMultiplier = 2.5f;
        
        [Tooltip("Multiplier applied to gravity when the jump button is released early (variable jump).")]
        [Range(0f, 100f)]
        public float LowJumpMultiplier = 2f;

        [Header("Ground Detection")]
        [Range(0f, 20f)]
        public float GroundCheckDistance = 0.1f;
        [Range(0f, 20f)]
        public float GroundCheckRadius = 0.3f;
        public Vector3 GroundCheckOffset = Vector3.up * 0.1f;
        public LayerMask GroundLayer;
    }
}
