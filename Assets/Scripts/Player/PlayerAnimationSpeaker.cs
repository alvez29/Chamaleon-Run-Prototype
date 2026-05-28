using UnityEngine;

namespace Game.Player
{
    public class PlayerAnimationSpeaker : MonoBehaviour
    {
        [SerializeField] private PlayerAudioOrchestrator m_playerAudioOrchestrator;

        private void OnStep()
        {
            m_playerAudioOrchestrator.PlayStepSound();
        }
    }
}