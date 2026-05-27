using System;
using System.Collections;
using System.Linq;
using Game.Core;
using Game.Level.Collectibles;
using Game.Manager;
using Game.Player;
using UnityEngine;

namespace Game.Level
{
    public class LevelManager : MonoBehaviour
    {
        public event Action OnPlayerJustDied;
        public event Action OnLevelStarted;
        
        [Header("Components References")]
        [SerializeField] private SceneReference m_nextLevel;
        [SerializeField] private GameObject m_player;
        [SerializeField] private GameObject m_playerVisual;
        [SerializeField] private Transform m_playerStartPoint;
        [SerializeField] private SceneManager m_sceneManager;
        
        [SerializeField] private PlatformColor m_initialPlatformColor = PlatformColor.Blue;
        [SerializeField] private TransitionManager m_transitionManager;
        
        [Header("Settings")]
        [SerializeField] private float m_loseTimeScale = 0.2f;

        private Collectible[] m_collectedCollectiblesCache = Array.Empty<Collectible>();
        private Coroutine m_timeScaleCoroutine;
        private bool m_hasLost;
        
        private PlayerInputHandler m_playerInputHandler;
        private PlayerMovement m_playerMovement;
        private PlayerColorHandler m_playerColorHandler;
        private PlayerColoredPlatformCollisionDetector m_playerCollisionDetector;
        
        private void Awake()
        {
            m_transitionManager.FadeOutInmmediatly();
            m_transitionManager.FadeIn();
            
            KillZoneBehaviour[] killZones = FindObjectsOfType<KillZoneBehaviour>();
            WinCollectible[] winCollectibles= FindObjectsOfType<WinCollectible>();
            Collectible[] collectibles = FindObjectsOfType<Collectible>();

            if (m_nextLevel != null && m_sceneManager != null)
            {
                m_sceneManager.PreloadScene(m_nextLevel);
            }
            
            if (m_player != null)
            {
                if (m_player.TryGetComponent(out PlayerColoredPlatformCollisionDetector collisionDetector))
                {
                    m_playerCollisionDetector = collisionDetector;
                    collisionDetector.OnPlayerCollidedPlatformWithIncorrectColor += OnLose;
                }
                else
                {
                    PlayerColoredPlatformCollisionDetector childrenCollisionDetector =
                        m_player.GetComponentInChildren<PlayerColoredPlatformCollisionDetector>();

                    if (childrenCollisionDetector != null)
                    {
                        m_playerCollisionDetector = childrenCollisionDetector;
                        childrenCollisionDetector.OnPlayerCollidedPlatformWithIncorrectColor += OnLose;
                    }
                }

                if (m_player.TryGetComponent(out PlayerColorHandler playerColorHandler))
                {
                    m_playerColorHandler = playerColorHandler;
                }

                if (m_player.TryGetComponent(out PlayerInputHandler playerInputHandler))
                {
                    m_playerInputHandler = playerInputHandler;
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
                ReactivateAllCollectibles(m_collectedCollectiblesCache);
            }
        }

        private void Start()
        {
            StartLevel();
        }

        private void ReactivateAllCollectibles(Collectible[] collectibles)
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

        private void OnResetLevelTransitionFinished()
        {
            m_transitionManager.FadeIn();
            m_transitionManager.OnResetLevelTransitionFinished -= OnResetLevelTransitionFinished;
            StartLevel();
        }
        
        private void StartLevel()
        {
            if (Time.timeScale < 1f && m_timeScaleCoroutine != null)
            {
                StopCoroutine(m_timeScaleCoroutine);
            }

            Time.timeScale = 1f;
            m_playerMovement.EnableAutoRun();
            m_playerVisual.SetActive(true);
            m_playerCollisionDetector.ResetCacheAndCollisions();
            ReactivateAllCollectibles(m_collectedCollectiblesCache);
            m_playerColorHandler.SetColor(m_initialPlatformColor);
            TeleportPlayerToStart();
            m_playerInputHandler.EnableAllInputs();
            m_hasLost = false;
            OnLevelStarted?.Invoke();
        }
        
        private void OnLose()
        {
            Debug.Log("[Level Manager] Lose");
            m_hasLost = true;
            OnPlayerJustDied?.Invoke();
            m_playerVisual.SetActive(false);
            m_playerInputHandler.DisableAllInputs();
            
            if (m_timeScaleCoroutine != null) m_timeScaleCoroutine = null;
            
            m_timeScaleCoroutine = StartCoroutine(ChangeTimeScale(Time.timeScale, m_loseTimeScale));
            m_transitionManager.PlayResetLevelTransition();
            m_transitionManager.OnResetLevelTransitionFinished += OnResetLevelTransitionFinished;
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
            if (!m_hasLost)
            {
                m_playerInputHandler.DisableAllInputs();
                m_playerMovement.StopAutoRun();
                m_transitionManager.PlayFinishLevelTransition();
                m_transitionManager.OnFinishLevelTransitionFinished += OnFinishLevelTransitionFinished;    
            }
        }

        private void OnFinishLevelTransitionFinished()
        {
            Debug.Log("[Level Manager] Win");
            
            if (m_sceneManager!= null && !m_sceneManager.ActivatePreloadedScene())
            {
                if (m_nextLevel != null)
                {
                    m_sceneManager.LoadScene(m_nextLevel);
                }
                else
                {
                    Debug.Log("[Level Manager] Level could not be loaded");
                }
            }
        }
    }
}