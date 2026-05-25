using UnityEngine;

namespace Game.Player
{
    public class PlayerAnimationUpdater : MonoBehaviour
    {
        [SerializeField] private Animator m_animator;

        public void UpdateParameters(Vector3 playerVelocity, bool isInGround, int jumpsRemaining)
        {
            m_animator.SetFloat("VerticalSpeed", playerVelocity.y);
            m_animator.SetBool("IsInGround", isInGround);
            m_animator.SetInteger("JumpsRemaining", jumpsRemaining);
        }
    }
}