using UnityEngine;

namespace Game.Level.Collectibles
{
    public class ImpulseCollectible : Collectible
    {
        [SerializeField] private float m_horizontalForceStrength = 2;
        
        protected override void OnCollected(GameObject other)
        {
            other.TryGetComponent(out Rigidbody playerRigidBody);

            if (playerRigidBody != null)
            {
                playerRigidBody.AddForce(new Vector3(m_horizontalForceStrength, 0.0f, 0.0f), ForceMode.Force);    
            }
            
        }
    }
}