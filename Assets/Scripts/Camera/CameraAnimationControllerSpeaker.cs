using System;
using UnityEngine;

namespace Game.Camera
{
    public class CameraAnimationControllerSpeaker: MonoBehaviour
    {
        public event Action OnShouldStartTransition;

        public void OnShouldStartTransitionCall()
        {
            OnShouldStartTransition?.Invoke();
        }
        
    }
}