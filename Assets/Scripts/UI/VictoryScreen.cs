using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class VictoryScreen : MonoBehaviour
    {
        public event Action OnCountdownFinished;
        
        [SerializeField] private TextMeshProUGUI m_collectibleMessage;
        [SerializeField] private TextMeshProUGUI m_loadingMessage;
        [SerializeField] private Animator m_animator;
        [SerializeField] private float m_loadingMessageAnimationRate = 0.3f;
        [SerializeField] private float m_waitTime = 5f;

        private WaitForSeconds m_animationDelay;

        private void Awake()
        {
            m_animator.Play("FadeInVictoryScreen");
            m_animationDelay = new WaitForSeconds(m_loadingMessageAnimationRate);
            StartCoroutine(LoadingScreenAnimation());
            StartCoroutine(CountdownToNextLevel(m_waitTime));
        }

        public void ShowCollectibleMessage(int currentCollectibleNumber, int maxCollectibles)
        {
            m_collectibleMessage.gameObject.SetActive(true);
            m_collectibleMessage.text =
                string.Format(m_collectibleMessage.text, currentCollectibleNumber, maxCollectibles);
        }

        private void OnFadeOut()
        {
            OnCountdownFinished?.Invoke();
        }
        
        private IEnumerator LoadingScreenAnimation()
        {
            string[] frames = { "", ".", "..", "..." };
            string originalText = m_loadingMessage.text;
            int index = 0;

            while (true)
            {
                m_loadingMessage.text = originalText + frames[index];
                index = (index + 1) % frames.Length;
                yield return m_animationDelay;
            }
        }

        private IEnumerator CountdownToNextLevel(float time)
        {
            yield return new WaitForSeconds(time);
            m_animator.Play("FadeOutVictoryScreen");
        }
    }
}