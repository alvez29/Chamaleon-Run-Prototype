using Game.Level;
using Game.Manager;
using UnityEngine;

namespace Game.Level
{
    public class LevelAudioOrchestrator : MonoBehaviour
    {
        [SerializeField] private LevelManager m_levelManager;
        
        [SerializeField] private AudioClip m_loseSound;

        private void Awake()
        {
            if (m_levelManager)
            {
                m_levelManager.OnPlayerJustDied += () =>
                {
                    AudioManager.Instance.PlaySFX(m_loseSound);
                };
                
                m_levelManager.OnLevelWon += () =>
                {
                    AudioManager.Instance.StopAllSounds();
                };
                
                m_levelManager.OnLevelStarted += () =>
                {
                    AudioManager.Instance.RestoreSoundVolume();
                }; 
            }
        }
    }
}