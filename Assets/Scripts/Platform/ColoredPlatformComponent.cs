using Game.Level.Data;
using Game.Utils;
using UnityEngine;

namespace Game.Level
{
    public class ColoredPlatformComponent : MonoBehaviour
    {
        [SerializeField] private PlatformData m_platformData;
        [SerializeField] private PlatformColor m_platformColor;
        [SerializeField] private Collider m_collider;
        
        private void Start()
        {
            // TODO: Change also visuals from platform data
            SetTagByColor(gameObject, m_platformColor);
            SetTagByColor(m_collider.gameObject, m_platformColor);
        }

        private void SetTagByColor(GameObject subject, PlatformColor color)
        {
            switch (color)
            {
                case PlatformColor.Blue:
                    subject.tag = Constants.BLUE_PLATFORM_TAG;
                    break;
                case PlatformColor.Yellow:
                    subject.tag = Constants.YELLOW_PLATFORM_TAG;
                    break;
                case PlatformColor.Neutral:
                default:
                    return;
            }
        }
    }
}
