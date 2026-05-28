using UnityEngine;

namespace Game.Player
{
    public class PlayerAnimationUpdater : MonoBehaviour
    {
        [SerializeField] private Animator m_animator;

        private readonly int m_verticalSpeed = Animator.StringToHash("VerticalSpeed");
        private readonly int m_isInGround = Animator.StringToHash("IsInGround");
        private readonly int m_jumpsRemaining = Animator.StringToHash("JumpsRemaining");
        
        public void UpdateParameters(Vector3 playerVelocity, bool isInGround, int jumpsRemaining)
        {
            m_animator.SetFloat(m_verticalSpeed, playerVelocity.y);
            m_animator.SetBool(m_isInGround, isInGround);
            m_animator.SetInteger(m_jumpsRemaining, jumpsRemaining);
        }
    }
}