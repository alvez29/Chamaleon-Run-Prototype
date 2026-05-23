using System;
using UnityEngine;

namespace Game.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] 
        private Transform m_player;
        
        [SerializeField] 
        private CameraData m_cameraData;
        
        private Vector3 m_velocity = Vector3.zero;

        private void Awake()
        {
            transform.position = m_player.position + m_cameraData.Offset;
            transform.rotation = Quaternion.Euler(0f, m_cameraData.RotationInDegrees, 0f);
        }

        private void LateUpdate()
        {
            if (!m_player || !m_cameraData)
            {
                return;
            }

            Vector3 targetPosition = m_player.position + m_cameraData.Offset;
            Vector3 currentPosition = transform.position;
            
            float newX = Mathf.SmoothDamp(currentPosition.x, targetPosition.x, ref m_velocity.x, m_cameraData.SmoothTime.x);
            float newY = Mathf.SmoothDamp(currentPosition.y, targetPosition.y, ref m_velocity.y, m_cameraData.SmoothTime.y);

            transform.position = new Vector3(newX, newY, targetPosition.z);
        }
    }
}