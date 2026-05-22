using UnityEngine;

namespace Game.Player.Data
{
    [CreateAssetMenu(fileName = "New Player Stats", menuName = "Game/Player/Stats")]
    public class PlayerStats : ScriptableObject
    {
        [Header("Movement")]
        public float RunSpeed = 10f;

        [Header("Jumping")]
        public float InitialJumpForce = 12f;
        public float DoubleJumpForce = 8f;
        
        [Tooltip("Multiplier applied to gravity when falling to make the jump feel heavier/faster.")]
        public float FallMultiplier = 2.5f;
        
        [Tooltip("Multiplier applied to gravity when the jump button is released early (variable jump).")]
        public float LowJumpMultiplier = 2f;

        [Header("Ground Detection")]
        public float GroundCheckDistance = 0.1f;
        public float GroundCheckRadius = 0.3f;
        public Vector3 GroundCheckOffset = Vector3.up * 0.1f;
        public LayerMask GroundLayer;
    }
}
