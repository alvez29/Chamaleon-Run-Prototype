using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace Game.Manager
{
    
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager m_instance;
        public static AudioManager Instance
        {
            get
            {
                if (m_instance != null) return m_instance;
                
                var go = new GameObject("AudioManager");
                m_instance = go.AddComponent<AudioManager>();
                DontDestroyOnLoad(go);
                return m_instance;
            }
        }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource m_sfxSource;
        [SerializeField] private AudioSource m_windSource;
        [SerializeField] private AudioSource m_stepsSource;

        private Coroutine m_fadeInWindCoroutine;
        
        private void Awake()
        {
            if (m_instance != null && m_instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            m_instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);

            if (m_sfxSource == null)
            {
                m_sfxSource = gameObject.AddComponent<AudioSource>();
            }

            if (m_windSource == null)
            {
                m_windSource = gameObject.AddComponent<AudioSource>();
                m_windSource.playOnAwake = false;
            }

            if (m_stepsSource == null)
            {
                m_stepsSource = gameObject.AddComponent<AudioSource>();
            }
        }

        public void SetVFXVolume(float volume)
        {
            m_sfxSource.volume = volume;
        }
        
        public void PlaySFXWithRandomPitch(AudioClip clip, float volume = 1f, float minPitch = 0.8f, float maxPitch = 1.2f)
        {
            if (clip == null) return;
            
            m_sfxSource.pitch = Random.Range(minPitch, maxPitch);
            m_sfxSource.PlayOneShot(clip, volume);
        }

        public void PlaySFX(AudioClip clip, float volume = 1f)
        {
            if (clip == null) return;
            
            m_sfxSource.pitch = 1f;
            m_sfxSource.PlayOneShot(clip, volume);
        }

        public void PlayWindSound(AudioClip clip, float volume = 0.05f)
        {
            m_windSource.loop = true;
            m_windSource.clip = clip;
            m_windSource.volume = volume;
            
            m_fadeInWindCoroutine = StartCoroutine(FadeInAudio(m_windSource, 0.8f, 0.03f));
        }

        public void PlayStep(AudioClip clip, float volume = 0.2f, float minPitch = 0.8f, float maxPitch = 1.2f)
        {
            if (clip == null) return;
            
            m_stepsSource.pitch = Random.Range(minPitch, maxPitch);
            m_stepsSource.PlayOneShot(clip, volume);
        }

        public void StopAllSounds()
        {
            StartCoroutine(FadeOutAudio(m_sfxSource, 0.5f));
            StartCoroutine(FadeOutAudio(m_windSource, 0.5f));
            StartCoroutine(FadeOutAudio(m_stepsSource, 0.5f));
        }

        public void RestoreSoundVolume()
        {
            m_sfxSource.volume = 1.0f;
            m_windSource.volume = 1.0f;
            m_stepsSource.volume = 1.0f;
        }
        
        public void StopWindSound()
        {
            if (m_fadeInWindCoroutine != null)
            {
                StopCoroutine(m_fadeInWindCoroutine);
                m_fadeInWindCoroutine = null;
            }
            
            StartCoroutine(FadeOutAudio(m_windSource, 0.2f));
        }
        
        private IEnumerator FadeInAudio(AudioSource audioSource, float duration, float targetVolume = 1f)
        {
            audioSource.volume = 0f;
            audioSource.Play();
    
            float currentTime = 0f;
            
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(0f, targetVolume, currentTime / duration);
                yield return null;
            }
            
            audioSource.volume = targetVolume;
        }

        private IEnumerator FadeOutAudio(AudioSource audioSource, float duration)
        {
            float startVolume = audioSource.volume;
            float currentTime = 0f;
            
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, 0f, currentTime / duration);
                yield return null;
            }
    
            audioSource.volume = 0f;
            audioSource.Stop();
        }
    }
}