using UnityEngine;

namespace Game.Level.Data
{
    [CreateAssetMenu(fileName = "New Platform Data", menuName = "Game/Level/Platforms", order = 0)]
    public class PlatformData : ScriptableObject
    {
        public GameObject platformVisual;
    }
}