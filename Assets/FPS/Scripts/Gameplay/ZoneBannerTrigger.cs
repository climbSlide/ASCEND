using UnityEngine;
using Unity.FPS.Game;

namespace Unity.FPS.Gameplay
{
    public class ZoneBannerTrigger : MonoBehaviour
    {
        public string Message = "New Zone Entered";
        private bool m_HasTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (m_HasTriggered) return;
            if (other.GetComponent<PlayerCharacterController>() != null)
            {
                m_HasTriggered = true;
                DisplayMessageEvent displayMessage = Events.DisplayMessageEvent;
                displayMessage.Message = Message;
                displayMessage.DelayBeforeDisplay = 0f;
                EventManager.Broadcast(displayMessage);
            }
        }
    }
}