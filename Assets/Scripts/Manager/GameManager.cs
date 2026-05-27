using System;
using Game.Level;
using UnityEngine;

namespace Game.Manager
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager m_instance;
        public static GameManager Instance
        {
            get
            {
                if (m_instance != null) return m_instance;
                
                var go = new GameObject("GameManager");
                m_instance = go.AddComponent<GameManager>();
                DontDestroyOnLoad(go);
                return m_instance;
            }
        }

        private KillZoneBehaviour[] m_killzones;
        
        [SerializeField] private GameObject m_player;
        [SerializeField] private LevelManager m_currentLevel;
        
        [SerializeField] private int m_targetFps = 60;

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
            
            Application.targetFrameRate = m_targetFps;
            QualitySettings.vSyncCount = 0;
        }
    }
}