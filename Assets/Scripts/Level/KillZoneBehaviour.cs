using System;
using UnityEngine;

namespace Game.Level
{
    public class KillZoneBehaviour : MonoBehaviour
    {
        public event Action OnPlayerEnteredKillZone;
        
        private void OnTriggerEnter(Collider other)
        {
            OnPlayerEnteredKillZone?.Invoke();
        }
    }
}