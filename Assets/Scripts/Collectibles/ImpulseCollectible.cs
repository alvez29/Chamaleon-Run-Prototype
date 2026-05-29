using Game.Player;
using UnityEngine;

namespace Game.Level.Collectibles
{
    public class ImpulseCollectible : Collectible
    {
        [SerializeField] private float m_horizontalForceStrength = 3000;
        [SerializeField] private float m_speedUpTime = 2f;
        [SerializeField] private float m_speedFactor = 1.5f;
        [SerializeField] private ParticleSystem m_impulseParticles;

        private PlayerMovement m_playerMovement;
        
        protected override void OnCollected(GameObject other)
        {
            if (other.TryGetComponent(out Rigidbody playerRigidBody))
            {
                playerRigidBody.AddForce(new Vector3(m_horizontalForceStrength, 0.0f, 0.0f), ForceMode.Force);
            }

            if (other.TryGetComponent(out PlayerMovement playerMovement))
            {
                m_playerMovement = playerMovement;
                playerMovement.StartSpeedUp(m_speedUpTime, m_speedFactor);
            }
        }

        protected internal override void Activate()
        {
            base.Activate();
            
            if (m_playerMovement)
            {
                m_playerMovement.StopSpeedUp();
                m_playerMovement = null;    
            }
            
            m_impulseParticles.Play();
        }

        protected internal override void Deactivate()
        {
            m_impulseParticles.Stop();
            
            base.Deactivate();
        }
    }
}