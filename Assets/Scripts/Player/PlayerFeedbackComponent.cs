using Game.Camera;
using Game.Level;
using Game.Utils;
using UnityEngine;

namespace Game.Player
{
    public class PlayerFeedbackComponent : MonoBehaviour
    {
        [Header("Components References")]
        [SerializeField] private ShakerComponent m_cameraShake;
        [SerializeField] private LevelManager m_levelManager;
        [SerializeField] private ParticleSystem m_colorDustParticles;
        [SerializeField] private ParticleSystem m_whiteDustParticles;
        [SerializeField] private ParticleSystem m_cubeParticles;
        [SerializeField] private ParticleSystem m_jumpParticles;
        [SerializeField] private ParticleSystem m_doubleJumpParticles;
        [SerializeField] private ParticleSystem m_changeColorParticles;
        [SerializeField] private TrailRenderer m_windTrail;
        
        [SerializeField] private PlayerGroundDetector m_groundDetector;
        [SerializeField] private PlayerMovement m_playerMovement;
        [SerializeField] private PlayerColorHandler m_playerColorHandler;

        [Header("Settings")]
        [SerializeField] private float m_playerDeathCameraShakeIntensity = 0.5f; 
        [SerializeField] private float m_playerDeathCameraShakeDuration = 0.2f;
        [SerializeField] private float m_cubeParticlesLandVelocityThreshold = -24f;
        
        private Color m_blueColor;
        private Color m_yellowColor;
        
        private void Awake()
        {
            ColorUtility.TryParseHtmlString(Constants.BLUE_COLOR_HEX, out var blueColor);
            ColorUtility.TryParseHtmlString(Constants.YELLOW_COLOR_HEX, out var yellowColor);
            
            m_blueColor = blueColor;
            m_yellowColor = yellowColor;
            
            m_levelManager.OnPlayerJustDied += () =>
            {
                PlayCubeParticles(m_playerColorHandler.CurrentColor);
                m_whiteDustParticles.Play();
                m_cameraShake.Shake(m_playerDeathCameraShakeIntensity, m_playerDeathCameraShakeDuration);
                m_windTrail.enabled = false;
            };

            m_levelManager.OnLevelStarted += () =>
            {
                m_windTrail.enabled = true;
            };

            if (!m_groundDetector) m_groundDetector = GetComponent<PlayerGroundDetector>();
            if (!m_playerMovement) m_playerMovement = GetComponent<PlayerMovement>();
            if (!m_playerColorHandler) m_playerColorHandler = GetComponent<PlayerColorHandler>();

            if (m_playerColorHandler)
            {
                m_playerColorHandler.OnColorSwitched += newColor =>
                {
                    ModifyParticlesColor(m_colorDustParticles, newColor);
                    ModifyParticlesColor(m_changeColorParticles, newColor);
                    m_changeColorParticles.Play();
                };
            }

            if (m_groundDetector != null)
            {
                m_groundDetector.OnGroundDetected += () =>
                {
                    if (m_playerMovement.Velocity.y < m_cubeParticlesLandVelocityThreshold)
                    {
                        PlayCubeParticles(m_playerColorHandler.CurrentColor);    
                    }
                    
                };
            }
            
            if (m_playerMovement != null)
            {
                m_playerMovement.OnJumpExecuted += () =>
                {
                    m_jumpParticles.Play();
                };

                m_playerMovement.OnDoubleJumpExecuted += () =>
                {
                    m_doubleJumpParticles.Play();
                };
            }
        }

        private void FixedUpdate()
        {
            bool shouldEnableColorDustParticles = m_groundDetector.IsGrounded || m_playerMovement.IsJumping;

            if (shouldEnableColorDustParticles && !m_colorDustParticles.isPlaying)
            {
                m_colorDustParticles.Play();
            }
            else if (!shouldEnableColorDustParticles && m_colorDustParticles.isPlaying)
            {
                m_colorDustParticles.Stop();
            }
        }

        private void PlayCubeParticles(PlatformColor platformColor)
        {
            ModifyParticlesColor(m_cubeParticles, platformColor);
            m_cubeParticles.Play(true);
        }

        private void ModifyParticlesColor(ParticleSystem particle, PlatformColor platformColor)
        {
            ParticleSystem.MainModule mainModule = particle.main;
            mainModule.startColor = platformColor == PlatformColor.Blue ? m_blueColor : m_yellowColor;
        }
    }
}