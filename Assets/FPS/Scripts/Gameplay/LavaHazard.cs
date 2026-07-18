using UnityEngine;
using Unity.FPS.Game;

namespace Unity.FPS.Gameplay
{
    public class LavaHazard : MonoBehaviour
    {
        public float DamagePerSecond = 30f;

        private void OnTriggerStay(Collider other)
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(DamagePerSecond * Time.deltaTime, gameObject);
            }
        }
    }
}