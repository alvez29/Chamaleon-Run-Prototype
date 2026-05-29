using System;
using Game.Player.Data;
using UnityEngine;

namespace Game.Player
{
    public class PlayerGroundDetector : MonoBehaviour
    {
        public event Action OnGroundDetected;
        public event Action OnGroundLost;
        
        [SerializeField] private PlayerStats m_stats;

        public bool IsGrounded { get; private set; }

        public void CheckGround(Vector3 groundCheckOffset, float groundCheckRadius, LayerMask groundLayer)
        {
            if (!m_stats) return;

            bool lastGroundedState = IsGrounded;
            
            Vector3 origin = transform.position + groundCheckOffset;
            Vector3 sphereCenter = origin + (Vector3.down * groundCheckRadius);
            
            IsGrounded = Physics.CheckSphere(sphereCenter, groundCheckRadius, groundLayer);
            
            if (lastGroundedState != IsGrounded && IsGrounded)
                OnGroundDetected?.Invoke();
            else if (lastGroundedState != IsGrounded && !IsGrounded)
                OnGroundLost?.Invoke();
        }

        private void OnDrawGizmos()
        {
            if (m_stats == null) return;

            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Vector3 origin = transform.position + m_stats.GroundCheckOffset;
            Gizmos.DrawWireSphere(origin + Vector3.down * m_stats.GroundCheckDistance, m_stats.GroundCheckRadius);
        }
    }
}