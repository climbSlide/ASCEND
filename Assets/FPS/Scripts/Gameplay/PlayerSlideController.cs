using UnityEngine;
using Unity.FPS.Game;

namespace Unity.FPS.Gameplay
{
    public class PlayerSlideController : MonoBehaviour
    {
        public bool IsSliding { get; private set; } = false;
        
        [Header("Sliding Parameters")]
        public float SlideForwardSpeed = 35f;
        public float SteeringSpeed = 15f;
        public float JumpForce = 12f;
        
        private PlayerCharacterController m_PlayerController;
        private PlayerInputHandler m_InputHandler;
        private Health m_Health;

        private void Start()
        {
            m_PlayerController = GetComponent<PlayerCharacterController>();
            m_InputHandler = GetComponent<PlayerInputHandler>();
            m_Health = GetComponent<Health>();
        }

        public void StartSliding()
        {
            if (IsSliding) return;
            IsSliding = true;
            Debug.Log("Player started sliding down the volcano!");
            
            // Broadcast Escape banner
            DisplayMessageEvent displayMessage = Events.DisplayMessageEvent;
            displayMessage.Message = "ERUPTION IMMINENT! SLIDE TO ESCAPE!";
            displayMessage.DelayBeforeDisplay = 0f;
            EventManager.Broadcast(displayMessage);
        }

        private void Update()
        {
            if (!IsSliding) return;

            // Forward Velocity: Slide down the volcano (roughly towards the center or a target angle)
            // For simplicity in this demo, we'll assume sliding is along the 'Slide Path' carved in Step 1.
            // The slide path in Step 1 was opposite to the start, roughly -PI to 0 angle.
            // Let's use the local forward but restricted to horizontal + gravity.
            
            Vector3 slideVelocity = transform.forward * SlideForwardSpeed;
            
            // Steering
            float horizontalInput = m_InputHandler.GetLookInputsHorizontal() + (m_InputHandler.GetMoveInput().x * 2f);
            Vector3 steering = transform.right * horizontalInput * SteeringSpeed;
            
            // Apply to character controller velocity
            Vector3 finalVelocity = slideVelocity + steering;
            finalVelocity.y = m_PlayerController.CharacterVelocity.y; // Keep gravity from main controller
            
            m_PlayerController.CharacterVelocity = finalVelocity;

            // Jumping over lava gaps
            if (m_PlayerController.IsGrounded && m_InputHandler.GetJumpInputDown())
            {
                m_PlayerController.CharacterVelocity += Vector3.up * JumpForce;
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (!IsSliding) return;

            // Damage player if they hit obstacles (not the ground)
            if (hit.gameObject.CompareTag("Obstacle") || (hit.normal.y < 0.5f && hit.gameObject.layer == 0))
            {
                if (m_Health != null && m_PlayerController.CharacterVelocity.magnitude > 10f)
                {
                    m_Health.TakeDamage(20f, hit.gameObject);
                    Debug.Log("Hit obstacle while sliding! Damage taken.");
                }
            }
        }
    }
}