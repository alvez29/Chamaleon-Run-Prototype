using UnityEngine;

namespace Game.Manager.TransitionManager
{
    public class TransitionCanvasAnimatorSpeaker : MonoBehaviour
    {
        public event System.Action OnFadeOutCompleted;
        public event System.Action OnFadeInCompleted;

        public void OnFadeOutAnimationCompleted()
        {
            OnFadeOutCompleted?.Invoke();
        }
    
        public void OnFadeInAnimationCompleted()
        {
            OnFadeInCompleted?.Invoke();
        }
    }
}
