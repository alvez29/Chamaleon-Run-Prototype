using Game.Player;
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
        
        [SerializeField] private GameObject m_player;
        private PlayerColorHandler m_playerColorHandler;
        
        public PlayerColorHandler PlayerColorHandler => m_playerColorHandler;
    }
}