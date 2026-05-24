using System;
using System.Collections;
using System.Linq;
using Game.Level.Collectibles;
using Game.Manager.TransitionManager;
using Game.Player;
using UnityEngine;

namespace Game.Level
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Components References")]
        [SerializeField] private GameObject m_nextLevelPrefab;
        [SerializeField] private GameObject m_player;
        [SerializeField] private Transform m_playerStartPoint;
        [SerializeField] private PlayerMovement m_playerMovement;
        [SerializeField] private PlatformColor m_initialPlatformColor = PlatformColor.Blue;
        [SerializeField] private TransitionManager m_transitionManager;
        [SerializeField] private TransitionManagerAnimatorSpeaker m_transitionManagerAnimatorSpeaker;
        
        [Header("Settings")]
        [SerializeField] private float m_loseTimeScale = 0.2f;

        private Collectible[] m_collectedCollectiblesCache = Array.Empty<Collectible>();
        
        private PlayerColorHandler m_playerColorHandler;
        
        private void Awake()
        {
            KillZoneBehaviour[] killZones = FindObjectsOfType<KillZoneBehaviour>();
            WinCollectible[] winCollectibles= FindObjectsOfType<WinCollectible>();
            Collectible[] collectibles = FindObjectsOfType<Collectible>();

            if (m_player != null)
            {
                if (m_player.TryGetComponent(out PlayerColoredPlatformCollisionDetector collisionDetector))
                {
                    collisionDetector.OnPlayerCollidedPlatformWithIncorrectColor += OnLose;
                }
                else
                {
                    PlayerColoredPlatformCollisionDetector childrenCollisionDetector =
                        m_player.GetComponentInChildren<PlayerColoredPlatformCollisionDetector>();

                    if (childrenCollisionDetector != null)
                        childrenCollisionDetector.OnPlayerCollidedPlatformWithIncorrectColor += OnLose;
                }

                if (m_player.TryGetComponent(out PlayerColorHandler playerColorHandler))
                {
                    m_playerColorHandler = playerColorHandler;
                }

                if (m_player.TryGetComponent(out PlayerMovement playerMovement))
                {
                    m_playerMovement = playerMovement;
                }
            }

            foreach (KillZoneBehaviour killzone in killZones)
            {
                killzone.OnPlayerEnteredKillZone += OnLose;
            }

            foreach (WinCollectible winCollectible in winCollectibles)
            {
                winCollectible.OnWinCollectibleCollected += OnWin;
            }

            foreach (Collectible collectible in collectibles)
            {
                collectible.OnCollectibleCollected += OnCollectibleCollected;
                ReactivateCollectibles(m_collectedCollectiblesCache);
            }
        }

        private void Start()
        {
            StartLevel();
        }

        private void ReactivateCollectibles(Collectible[] collectibles)
        {
            foreach (Collectible collectible in collectibles)
            {
                collectible.Activate();
            }

            m_collectedCollectiblesCache = Array.Empty<Collectible>();
        }

        private void OnCollectibleCollected(Collectible collectible)
        {
            m_collectedCollectiblesCache = m_collectedCollectiblesCache.Append(collectible).ToArray();
        }
        
        private void TeleportPlayerToStart()
        {
            if (m_player == null || m_playerStartPoint == null) return;

            if (m_player.TryGetComponent(out Rigidbody playerBody))
            {
                playerBody.position = m_playerStartPoint.position;
                playerBody.velocity = Vector3.zero;
                playerBody.angularVelocity = Vector3.zero;
            }
        }

        private void OnFadeOutFinished()
        {
            m_transitionManager.FadeIn();
            m_transitionManagerAnimatorSpeaker.OnFadeOutCompleted -= OnFadeOutFinished;
            StartLevel();
        }
        
        private void StartLevel()
        {
            if (Time.timeScale < 1f) StartCoroutine(ChangeTimeScale(Time.timeScale, 1f));
            ReactivateCollectibles(m_collectedCollectiblesCache);
            m_playerColorHandler.SetColor(m_initialPlatformColor);
            TeleportPlayerToStart();
            Time.timeScale = 1f;
        }
        
        private void OnLose()
        {
            Debug.Log("Lose");

            StartCoroutine(ChangeTimeScale(Time.timeScale, m_loseTimeScale));
            m_transitionManager.FadeOut();
            m_transitionManagerAnimatorSpeaker.OnFadeOutCompleted += OnFadeOutFinished;
        }

        IEnumerator ChangeTimeScale(float originTimeScale, float targetTimeScale, float transitionDuration = 0.2f)
        {
            float timeElapsed = 0f;
            
            while (timeElapsed < transitionDuration)
            {
                timeElapsed += Time.deltaTime;
                Time.timeScale = Mathf.Clamp(originTimeScale, targetTimeScale, timeElapsed / transitionDuration);
                yield return null;
            }
            
            Time.timeScale = targetTimeScale;
        }   

        private void OnWin()
        {
            Debug.Log("Win");
        }
    }
}