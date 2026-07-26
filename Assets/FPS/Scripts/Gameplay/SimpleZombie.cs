using UnityEngine;
using UnityEngine.AI;
using Unity.FPS.Game;

namespace Unity.FPS.Gameplay
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Health), typeof(AudioSource))]
    public class SimpleZombie : MonoBehaviour
    {
        [Header("Visuals")]
        public Sprite IdleSprite;
        public Sprite WalkSprite;
        public Sprite AttackSprite;
        public SpriteRenderer SpriteRenderer;

        [Header("Audio")]
        public AudioClip ShuffleSound;
        public AudioClip GroanSound;
        public float AudioInterval = 5f;

        [Header("Combat")]
        public float AttackRange = 1.5f;
        public float AttackDamage = 5f;
        public float AttackInterval = 1.5f;

        private NavMeshAgent m_Agent;
        private Health m_Health;
        private AudioSource m_AudioSource;
        private Transform m_PlayerTransform;
        private Health m_PlayerHealth;
        private float m_LastAttackTime;
        private float m_LastAudioTime;

        void Start()
        {
            m_Agent = GetComponent<NavMeshAgent>();
            m_Health = GetComponent<Health>();
            m_AudioSource = GetComponent<AudioSource>();
            
            if (SpriteRenderer == null)
                SpriteRenderer = GetComponentInChildren<SpriteRenderer>();

            GameObject player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                player = GameObject.Find("Player");
            }
            
            if (player)
            {
                m_PlayerTransform = player.transform;
                m_PlayerHealth = player.GetComponent<Health>();
            }

            m_Health.OnDie += OnDie;
            m_LastAudioTime = Time.time + Random.Range(0, AudioInterval);
        }

        void Update()
        {
            if (m_Health.CurrentHealth <= 0 || m_PlayerTransform == null)
            {
                if (m_Agent.enabled) m_Agent.isStopped = true;
                return;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, m_PlayerTransform.position);

            if (distanceToPlayer <= AttackRange)
            {
                Attack();
            }
            else
            {
                Move();
            }

            HandleRandomAudio();
        }

        void Move()
        {
            m_Agent.isStopped = false;
            m_Agent.SetDestination(m_PlayerTransform.position);
            
            if (SpriteRenderer)
                SpriteRenderer.sprite = WalkSprite;
        }

        void Attack()
        {
            m_Agent.isStopped = true;
            
            if (SpriteRenderer)
                SpriteRenderer.sprite = AttackSprite;

            if (Time.time - m_LastAttackTime >= AttackInterval)
            {
                m_LastAttackTime = Time.time;
                if (m_PlayerHealth)
                {
                    m_PlayerHealth.TakeDamage(AttackDamage, gameObject);
                }
                
                if (GroanSound)
                    m_AudioSource.PlayOneShot(GroanSound);
            }
        }

        void HandleRandomAudio()
        {
            if (Time.time - m_LastAudioTime >= AudioInterval)
            {
                m_LastAudioTime = Time.time;
                if (ShuffleSound)
                    m_AudioSource.PlayOneShot(ShuffleSound);
            }
        }

        void OnDie()
        {
            m_Agent.enabled = false;
            Destroy(gameObject, 2f);
        }
    }
}
