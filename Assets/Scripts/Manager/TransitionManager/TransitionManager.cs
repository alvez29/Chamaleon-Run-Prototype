using UnityEngine;

namespace Game.Level
{
    public class TransitionManager : MonoBehaviour
    {
        
        [Header("Fade")]
        [SerializeField] private Animator m_transitionAnimator;

        public void FadeOut()
        {
            m_transitionAnimator.Play("FadeOut");
        }

        public void FadeIn()
        {
            m_transitionAnimator.Play("FadeIn");
        }
    }
}