using System;
using Game.Utils;
using UnityEngine;

namespace Game.Level
{
    public abstract class Collectible : MonoBehaviour
    {
        public event Action OnCollectibleCollected;

        private const int PLAYER_LAYER = 3;
        
        [SerializeField] private ParticleSystem onCollectedParticles; 
        
        private bool m_isActivated = true;

        private void Awake()
        {
            Activate();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (m_isActivated && ((1 << other.gameObject.layer) & PLAYER_LAYER) == 0)
            {
                OnCollected(other.gameObject);
                onCollectedParticles.Play();
                OnCollectibleCollected?.Invoke();
                Deactivate();
            }
            
        }

        protected abstract void OnCollected(GameObject player);

        protected internal virtual void Deactivate()
        {
            m_isActivated = false;
        }

        protected internal virtual void Activate()
        {
            m_isActivated = true;
        }
    }
}