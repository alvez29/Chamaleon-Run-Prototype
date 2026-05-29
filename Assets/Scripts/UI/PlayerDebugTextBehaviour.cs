using System.Text;
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

        private readonly StringBuilder m_debugInfoBuilder = new();

        private void Update()
        {
            if (!m_debugText || !m_playerMovement || !m_playerColorHandler)
            {
                return;
            }

            m_debugInfoBuilder.Clear();
            m_debugInfoBuilder
                .AppendLine($"<b>Velocity X:</b> {m_playerMovement.Velocity.x:F2}")
                .AppendLine($"<b>Velocity Y:</b> {m_playerMovement.Velocity.y:F2}")
                .AppendLine($"<b>Current Color:</b> {m_playerColorHandler.CurrentColor}")
                .AppendLine($"<b>Is Jumping:</b> {m_playerMovement.IsJumping}")
                .AppendLine($"<b>Is Falling:</b> {m_playerMovement.IsFalling}")
                .AppendLine($"<b>Jumps Remaining:</b> {m_playerMovement.JumpsRemaining}")
                .AppendLine($"<b>FPS:</b> {1f / Time.unscaledDeltaTime:F0}")
                .AppendLine($"<b>Physics Update:</b> {1f / Time.fixedDeltaTime:F0}");

            m_debugText.text = m_debugInfoBuilder.ToString();
        }
    }
}
