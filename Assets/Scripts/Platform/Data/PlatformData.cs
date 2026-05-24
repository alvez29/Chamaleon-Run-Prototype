using UnityEngine;

namespace Game.Level.Data
{
    [CreateAssetMenu(fileName = "New Platform Data", menuName = "Game/Level/Platform", order = 0)]
    public class PlatformData : ScriptableObject
    {
        public enum PlatformType
        {
            Squared,
            Slope,
            Barrier,
            NonRegistered,
        }
        
        [SerializeField] 
        public PlatformColor Color = PlatformColor.Black;
        
        [SerializeField] 
        public GameObject PlatformPrefab;
        
        [SerializeField]
        public Vector3Int Dimensions = Vector3Int.one;
        
        [SerializeField] 
        public bool UseMeshCollider;
        
        [SerializeField]
        public PlatformType Type = PlatformType.Squared;
    }
}