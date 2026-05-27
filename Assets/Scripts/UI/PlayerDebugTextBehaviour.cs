using UnityEngine;
using Game.Player;
using UnityEngine.UI;

namespace Game.UI
{
    public class PlayerDebugTextBehaviour : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Text m_debugText;
        
        [Header("Player References")]
        [SerializeField] private PlayerMovement m_playerMovement;
        [SerializeField] private PlayerColorHandler m_playerColorHandler;

        private void Update()
        {
            if (!m_debugText || !m_playerMovement || !m_playerColorHandler)
            {
                return;
            }

            string debugInfo = $"<b>Velocity X:</b> {m_playerMovement.Velocity.x:F2}\n" +
                               $"<b>Velocity Y:</b> {m_playerMovement.Velocity.y:F2}\n" +
                               $"<b>Current Color:</b> {m_playerColorHandler.CurrentColor}\n" +
                               $"<b>Is Jumping:</b> {m_playerMovement.IsJumping}\n" +
                               $"<b>Is Falling:</b> {m_playerMovement.IsFalling}\n" +
                               $"<b>Jumps Remaining:</b> {m_playerMovement.JumpsRemaining}\n" +
                               $"FPS {1/Time.deltaTime}\n";

            m_debugText.text = debugInfo;
        }
    }
}
