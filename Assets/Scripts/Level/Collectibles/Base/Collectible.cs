using Game.Utils;
using UnityEngine;

namespace Game.Level
{
    public abstract class Collectible : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.PLAYER_TAG))
            {
                OnCollected(other.gameObject);    
            }
            
        }

        protected abstract void OnCollected(GameObject player);
    }
}