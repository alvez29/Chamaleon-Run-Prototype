using System;
using System.Collections;
using UnityEngine;

namespace Game.Manager
{
    public class SceneManager : MonoBehaviour
    {
        public event Action OnScenePreloaded;
        public event Action OnSceneLoaded;

        private AsyncOperation m_currentLoadOperation;

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneRoutine(sceneName, true));
        }
        
        public void PreloadScene(string sceneName)
        {
            StartCoroutine(LoadSceneRoutine(sceneName, false));
        }
        
        public bool ActivatePreloadedScene()
        {
            if (m_currentLoadOperation is { allowSceneActivation: false })
            {
                m_currentLoadOperation.allowSceneActivation = true;
                return true;
            }

            return false;
        }

        private IEnumerator LoadSceneRoutine(string sceneName, bool activateImmediately)
        {
            yield return null; 
            
            m_currentLoadOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

            if (m_currentLoadOperation != null)
            {
                m_currentLoadOperation.allowSceneActivation = activateImmediately;

                if (!activateImmediately)
                {
                    OnScenePreloaded?.Invoke();

                    yield return new WaitUntil(() => m_currentLoadOperation.allowSceneActivation);
                }

                while (!m_currentLoadOperation.isDone)
                {
                    yield return null;
                }
            }

            m_currentLoadOperation = null;
            OnSceneLoaded?.Invoke();
        }
    }
}
