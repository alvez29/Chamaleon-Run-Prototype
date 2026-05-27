using System.Collections.Generic;
using UnityEngine;

namespace Game.Level.Platforms
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovingPlatformComponent : MonoBehaviour
    {
        public enum MovementType
        {
            PingPong,
            Loop
        }

        [Header("Path Settings")]
        [SerializeField] private List<Transform> m_waypoints;
        [SerializeField] private float m_speed = 5f;
        [SerializeField] private MovementType m_movementType = MovementType.PingPong;
        [SerializeField] private float m_waitTimeAtWaypoint = 0.5f;

        private Rigidbody m_rigidbody;
        private int m_currentWaypointIndex = 0;
        private bool m_movingForward = true;
        private float m_waitTimer = 0f;

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            m_rigidbody.isKinematic = true; 
            m_rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

            if (m_waypoints.Count > 0 && m_waypoints[0] != null)
            {
                m_rigidbody.position = m_waypoints[0].position;
            }
        }

        private void FixedUpdate()
        {
            if (m_waypoints == null || m_waypoints.Count < 2) return;

            if (m_waitTimer > 0f)
            {
                m_waitTimer -= Time.fixedDeltaTime;
                return;
            }

            Transform targetWaypoint = m_waypoints[m_currentWaypointIndex];

            if (targetWaypoint)
            {
                Vector3 newPosition = Vector3.MoveTowards(m_rigidbody.position, targetWaypoint.position, m_speed * Time.fixedDeltaTime);
                m_rigidbody.MovePosition(newPosition);

                if (Vector3.Distance(m_rigidbody.position, targetWaypoint.position) < 0.01f)
                {
                    m_waitTimer = m_waitTimeAtWaypoint;
                    UpdateWaypointIndex();
                }    
            }
        }

        public void ResetPosition()
        {
            m_rigidbody.position = m_waypoints[0].position;
        }

        private void UpdateWaypointIndex()
        {
            if (m_movementType == MovementType.PingPong)
            {
                if (m_movingForward)
                {
                    m_currentWaypointIndex++;
                    
                    if (m_currentWaypointIndex >= m_waypoints.Count)
                    {
                        m_currentWaypointIndex = m_waypoints.Count - 2;
                        m_movingForward = false;
                    }
                }
                else
                {
                    m_currentWaypointIndex--;
                    
                    if (m_currentWaypointIndex < 0)
                    {
                        m_currentWaypointIndex = 1;
                        m_movingForward = true;
                    }
                }
            }
            else
            {
                m_currentWaypointIndex = (m_currentWaypointIndex + 1) % m_waypoints.Count;
            }
        }

        private void OnDrawGizmos()
        {
            if (m_waypoints == null || m_waypoints.Count < 2) return;

            Gizmos.color = Color.green;
            for (int i = 0; i < m_waypoints.Count; i++)
            {
                if (m_waypoints[i] != null)
                {
                    Gizmos.DrawWireSphere(m_waypoints[i].position, 0.3f);
                    
                    if (i < m_waypoints.Count - 1 && m_waypoints[i + 1] != null)
                    {
                        Gizmos.DrawLine(m_waypoints[i].position, m_waypoints[i + 1].position);
                    }
                }
            }

            if (m_movementType == MovementType.Loop && m_waypoints[0] != null && m_waypoints[m_waypoints.Count - 1] != null)
            {
                Gizmos.DrawLine(m_waypoints[m_waypoints.Count - 1].position, m_waypoints[0].position);
            }
        }
    }
}
