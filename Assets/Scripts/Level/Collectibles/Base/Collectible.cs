using System;
using Game.Utils;
using UnityEngine;

namespace Game.Level
{
    public abstract class Collectible : MonoBehaviour
    {
        public event Action<Collectible> OnCollectibleCollected;

        private const int PLAYER_LAYER = 3;
        
        private MeshRenderer m_meshRenderer;
        private bool m_isActivated = true;

        private void Awake()
        {
            m_meshRenderer = GetComponentInChildren<MeshRenderer>();

            Activate();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (m_isActivated && ((1 << other.gameObject.layer) & PLAYER_LAYER) == 0)
            {
                OnCollected(other.gameObject);
                OnCollectibleCollected?.Invoke(this);
                Deactivate();
            }
            
        }

        protected abstract void OnCollected(GameObject player);

        protected internal virtual void Deactivate()
        {
            m_isActivated = false;
            m_meshRenderer.enabled = false;
        }

        protected internal virtual void Activate()
        {
            m_isActivated = true;
            m_meshRenderer.enabled = true;
        }
    }
}