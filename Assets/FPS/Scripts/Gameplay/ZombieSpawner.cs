using UnityEngine;
using UnityEngine.AI;
using Unity.FPS.Game;
using System.Collections.Generic;

namespace Unity.FPS.Gameplay
{
    public class ZombieSpawner : MonoBehaviour
    {
        [Tooltip("The zombie prefab to spawn")]
        public GameObject ZombiePrefab;

        [Tooltip("Maximum number of zombies to have in the scene")]
        public int MaxZombies = 20;

        [Tooltip("Minimum distance from the player to spawn zombies")]
        public float MinDistanceFromPlayer = 20f;

        [Tooltip("Maximum distance from the player to spawn zombies")]
        public float MaxDistanceFromPlayer = 100f;

        [Tooltip("How often to check if more zombies should be spawned")]
        public float SpawnCheckInterval = 5f;

        private Transform m_PlayerTransform;
        private List<GameObject> m_SpawnedZombies = new List<GameObject>();
        private float m_LastCheckTime;

        void Start()
        {
            var player = GameObject.FindWithTag("Player");
            if (player)
            {
                m_PlayerTransform = player.transform;
            }
            else
            {
                // Fallback: look for object named "Player"
                var playerObj = GameObject.Find("Player");
                if (playerObj) m_PlayerTransform = playerObj.transform;
            }

            SpawnInitialZombies();
        }

        void Update()
        {
            if (Time.time - m_LastCheckTime > SpawnCheckInterval)
            {
                m_LastCheckTime = Time.time;
                CleanupDeadZombies();
                if (m_SpawnedZombies.Count < MaxZombies)
                {
                    TrySpawnZombie();
                }
            }
        }

        void SpawnInitialZombies()
        {
            for (int i = 0; i < MaxZombies; i++)
            {
                TrySpawnZombie();
            }
        }

        void TrySpawnZombie()
        {
            if (ZombiePrefab == null) return;

            Vector3 center = m_PlayerTransform ? m_PlayerTransform.position : transform.position;
            
            // Attempt to find a valid spot
            for (int i = 0; i < 10; i++)
            {
                Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(MinDistanceFromPlayer, MaxDistanceFromPlayer);
                Vector3 randomPos = center + new Vector3(randomCircle.x, 0, randomCircle.y);

                if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 10f, NavMesh.AllAreas))
                {
                    // Check distance from player again to be sure
                    if (m_PlayerTransform && Vector3.Distance(hit.position, m_PlayerTransform.position) < MinDistanceFromPlayer)
                        continue;

                    GameObject zombie = Instantiate(ZombiePrefab, hit.position, Quaternion.identity);
                    m_SpawnedZombies.Add(zombie);
                    return;
                }
            }
        }

        void CleanupDeadZombies()
        {
            m_SpawnedZombies.RemoveAll(z => z == null);
        }
    }
}
