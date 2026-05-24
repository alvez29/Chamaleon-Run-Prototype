using System;
using UnityEngine;

namespace Game.Level.Collectibles
{
    public class WinCollectible : Collectible
    {
        public event Action OnWinCollectibleCollected;
        
        protected override void OnCollected(GameObject player)
        {
            OnWinCollectibleCollected?.Invoke();
        }
    }
}