using UnityEngine;

namespace Game.Level.Data
{
    [CreateAssetMenu(fileName = "New Platform Data", menuName = "Game/Level/Platform", order = 0)]
    public class PlatformData : ScriptableObject
    {
        [SerializeField] 
        public PlatformColor Color = PlatformColor.Black;
        
        [SerializeField] 
        public GameObject PlatformPrefab;
        
        [SerializeField] 
        public bool UseMeshCollider;
    }
}