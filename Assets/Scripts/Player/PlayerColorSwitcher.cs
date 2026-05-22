using System;
using Level;
using UnityEngine;

namespace Game.Player
{
    public class PlayerColorSwitcher : MonoBehaviour
    {
        public event Action<PlatformColor> OnColorSwitched; 
        
        [SerializeField] PlayerInputHandler m_playerInputHandler;
        [SerializeField] Renderer[] m_visuals;
        
        [SerializeField] Material m_blueMaterial;
        [SerializeField] Material m_yellowMaterial;
        
        public bool isYellow;
        public PlatformColor CurrentColor => isYellow ? PlatformColor.Yellow : PlatformColor.Blue;
        
        private void Awake()
        {
            UpdateMaterial(m_visuals, CurrentColor);
        }

        private void OnEnable()
        {
            if (m_playerInputHandler == null) return;

            m_playerInputHandler.OnSwitchColorStarted += SwitchColor;
        }

        private void OnDisable()
        {
            if (m_playerInputHandler == null) return;

            m_playerInputHandler.OnSwitchColorStarted -= SwitchColor;
        }

        public void SetColor(PlatformColor color)
        {
            if (color == PlatformColor.Yellow)
                isYellow = true;
            else if (color == PlatformColor.Blue)
                isYellow = false;
            else return;
            
            UpdateMaterial(m_visuals, CurrentColor);
        }
        
        private void SwitchColor()
        {
            isYellow = !isYellow;
            UpdateMaterial(m_visuals, CurrentColor);
        }
        
        private void UpdateMaterial(Renderer[] visuals, PlatformColor color)
        {
            Material materialToApply = isYellow ? m_yellowMaterial : m_blueMaterial;
            
            foreach (Renderer model in visuals)
            {
                model.material = materialToApply;
            } 
            
            OnColorSwitched?.Invoke(color);
        }
        
    }
}
