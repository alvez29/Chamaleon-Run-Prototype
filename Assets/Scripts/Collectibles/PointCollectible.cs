using System;
using UnityEngine;

namespace Game.Level.Collectibles
{
    public class PointCollectible : Collectible
    {
        public event Action OnPointCollected;
        
        [SerializeField] private ParticleSystem m_pointParticles;
        [SerializeField] private MeshRenderer m_pointMesh;
        
        protected override void OnCollected(GameObject player)
        {
            OnPointCollected?.Invoke();
        }

        protected internal override void Deactivate()
        {
            m_pointParticles.Stop();
            m_pointMesh.enabled = false;
            
            base.Deactivate();
        }

        protected internal override void Activate()
        {
            base.Activate();
            
            m_pointParticles.Play();
            m_pointMesh.enabled = true;
        }
    }
}