using UnityEngine;
using Unity.FPS.Game;

namespace Unity.FPS.Gameplay
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class FallingRelic : MonoBehaviour
    {
        public float InitialFallSpeed = 30f;

        private Rigidbody m_Rigidbody;
        private Collider m_Collider;
        private Pickup m_Pickup;
        private bool m_IsGrounded = false;
        private float m_TargetGroundY;

        private void Awake()
        {
            m_Pickup = GetComponent<Pickup>();
            if (m_Pickup != null)
            {
                m_Pickup.enabled = false; // Disable pickup bobbing and trigger logic while falling
            }

            m_Rigidbody = GetComponent<Rigidbody>();
            m_Collider = GetComponent<Collider>();

            if (m_Rigidbody != null)
            {
                m_Rigidbody.isKinematic = false;
                m_Rigidbody.useGravity = true;
                m_Rigidbody.linearVelocity = new Vector3(0, -InitialFallSpeed, 0);
            }

            if (m_Collider != null)
            {
                m_Collider.isTrigger = false; // Physical collision so it falls and lands on the terrain
            }
        }

        private void Start()
        {
            CheckAndLand();
        }

        private void FixedUpdate()
        {
            if (m_IsGrounded) return;
            CheckAndLand();
        }

        private void Update()
        {
            if (m_IsGrounded) return;

            // Apply downward movement if physics hasn't moved it enough
            if (!m_IsGrounded && m_Rigidbody != null)
            {
                m_Rigidbody.linearVelocity = new Vector3(0, Mathf.Min(m_Rigidbody.linearVelocity.y, -InitialFallSpeed), 0);
            }

            CheckAndLand();
        }

        private void CheckAndLand()
        {
            if (m_IsGrounded) return;

            // Raycast down to find ground Y dynamically
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1000f))
            {
                m_TargetGroundY = hit.point.y;
            }

            float boundsOffset = m_Collider != null ? m_Collider.bounds.extents.y : 1f;
            if (transform.position.y - boundsOffset <= m_TargetGroundY + 0.5f)
            {
                LandOnGround(new Vector3(transform.position.x, m_TargetGroundY + boundsOffset, transform.position.z));
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (m_IsGrounded) return;

            float boundsOffset = m_Collider != null ? m_Collider.bounds.extents.y : 1f;
            Vector3 landPos = collision.contacts.Length > 0 ? collision.contacts[0].point + Vector3.up * boundsOffset : transform.position;
            LandOnGround(landPos);
        }

        private void LandOnGround(Vector3 landPosition)
        {
            if (m_IsGrounded) return;
            m_IsGrounded = true;

            transform.position = landPosition;

            if (m_Rigidbody != null)
            {
                m_Rigidbody.isKinematic = true;
                m_Rigidbody.linearVelocity = Vector3.zero;
            }

            if (m_Collider != null)
            {
                m_Collider.isTrigger = true;
            }

            if (m_Pickup != null)
            {
                m_Pickup.enabled = true; // Enables pickup logic at the landed ground position
            }
        }
    }
}

