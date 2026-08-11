using UnityEngine;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;

namespace Unity.FPS.Gameplay
{
    public class DragonBossController : MonoBehaviour
    {
        public GameObject RelicPrefab;
        public Vector3 RelicSpawnPosition = new Vector3(1000f, 410f, 1000f);

        private Health m_Health;

        private void Start()
        {
            m_Health = GetComponent<Health>();
            if (m_Health != null)
            {
                m_Health.MaxHealth = 2000f;
                m_Health.CurrentHealth = 2000f;
                m_Health.OnDie += OnBossDeath;
            }

            // Scale up the boss to make it look giant and epic, filling at least half the caldera!
            transform.localScale = new Vector3(100f, 100f, 100f);
        }

        private void OnBossDeath()
        {
            Debug.Log("Dragon Boss defeated! Spawning relic.");
            if (RelicPrefab != null)
            {
                // Spawn relic at the dragon's position so it falls down to the surface.
                GameObject relic = Instantiate(RelicPrefab, transform.position, Quaternion.identity);
                relic.name = "SacredRelic_Objective";

                // Make the relic big enough for the player to clearly see across the crater.
                relic.transform.localScale = new Vector3(30f, 30f, 30f);

                // Attach FallingRelic component so it falls to the ground surface and activates pickup logic
                FallingRelic falling = relic.GetComponent<FallingRelic>();
                if (falling == null)
                {
                    falling = relic.AddComponent<FallingRelic>();
                }

                relic.SetActive(true);

                // Re-wire ObjectivePickupItem
                ObjectivePickupItem opi = relic.GetComponent<ObjectivePickupItem>();
                if (opi != null)
                {
                    opi.ItemToPickup = relic;
                    opi.Title = "Recover the Sacred Relic";
                    opi.Description = "Recover the dragon's relic from the center of the volcano crater.";
                }
            }
        }
    }
}