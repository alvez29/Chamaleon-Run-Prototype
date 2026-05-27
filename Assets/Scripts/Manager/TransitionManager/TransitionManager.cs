using System;
using Game.Camera;
using Game.Manager.TransitionManager;
using UnityEngine;

namespace Game.Level
{
    public class TransitionManager : MonoBehaviour
    {
        public event Action OnFinishLevelTransitionFinished;
        public event Action OnResetLevelTransitionFinished;

        [Header("Components References")] [SerializeField]
        private Animator m_transitionCanvasAnimator;

        [SerializeField] private Animator m_cameraAnimator;
        [SerializeField] private CameraAnimationControllerSpeaker m_cameraAnimationControllerSpeaker;
        [SerializeField] private TransitionCanvasAnimatorSpeaker m_transitionCanvasAnimatorSpeaker;

        public void FadeOutInmmediatly()
        {
            m_transitionCanvasAnimator.Play("FadeOutImmediately");
        }
        
        public void FadeOut()
        {
            m_transitionCanvasAnimator.Play("FadeOut");
        }

        public void FadeIn()
        {
            m_transitionCanvasAnimator.Play("FadeIn");
        }

        public void SpinCamera()
        {
            m_cameraAnimator.Play("SpinAndZoomCamera");
        }

        public void PlayResetLevelTransition()
        {
            FadeOut();
            m_transitionCanvasAnimatorSpeaker.OnFadeOutCompleted += OnResetLevelTransitionFadeOutCompleted;
        }
        
        public void PlayFinishLevelTransition()
        {
            SpinCamera();
            m_cameraAnimationControllerSpeaker.OnShouldStartTransition += OnFinishLevelTransitionSpinCompleted;
        }

        private void OnResetLevelTransitionFadeOutCompleted()
        {
            m_transitionCanvasAnimatorSpeaker.OnFadeOutCompleted -= OnResetLevelTransitionFadeOutCompleted;
            OnResetLevelTransitionFinished?.Invoke();
        }
        
        private void OnFinishLevelTransitionSpinCompleted()
        {
            m_cameraAnimationControllerSpeaker.OnShouldStartTransition -= OnFinishLevelTransitionSpinCompleted;
            
            FadeOut();

            m_transitionCanvasAnimatorSpeaker.OnFadeOutCompleted += OnFinishLevelTransitionFadeOutCompleted;
            
        }

        private void OnFinishLevelTransitionFadeOutCompleted()
        {
            m_transitionCanvasAnimatorSpeaker.OnFadeOutCompleted -= OnFinishLevelTransitionFadeOutCompleted;
            OnFinishLevelTransitionFinished?.Invoke();
        }
    }
}