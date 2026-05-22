using System;
using Game.Player.Data;
using UnityEngine;

namespace Game.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerStats m_stats;
        [SerializeField] private PlayerInputHandler m_inputHandler;
        [SerializeField] private PlayerGroundDetector m_groundDetector;
        [SerializeField] private bool m_autoRun;
        
        private Rigidbody m_playerBody;
        private bool m_canDoubleJump;
        private bool m_isJumpCanceled;

        private void Awake()
        {
            m_playerBody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            if (m_inputHandler == null) return;
            
            m_inputHandler.OnJumpStarted += HandleJumpStarted;
            m_inputHandler.OnJumpCanceled += HandleJumpCanceled;
        }

        private void OnDisable()
        {
            if (m_inputHandler == null) return;
            
            m_inputHandler.OnJumpStarted -= HandleJumpStarted;
            m_inputHandler.OnJumpCanceled -= HandleJumpCanceled;
        }

        private void FixedUpdate()
        {
            if (!m_stats) return;
            
            if (m_autoRun) m_playerBody.velocity = new Vector3(m_stats.RunSpeed, m_playerBody.velocity.y, .0f);

            switch (m_playerBody.velocity.y)
            {
                case < 0:
                    HandleFallingUpdate(m_playerBody, m_stats);
                    break;
                case > 0 when m_isJumpCanceled:
                    HandleJumpingUpdate(m_playerBody, m_stats);
                    break;
            }
            
        }

        private static void HandleFallingUpdate(Rigidbody body, PlayerStats stats)
        {
            body.velocity += Vector3.up * (Physics.gravity.y * (stats.FallMultiplier - 1f) * Time.fixedDeltaTime);
        }

        private static void HandleJumpingUpdate(Rigidbody body, PlayerStats stats)
        {
            body.velocity += Vector3.up * (Physics.gravity.y * (stats.LowJumpMultiplier - 1f) * Time.fixedDeltaTime);
        }
        
        private void HandleJumpStarted()
        {
            if (!m_groundDetector || !m_stats) return;
            
            if (m_groundDetector.IsGrounded)
            {
                m_playerBody.velocity = new Vector3(m_playerBody.velocity.x, m_stats.InitialJumpForce, .0f);
                m_canDoubleJump = true;
                m_isJumpCanceled = false;
            }
            else if (m_canDoubleJump)
            {
                m_playerBody.velocity = new Vector3(m_playerBody.velocity.x, m_stats.DoubleJumpForce, .0f);
                m_canDoubleJump = false;
                m_isJumpCanceled = false;
            }
        }

        private void HandleJumpCanceled()
        {
            m_isJumpCanceled = true;
        }
    }
}
