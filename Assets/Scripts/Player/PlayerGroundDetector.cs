using System;
using Game.Player.Data;
using UnityEngine;

namespace Game.Player
{
    public class PlayerGroundDetector : MonoBehaviour
    {
        public event Action OnGroundDetected;
        
        [SerializeField] private PlayerStats m_stats;

        public bool IsGrounded { get; private set; }

        public void CheckGround()
        {
            if (!m_stats) return;

            bool lastGroundedState = IsGrounded;
            
            Vector3 origin = transform.position + m_stats.GroundCheckOffset;
            Vector3 sphereCenter = origin + (Vector3.down * m_stats.GroundCheckDistance);
            
            IsGrounded = Physics.CheckSphere(sphereCenter, m_stats.GroundCheckRadius, m_stats.GroundLayer);
            
            if (lastGroundedState != IsGrounded && IsGrounded)
                OnGroundDetected?.Invoke();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Vector3 origin = transform.position + m_stats?.GroundCheckOffset ?? Vector3.zero;
            Gizmos.DrawWireSphere(origin + Vector3.down * m_stats?.GroundCheckDistance ?? Vector3.zero,
                m_stats?.GroundCheckRadius ?? 0.0f);
        }
    }
}