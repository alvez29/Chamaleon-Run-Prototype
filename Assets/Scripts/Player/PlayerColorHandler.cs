using System;
using Game.Level;
using UnityEngine;

namespace Game.Player
{
    public class PlayerColorHandler : MonoBehaviour
    {
        public event Action<PlatformColor> OnColorSwitched; 
        
        [SerializeField] private PlayerInputHandler m_playerInputHandler;
        [SerializeField] private Renderer[] m_visuals;
        
        [SerializeField] private Material m_blueMaterial;
        [SerializeField] private Material m_yellowMaterial;
        
        public bool m_isYellow;
        public PlatformColor CurrentColor => m_isYellow ? PlatformColor.Yellow : PlatformColor.Blue;
        
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
            m_isYellow = color == PlatformColor.Yellow;
            UpdateMaterial(m_visuals, CurrentColor);
        }
        
        private void SwitchColor()
        {
            m_isYellow = !m_isYellow;
            UpdateMaterial(m_visuals, CurrentColor);
        }
        
        private void UpdateMaterial(Renderer[] visuals, PlatformColor color)
        {
            Material materialToApply = color == PlatformColor.Yellow ? m_yellowMaterial : m_blueMaterial;
            
            foreach (Renderer model in visuals)
            {
                model.material = materialToApply;
            } 
            
            OnColorSwitched?.Invoke(color);
        }
        
    }
}
