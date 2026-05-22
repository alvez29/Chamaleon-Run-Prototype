using UnityEngine;

namespace Game.Player.Data
{
    [CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Game/Player/Stats")]
    public class PlayerStats : ScriptableObject
    {
        [Header("Movement")]
        public float runSpeed = 10f;

        [Header("Jumping")]
        public float initialJumpForce = 12f;
        public float doubleJumpForce = 8f;
        
        [Tooltip("Multiplier applied to gravity when falling to make the jump feel heavier/faster.")]
        public float fallMultiplier = 2.5f;
        
        [Tooltip("Multiplier applied to gravity when the jump button is released early (variable jump).")]
        public float lowJumpMultiplier = 2f;

        [Header("Ground Detection")]
        public float groundCheckDistance = 0.1f;
        public float groundCheckRadius = 0.3f;
        public Vector3 groundCheckOffset = Vector3.up * 0.1f;
        public LayerMask groundLayer;
    }
}
