using System;
using System.Collections;
using System.Linq;
using Game.Player.Data;
using Game.Utils;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        enum BouncePreventionMode
        {
            Simple,
            None,
        }
        
        [SerializeField] private PlayerStats m_stats;
        [SerializeField] private PlayerInputHandler m_inputHandler;
        [SerializeField] private PlayerGroundDetector m_groundDetector;
        [SerializeField] private bool m_autoRun;
        [SerializeField] private BouncePreventionMode m_bouncePreventionMode = BouncePreventionMode.Simple;
        
        private Rigidbody m_playerBody;
        private Collider m_playerBodyCollider;
        private int m_playerBodyColliderId;
        private int m_jumpsRemaining;
        private bool m_isJumpCanceled;

        private void Awake()
        {
            m_playerBody = GetComponent<Rigidbody>();
            m_playerBodyCollider =  GetComponent<Collider>();
            
            m_playerBodyCollider.hasModifiableContacts = true;
            m_playerBodyCollider.providesContacts = true;
            m_playerBodyColliderId = m_playerBodyCollider.GetInstanceID();
            
            // since assets are modular we need this fix to prevent GhostBumpsCollision
            Physics.ContactModifyEventCCD += PreventGhostBumpCCD;
            Physics.ContactModifyEvent += PreventGhostBumpCCD;

            m_groundDetector.OnGroundDetected += OnGroundDetectorDetectedGround;
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
        
        private void OnDestroy()
        {
            Physics.ContactModifyEventCCD -= PreventGhostBumpCCD;
            Physics.ContactModifyEvent -= PreventGhostBumpCCD;
        }

        private void FixedUpdate()
        {
            if (!m_stats) return;

            m_groundDetector.CheckGround();
            
            if (m_autoRun)
            {
                ProcessAutoRun(m_playerBody, m_stats.RunSpeed, m_stats.RunAcceleration);
            }

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
            if (!m_stats) return;
            
            if (m_jumpsRemaining > 0)
            {
                float jumpForce = m_jumpsRemaining == Constants.MAX_PLAYER_JUMPS ? m_stats.InitialJumpForce : m_stats.DoubleJumpForce;
                
                m_playerBody.velocity = new Vector3(m_playerBody.velocity.x, 0f, m_playerBody.velocity.z);
                m_playerBody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
                
                m_jumpsRemaining--;
                m_isJumpCanceled = false;
            }
        }

        private void HandleJumpCanceled()
        {
            m_isJumpCanceled = true;
        }

        private void OnGroundDetectorDetectedGround()
        {
            if (m_jumpsRemaining < Constants.MAX_PLAYER_JUMPS) ResetJumps();
        }

        private void ResetJumps()
        {
            m_jumpsRemaining = Constants.MAX_PLAYER_JUMPS;
        }

        private void ProcessAutoRun(Rigidbody playerBody, float runSpeed, float runAcceleration)
        {
            float speedDifference = runSpeed - playerBody.velocity.x;
            
            m_playerBody.AddForce(Vector3.right * (speedDifference * runAcceleration), ForceMode.Acceleration);
        }
        
        private void PreventGhostBumpCCD(PhysicsScene physicsScene, NativeArray<ModifiableContactPair> contactPairs)
        {
            switch (m_bouncePreventionMode)
            {
                case BouncePreventionMode.Simple:
                    ModifiableContactPair[] ballContactPairs =
                        contactPairs.Where(pair => pair.colliderInstanceID == m_playerBodyColliderId).ToArray();
                    SimpleBouncePrevention(ballContactPairs);
                    break;


                case BouncePreventionMode.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void SimpleBouncePrevention(ModifiableContactPair[] ballContactPairs)
        {
            foreach (ModifiableContactPair pair in ballContactPairs)
            {
                for (int i = 0; i < pair.contactCount; i++)
                {
                    if (pair.GetSeparation(i) > 0)
                    {
                        pair.IgnoreContact(i);
                    }
                }
            }
        }
    }
}
