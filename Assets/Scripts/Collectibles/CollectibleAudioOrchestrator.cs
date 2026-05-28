using System;
using Game.Manager;
using UnityEngine;

namespace Game.Level.Collectibles
{
    public class CollectibleAudioOrchestrator : MonoBehaviour
    {
        [SerializeField] private Collectible m_collectible;

        [SerializeField] private AudioClip m_onCollectedSound ;
        
        private void Awake()
        {
            if (m_collectible)
            {
                m_collectible.OnCollectibleCollected += () => AudioManager.Instance.PlaySFX(m_onCollectedSound);
            }
        }
    }
}