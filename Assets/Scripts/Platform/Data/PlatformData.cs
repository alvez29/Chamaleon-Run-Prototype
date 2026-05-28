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

        [SerializeField] private PlatformColor m_color = PlatformColor.Black;

        [SerializeField] private GameObject m_platformPrefab;

        [SerializeField] private Vector3Int m_dimensions = Vector3Int.one;

        [SerializeField] private bool m_useMeshCollider;

        [SerializeField] private PlatformType m_type = PlatformType.Squared;
        
        public PlatformColor Color => m_color;

        public GameObject PlatformPrefab => m_platformPrefab;

        public Vector3Int Dimensions => m_dimensions;

        public bool UseMeshCollider => m_useMeshCollider;

        public PlatformType Type => m_type;
    }
}