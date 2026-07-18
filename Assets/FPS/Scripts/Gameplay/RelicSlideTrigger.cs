using UnityEngine;
using Unity.FPS.Game;

namespace Unity.FPS.Gameplay
{
    public class RelicSlideTrigger : MonoBehaviour
    {
        private void Start()
        {
            EventManager.AddListener<PickupEvent>(OnPickupEvent);
        }

        private void OnPickupEvent(PickupEvent evt)
        {
            if (evt.Pickup.name.Contains("SacredRelic"))
            {
                PlayerSlideController slideCtrl = FindAnyObjectByType<PlayerSlideController>();
                if (slideCtrl != null)
                {
                    slideCtrl.StartSliding();
                }

                // Create the Escape Objective
                GameObject escapeObjectiveGo = new GameObject("Objective_EscapeVolcano");
                // Position at the bottom of the slide path: angle ~ -0.05 * PI
                float finalAngle = -0.05f * Mathf.PI;
                float finalR = 900f;
                float finalH = 15f;
                float finalX = 1000f + finalR * Mathf.Cos(finalAngle);
                float finalZ = 1000f + finalR * Mathf.Sin(finalAngle);
                
                escapeObjectiveGo.transform.position = new Vector3(finalX, finalH, finalZ);
                
                BoxCollider bc = escapeObjectiveGo.AddComponent<BoxCollider>();
                bc.isTrigger = true;
                bc.size = new Vector3(20f, 10f, 20f);
                
                ObjectiveReachPoint objective = escapeObjectiveGo.AddComponent<ObjectiveReachPoint>();
                objective.Title = "ESCAPE THE VOLCANO!";
                objective.Description = "Slide down to the rescue point at the base of the mountain before it's too late!";
                objective.IsOptional = false;
                objective.DelayVisible = 0f;
            }
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener<PickupEvent>(OnPickupEvent);
        }
    }
}