using UnityEngine;

namespace Game.Level.Collectibles
{
    public class ImpulseCollectible : Collectible
    {
        [SerializeField] private float m_horizontalForceStrength = 3000;

        protected override void OnCollected(GameObject other)
        {
            if (other.TryGetComponent(out Rigidbody playerRigidBody))
            {
                playerRigidBody.AddForce(new Vector3(m_horizontalForceStrength, 0.0f, 0.0f), ForceMode.Force);
            }
        }
    }
}