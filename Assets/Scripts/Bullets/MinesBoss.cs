using System.Collections;
using GD.MinMaxSlider;
using UnityEngine;
using Random = UnityEngine.Random;
using Utils;
namespace RiverAttack
{
    public class MinesBoss: Bullets
    {
        const float LERP = 5.0f;
        [SerializeField] AudioEventSample alertMineAudio;
        [SerializeField] GameObject deadParticlePrefab;
        [SerializeField] float deadTimeOutExplosion;
        [SerializeField] AudioEventSample enemyExplodeAudio;
        [SerializeField] GameObject explosionParticlePrefab;
        [SerializeField] float mineTimeOutExplosion;
        [SerializeField] AudioEventSample minesExplodeAudio;
        [SerializeField] float playerApproachRadius;
        [SerializeField, MinMaxSlider(0f, 20f)] Vector2 playerApproachRadiusRandom;
        public string onAlert;
        internal Vector3 moveDirection;
        
        float m_StartTime;
        bool m_StartExplosion;
        bool m_IsDestroy;
        PlayerDetectApproach m_PlayerDetectApproach;
        SphereCollider m_Collider;
        Transform m_Target;
        AudioSource m_AudioSource;
        Animator m_Animator;
        
        #region GizmoSettings
        [Header("Gizmo Settings")]
        public Color gizmoColor = new Color(255, 0, 0, 150);
        #endregion
        
        #region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
            Tools.ToggleChildren(transform);
            audioShoot.Play(m_AudioSource);
        }
        void Start()
        {
            SetInitialReferences();
        }
        void Update()
        {
            m_Target = m_PlayerDetectApproach.TargetApproach<PlayerMaster>(GameManager.instance.layerPlayer);
            if (m_Target == null || m_StartExplosion || m_IsDestroy)
                return;
            m_StartExplosion = true;
            AnimationAlert();
            Invoke(nameof(DetonationMine),2f);
        }
        void OnTriggerEnter(Collider collision)
        {
            if (!collision.GetComponentInParent<PlayerMaster>() && !collision.GetComponentInParent<EffectAreaMaster>()
                && (!collision.GetComponent<BulletPlayer>() || !collision.GetComponent<BulletPlayerBomb>()))
                return;
            DestroyMeExplosion();
        }
        void OnDisable()
        {
            m_StartExplosion = false;
            m_IsDestroy = false;
            m_Target = null;
            m_PlayerDetectApproach = null;
            Tools.ToggleChildren(transform);
        }
        #endregion
        
        void SetInitialReferences()
        {
            m_Animator = GetComponentInChildren<Animator>();
            m_AudioSource = GetComponent<AudioSource>();
            m_Collider = GetComponent<SphereCollider>();
            playerApproachRadius = SetPlayerApproachRadius();
        }
        

        internal void Initialization(Transform pool)
        {
            m_MyPool = pool;
            m_PlayerDetectApproach = new PlayerDetectApproach(transform.position, playerApproachRadius);
            
        }
        float randomRangeDetect
        {
            get { return Random.Range(playerApproachRadiusRandom.x, playerApproachRadiusRandom.y); }
        }
        float SetPlayerApproachRadius()
        {
            return playerApproachRadiusRandom != Vector2.zero ? randomRangeDetect : playerApproachRadius;
        }

        void DetonationMine()
        {
            if (m_IsDestroy) return;
            if (m_AudioSource != null && minesExplodeAudio != null)
            {
                minesExplodeAudio.Play(m_AudioSource);
            }
            StartCoroutine(nameof(ExpandCollider));
            Tools.ToggleChildren(transform, false);
            var transform1 = transform;
            var explosion = Instantiate(explosionParticlePrefab,transform1.position, transform1.rotation);
            Destroy(explosion, mineTimeOutExplosion);
            Invoke(nameof(DestroyMe), mineTimeOutExplosion);
            
        }
        void AnimationAlert()
        {
            if (m_Animator == null)
                m_Animator = GetComponentInChildren<Animator>();
            if (string.IsNullOrEmpty(onAlert)) 
                return;
            if (m_Animator == null) return;
            if (m_AudioSource != null && alertMineAudio != null)
                alertMineAudio.Play(m_AudioSource);
            m_Animator.SetTrigger(onAlert);
        }

        IEnumerator ExpandCollider()
        {
            float spendTime = 0.0f;
            if (m_Collider == null) yield break;
            float radioInitial = m_Collider.radius;

            while (spendTime < mineTimeOutExplosion)
            {
                float actualRadio = Mathf.Lerp(radioInitial, playerApproachRadius, spendTime / mineTimeOutExplosion);
                m_Collider.radius = actualRadio;
                spendTime += Time.deltaTime;
                yield return null;
            }
            m_Collider.radius = playerApproachRadius;
        }
        void DestroyMeExplosion()
        {
            m_IsDestroy = true;
            var transform1 = transform;
            if (m_AudioSource != null && enemyExplodeAudio != null)
                enemyExplodeAudio.Play(m_AudioSource);

            if (deadParticlePrefab)
            {
                var explosion = Instantiate(deadParticlePrefab,transform1.position, transform1.rotation);
                Destroy(explosion, deadTimeOutExplosion);
            }
            Tools.ToggleChildren(transform, false);
            Invoke(nameof(DestroyMe), deadTimeOutExplosion);
        }
        #region Gizmos
        void OnDrawGizmosSelected()
        {
        #if UNITY_EDITOR
            
            if (playerApproachRadius <= 0 && playerApproachRadiusRandom.y <= 0) return;
            float realApproachRadius  = playerApproachRadiusRandom != Vector2.zero ? Random.Range(playerApproachRadiusRandom.x, playerApproachRadiusRandom.y) : playerApproachRadius;
            var mineMaster = GetComponent<MineMaster>();
            var position = transform.position;

            // Código que será executado apenas no Editor
            if (playerApproachRadiusRandom == Vector2.zero)
            {
                Gizmos.color = gizmoColor;
                Gizmos.DrawWireSphere(center: position, realApproachRadius);
            }
            if(playerApproachRadiusRandom == Vector2.zero) return;
            Gizmos.color = gizmoColor + new Color(0.2f, 0.2f, 0.2f);
            Gizmos.DrawWireSphere(center: position, playerApproachRadiusRandom.x);
            Gizmos.color = gizmoColor - new Color(0.2f, 0.2f, 0.2f);
            Gizmos.DrawWireSphere(center: position, playerApproachRadiusRandom.y);
        #endif
        }
        #endregion
    }
}
