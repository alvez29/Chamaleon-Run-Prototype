using System;
using Game.Level;
using Game.Manager;
using UnityEngine;

namespace Game.Player
{
    public class PlayerAudioOrchestrator : MonoBehaviour
    {
        [SerializeField] private AudioClip m_stepSound;
        [SerializeField] private AudioClip m_jumpSound;
        [SerializeField] private AudioClip m_windSound;
        [SerializeField] private AudioClip m_colorSwitchSound;
        [SerializeField] private AudioClip m_onLandedSound;
        
        [SerializeField] private PlayerMovement m_playerMovement;
        [SerializeField] private PlayerGroundDetector m_groundDetector;
        [SerializeField] private PlayerColorHandler m_colorHandler;

        private void Awake()
        {
            if (m_playerMovement)
            {
                m_playerMovement.OnJumpExecuted += () => AudioManager.Instance.PlaySFX(m_jumpSound);
                m_playerMovement.OnDoubleJumpExecuted += () => AudioManager.Instance.PlaySFX(m_jumpSound);;
            }

            if (m_groundDetector)
            {
                m_groundDetector.OnGroundDetected += () =>
                {
                    AudioManager.Instance.PlaySFX(m_onLandedSound);
                    AudioManager.Instance.StopWindSound();
                };
                m_groundDetector.OnGroundLost += () => AudioManager.Instance.PlayWindSound(m_windSound);
            }

            if (m_colorHandler)
            {
                m_colorHandler.OnColorSwitched += (actualColor, shouldPlaySound) =>
                {
                    if (shouldPlaySound)
                    {
                        float pitch = actualColor == PlatformColor.Blue ? 1.4f : 0.6f;
                        AudioManager.Instance.PlaySFXWithRandomPitch(m_colorSwitchSound, 1f, pitch, pitch);    
                    }
                };
            }
        }

        public void PlayStepSound()
        {
            AudioManager.Instance.PlayStep(m_stepSound, 0.3f);
        }
    }
}