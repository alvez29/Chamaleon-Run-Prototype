using System;
using UnityEngine;

namespace Game.Level.Collectibles
{
    public class WinCollectible : Collectible
    {
        [SerializeField] private ParticleSystem winCollectibleParticles;
        
        public event Action OnWinCollectibleCollected;
        
        protected override void OnCollected(GameObject player)
        {
            winCollectibleParticles.Play();
            OnWinCollectibleCollected?.Invoke();
        }
    }
}