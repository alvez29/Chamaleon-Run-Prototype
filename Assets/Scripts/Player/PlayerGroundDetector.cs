using Game.Player.Data;
using UnityEngine;

namespace Game.Player
{
    public class PlayerGroundDetector : MonoBehaviour
    {
        [SerializeField] private PlayerStats m_stats;

        public bool IsGrounded { get; private set; }

        private void FixedUpdate()
        {
            if (!m_stats) return;
            
            Vector3 origin = transform.position + m_stats.GroundCheckOffset;
            Vector3 sphereCenter = origin + (Vector3.down * m_stats.GroundCheckDistance);
            
            IsGrounded = Physics.CheckSphere(sphereCenter, m_stats.GroundCheckRadius, m_stats.GroundLayer); 
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