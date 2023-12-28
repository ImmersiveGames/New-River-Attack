using System.Collections;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class MinesDetonator : ObstacleDetectApproach
    {
        [SerializeField] ParticleSystem pSystem;
        [SerializeField] GameObject detonationParticles;
        [SerializeField] float timeForDetonation = 3.0f;
        [SerializeField] float colliderSizeFinal = 5.0f;
        [SerializeField] float expansionSpeed = 1.0f;
        [SerializeField] float timeLife;
        [SerializeField] float shakeForce;
        [SerializeField] float shakeTime;
        [SerializeField] long millisecondsVibrate;
        
        bool m_OnExplosion;
        Collider m_Collider;
        float m_EndLife;
        float m_PlayerApproachRadius;
        double m_TParam;
        PlayerDetectApproach m_PlayerDetectApproach;
        MineMaster m_MineMaster;
        internal Coroutine detonation;
        GamePlayManager m_GamePlayManager;

        void OnEnable()
        {
            SetInitialReferences();
            target = null;
            m_OnExplosion = false;
            m_MineMaster.isDestroyed = false;
            detonationParticles.SetActive(false);
            m_PlayerApproachRadius = playerApproachRadius;
            m_Collider = GetComponentInChildren<Collider>();
            timeLife = pSystem.main.duration;
        }
        public void Update()
        {
            var position = transform.position;
            m_PlayerDetectApproach ??= new PlayerDetectApproach(position, m_PlayerApproachRadius);
            target = m_PlayerDetectApproach.TargetApproach<PlayerMaster>(GameManager.instance.layerPlayer);
            if (target == null || !shouldBeExplode)
                return;
            m_OnExplosion = true;
            detonation = StartCoroutine(MineDetonation());
            m_MineMaster.SetMyCoroutine(detonation);
        }

        IEnumerator MineDetonation()
        {
            m_MineMaster.MineOffInvulnerability();
            if (m_Collider.GetType() != typeof(SphereCollider)) yield break;
            
            m_MineMaster.OnEventMineAlert();
            var sphere = (SphereCollider)m_Collider;
            
            yield return new WaitForSeconds(timeForDetonation);
            if (m_MineMaster.isDestroyed) yield break;
            m_EndLife = Time.time + timeLife;
            detonationParticles.SetActive(true);
            
            CameraShake.ShakeCamera(shakeForce, shakeTime);
            
            //TODO: Arrumar nova forma de vibrar o celular
#if UNITY_ANDROID && !UNITY_EDITOR
            ToolsAndroid.Vibrate(millisecondsVibrate);
            Handheld.Vibrate();
#endif
            do
            {
                m_TParam += Time.deltaTime * expansionSpeed;
                sphere.radius = Mathf.Lerp(0.5f, colliderSizeFinal, (float)m_TParam);
            }
            while (sphere.radius < colliderSizeFinal - 0.1f);
            AutoDestroy();
        }

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_GamePlayManager = GamePlayManager.instance;
            m_MineMaster = GetComponent<MineMaster>();
        }
        
        bool shouldBeExplode { get { return m_MineMaster.isActive && m_OnExplosion == false; } }
        void AutoDestroy()
        {
            if (!(Time.time >= m_EndLife))
                return;
            detonationParticles.SetActive(false);
            m_MineMaster.DestroyMe();
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
