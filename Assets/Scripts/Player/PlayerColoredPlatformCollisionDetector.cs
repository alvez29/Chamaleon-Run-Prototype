using System;
using System.Collections.Generic;
using System.Linq;
using Game.Level;
using Game.Utils;
using UnityEngine;

namespace Game.Player
{
    public class PlayerColoredPlatformCollisionDetector : MonoBehaviour
    {
        public event Action OnPlayerCollidedPlatformWithIncorrectColor;
        
        [SerializeField] private PlayerColorHandler m_playerColorHandler;
        
        private Dictionary<int, string> m_platformsCache = new();

        private void OnEnable()
        {
            if (m_playerColorHandler != null) m_playerColorHandler.OnColorSwitched += OnPlayerChangedColor;
        }

        private void OnDisable()
        {
            if (m_playerColorHandler != null) m_playerColorHandler.OnColorSwitched -= OnPlayerChangedColor;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            PlatformColor currentColor = m_playerColorHandler?.CurrentColor ?? PlatformColor.Black;

            if (IsValidColor(currentColor, other.gameObject.tag) && m_playerColorHandler)
            {
                m_platformsCache.Add(other.gameObject.GetInstanceID(), other.gameObject.tag);
            }
            else
            {
                OnCollidedWithPlatformWithIncorrectColor();
            }
        }

        private static bool IsValidColor(PlatformColor currentColor, string platformTag)
        {
            switch (currentColor)
            {
                case PlatformColor.Blue when platformTag.Equals(Constants.BLUE_PLATFORM_TAG):
                case PlatformColor.Yellow when platformTag.Equals(Constants.YELLOW_PLATFORM_TAG):
                case PlatformColor.Black:
                    return true;
                default:
                    return false;
            }
        }

        private void OnPlayerChangedColor(PlatformColor currentColor)
        {
            if (m_platformsCache.Any(pair => !IsValidColor(currentColor, pair.Value)))
            {
                OnCollidedWithPlatformWithIncorrectColor();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var objectId = other.gameObject.GetInstanceID();

            m_platformsCache.Remove(objectId);
        }


        private void OnCollidedWithPlatformWithIncorrectColor()
        {
            Debug.Log("Boom!");
            OnPlayerCollidedPlatformWithIncorrectColor?.Invoke();
        }
    }
}