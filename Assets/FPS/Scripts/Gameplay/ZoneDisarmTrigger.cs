using UnityEngine;
using Unity.FPS.Gameplay;
using Unity.FPS.Game;

namespace Unity.FPS.Gameplay
{
    public class ZoneDisarmTrigger : MonoBehaviour
    {
        private bool m_HasTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (m_HasTriggered) return;

            PlayerCharacterController playerCtrl = other.GetComponent<PlayerCharacterController>();
            if (playerCtrl != null)
            {
                PlayerWeaponsManager weaponsManager = other.GetComponent<PlayerWeaponsManager>();
                if (weaponsManager != null)
                {
                    m_HasTriggered = true;

                    // Remove all weapons
                    for (int i = 0; i < 9; i++)
                    {
                        WeaponController weapon = weaponsManager.GetWeaponAtSlotIndex(i);
                        if (weapon != null)
                        {
                            weaponsManager.RemoveWeapon(weapon);
                        }
                    }

                    Debug.Log("Player disarmed successfully. Weapons inventory cleared.");

                    // Broadcast EMP Disarm banner event
                    DisplayMessageEvent displayMessage = Events.DisplayMessageEvent;
                    displayMessage.Message = "LOST IT ALL! Volcano EMP has vaporized your weapons! Find weapons in the ruins!";
                    displayMessage.DelayBeforeDisplay = 0f;
                    EventManager.Broadcast(displayMessage);
                }
            }
        }
    }
}