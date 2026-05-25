using System.Collections;
using UnityEngine;

namespace Game.Camera
{
    public class ShakerComponent : MonoBehaviour
    {
        private Vector3 m_originalLocalPosition;
        private Coroutine m_shakeCoroutine;
        
        public void Shake(float intensity = 0.5f, float duration = 0.15f)
        {
            if (m_shakeCoroutine != null)
            {
                StopCoroutine(m_shakeCoroutine);
                transform.localPosition = m_originalLocalPosition;
            }

            m_shakeCoroutine = StartCoroutine(ShakeRoutine(intensity, duration));
        }

        private IEnumerator ShakeRoutine(float intensity, float duration)
        {
            m_originalLocalPosition = transform.localPosition;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float normalizedTime = elapsed / duration;
                float damping = 1f - normalizedTime;
                damping *= damping; 

                float x = Random.Range(-1f, 1f) * intensity * damping;
                float y = Random.Range(-1f, 1f) * intensity * damping;

                transform.localPosition = m_originalLocalPosition + new Vector3(x, y, 0f);

                elapsed += Time.unscaledDeltaTime; 
                yield return null;
            }

            transform.localPosition = m_originalLocalPosition;
            m_shakeCoroutine = null;
        }
    }
}