using Game.Player.Data;
using UnityEngine;

namespace Game.Player
{
    public class PlayerJumpDebugger : MonoBehaviour
    {
        [SerializeField] private PlayerStats m_stats;
        
        [Header("Simulation Settings")]
        [Tooltip("Activa esto para simular un doble salto en ambas trayectorias")]
        [SerializeField] private bool m_simulateDoubleJump;
        
        [Tooltip("En qué segundo del salto se pulsa el doble salto")]
        [Range(0f, 1f)]
        [SerializeField] private float m_doubleJumpTime = 0.35f;

        private void OnDrawGizmos()
        {
            if (m_stats == null) return;

            // 1. Salto Corto (suelta el botón nada más saltar, usa FallGravityFactor mientras sube)
            DrawTrajectory(simulateShortJump: true, colorUp: Color.yellow);

            // 2. Salto Largo (deja pulsado el botón hasta arriba, usa JumpGravityFactor mientras sube)
            DrawTrajectory(simulateShortJump: false, colorUp: Color.green);
        }

        private void DrawTrajectory(bool simulateShortJump, Color colorUp)
        {
            Vector3 currentPos = transform.position;
            float currentVx = m_stats.RunSpeed;
            
            // Comprobamos si el jugador tiene Use Gravity activado en el Rigidbody
            Rigidbody rb = GetComponent<Rigidbody>();
            float unityGravity = (rb != null && rb.useGravity) ? Physics.gravity.y : 0f;
            
            // Gravedades efectivas
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

                // Lógica de gravedad de tu ProcessGravity
                float currentEffectiveGravity;
                
                if (currentVy < 0) // isFalling
                {
                    currentEffectiveGravity = effectiveDownGravity;
                    Gizmos.color = Color.red;
                }
                else // isJumping
                {
                    if (simulateShortJump && !doubleJumpTriggered)
                    {
                        // Si soltamos el botón (m_isJumpCanceled o !IsJumpPressed), usamos la gravedad de caída
                        currentEffectiveGravity = effectiveDownGravity;
                        Gizmos.color = colorUp; // Amarillo
                    }
                    else
                    {
                        // Si dejamos pulsado, usamos la gravedad de salto
                        currentEffectiveGravity = effectiveUpGravity;
                        Gizmos.color = colorUp; // Verde
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
