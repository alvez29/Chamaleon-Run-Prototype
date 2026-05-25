using System.Collections;
using Game.Player;
using Game.Player.Data;
using UnityEngine;

namespace Game.Level.Collectibles
{
    public class ImpulseCollectible : Collectible
    {
        [SerializeField] private float m_horizontalForceStrength = 3000;
        [SerializeField] private float m_speedUpTime = 2f;
        [SerializeField] private float m_speedFactor = 1.5f;

        private PlayerMovement m_playerMovement;
        
        protected override void OnCollected(GameObject other)
        {
            if (other.TryGetComponent(out Rigidbody playerRigidBody))
            {
                playerRigidBody.AddForce(new Vector3(m_horizontalForceStrength, 0.0f, 0.0f), ForceMode.Force);
            }

            if (other.TryGetComponent(out PlayerMovement playerMovement))
            {
                playerMovement.StartSpeedUp(m_speedUpTime, m_speedFactor);
            }
        }

        protected internal override void Activate()
        {
            if (m_playerMovement)
            {
                m_playerMovement.StopSpeedUp();
                m_playerMovement = null;    
            }
            base.Activate();
        }
    }
}