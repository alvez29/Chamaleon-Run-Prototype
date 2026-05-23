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

        [SerializeField] private int m_simulationSteps = 150;

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
            float currentVy = m_stats.InitialJumpForce;
            
            float dt = Time.fixedDeltaTime; 
            float timeElapsed = 0f;
            bool doubleJumpTriggered = false;

            Gizmos.color = colorUp;

            for (int i = 0; i < m_simulationSteps; i++)
            {
                Vector3 nextPos = currentPos;
                
                if (m_simulateDoubleJump && !doubleJumpTriggered && timeElapsed >= m_doubleJumpTime)
                {
                    currentVy = m_stats.DoubleJumpForce;
                    doubleJumpTriggered = true;
                    
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireSphere(currentPos, 0.2f);
                }

                float effectiveGravity = Physics.gravity.y;
                bool isFalling = currentVy < 0;
                bool isRisingWithSimpleJump = currentVy > 0 && simulateShortJump && !doubleJumpTriggered;
                bool isRising = currentVy > 0;
                
                if (isFalling)
                {
                    effectiveGravity *= m_stats.FallMultiplier;
                    Gizmos.color = Color.red;
                }
                else if (isRisingWithSimpleJump)
                {
                    effectiveGravity *= m_stats.LowJumpMultiplier;
                    Gizmos.color = colorUp;
                }
                else if (isRising)
                {
                    Gizmos.color = colorUp;
                }
                
                currentVy += effectiveGravity * dt;

                nextPos.x += currentVx * dt;
                nextPos.y += currentVy * dt;

                Gizmos.DrawLine(currentPos, nextPos);
                
                currentPos = nextPos;
                timeElapsed += dt;

                bool simulationHasExceeded = currentPos.y < transform.position.y - 4f;
                
                if (simulationHasExceeded)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawWireSphere(currentPos, 0.1f);
                    break;
                }
            }
        }
    }
}
