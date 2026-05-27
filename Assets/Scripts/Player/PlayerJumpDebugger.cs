using Game.Player.Data;
using UnityEngine;

namespace Game.Player
{
    public class PlayerJumpDebugger : MonoBehaviour
    {
        [SerializeField] private PlayerStats m_stats;
        
        [Header("Simulation Settings")]
        [SerializeField] private bool m_simulateDoubleJump;
        
        [Range(0f, 1f)]
        [SerializeField] private float m_doubleJumpTime = 0.35f;

        private void OnDrawGizmos()
        {
            if (m_stats == null) return;

            DrawTrajectory(simulateShortJump: true, colorUp: Color.yellow);
            DrawTrajectory(simulateShortJump: false, colorUp: Color.green);
        }

        private void DrawTrajectory(bool simulateShortJump, Color colorUp)
        {
            Vector3 currentPos = transform.position;
            float currentVx = m_stats.RunSpeed;
            
            Rigidbody rb = GetComponent<Rigidbody>();
            float unityGravity = rb is {useGravity: true} ? Physics.gravity.y : 0f;
            
            float effectiveUpGravity = unityGravity - (m_stats.BaseGravity * m_stats.JumpGravityFactor);
            float effectiveDownGravity = unityGravity - (m_stats.BaseGravity * m_stats.FallGravityFactor);
            
            float currentVy = Mathf.Sqrt(m_stats.InitialJumpHeight * -2f * effectiveUpGravity);
            
            float dt = Time.fixedDeltaTime; 
            float timeElapsed = 0f;
            bool doubleJumpTriggered = false;

            for (int i = 0; i < 150; i++)
            {
                Vector3 nextPos = currentPos;
                
                if (m_simulateDoubleJump && !doubleJumpTriggered && timeElapsed >= m_doubleJumpTime)
                {
                    currentVy = Mathf.Sqrt(m_stats.DoubleJumpHeight * -2f * effectiveUpGravity);
                    doubleJumpTriggered = true;
                    
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireSphere(currentPos, 0.2f);
                }

                float currentEffectiveGravity;
                
                if (currentVy < 0)
                {
                    currentEffectiveGravity = effectiveDownGravity;
                    Gizmos.color = Color.red;
                }
                else
                {
                    if (simulateShortJump && !doubleJumpTriggered)
                    {
                        currentEffectiveGravity = effectiveDownGravity;
                        Gizmos.color = colorUp;
                    }
                    else
                    {
                        currentEffectiveGravity = effectiveUpGravity;
                        Gizmos.color = colorUp;
                    }
                }
                
                currentVy += currentEffectiveGravity * dt;

                nextPos.x += currentVx * dt;
                nextPos.y += currentVy * dt;

                Gizmos.DrawLine(currentPos, nextPos);
                
                currentPos = nextPos;
                timeElapsed += dt;

                if (currentPos.y < transform.position.y - 4f)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawWireSphere(currentPos, 0.1f);
                    break;
                }
            }
        }
    }
}
