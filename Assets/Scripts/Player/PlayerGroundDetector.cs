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

            Vector3 origin = transform.position + m_stats.groundCheckOffset;

            IsGrounded = Physics.SphereCast(
                origin,
                m_stats.groundCheckRadius,
                Vector3.down,
                out RaycastHit hit,
                m_stats.groundCheckDistance,
                m_stats.groundLayer
            );
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Vector3 origin = transform.position + m_stats?.groundCheckOffset ?? Vector3.zero;
            Gizmos.DrawWireSphere(origin + Vector3.down * m_stats?.groundCheckDistance ?? Vector3.zero,
                m_stats?.groundCheckRadius ?? 0.0f);
        }
    }
}