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
                m_Health.MaxHealth = 500f;
                m_Health.CurrentHealth = 500f;
                m_Health.OnDie += OnBossDeath;
            }

            // Scale up the boss to make it look giant and epic!
            transform.localScale = new Vector3(5f, 5f, 5f);
        }

        private void OnBossDeath()
        {
            Debug.Log("Dragon Boss defeated! Spawning relic.");
            if (RelicPrefab != null)
            {
                GameObject relic = Instantiate(RelicPrefab, RelicSpawnPosition, Quaternion.identity);
                relic.name = "SacredRelic_Objective";
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