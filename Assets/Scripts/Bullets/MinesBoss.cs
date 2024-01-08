using GD.MinMaxSlider;
using UnityEngine;
using Random = UnityEngine.Random;
using Utils;
namespace RiverAttack
{
    public class MinesBoss: Bullets
    {
        [SerializeField] AudioEventSample alertMineAudio;
        [SerializeField] GameObject deadParticlePrefab;
        [SerializeField] float deadTimeOutExplosion;
        [SerializeField] AudioEventSample enemyExplodeAudio;
        [SerializeField] GameObject explosionParticlePrefab;
        [SerializeField] float mineTimeOutExplosion;
        [Header("Approach")]
        [SerializeField] float playerApproachRadius;
        [SerializeField, MinMaxSlider(0f, 20f)] Vector2 playerApproachRadiusRandom;
        public string onAlert;
        #region GizmoSettings
        [Header("Gizmo Settings")]
        public Color gizmoColor = new Color(255, 0, 0, 150);
        #endregion

        [Header("Drop Items")]
        [SerializeField]
        float dropHeight = 1;
        [SerializeField]
        float timeToAutoDestroy;
        [SerializeField, Tooltip("Se o mínimo for diferente de 0 o valor é aleatório entre min e max."), MinMaxSlider(0, 1)]
        Vector2 dropChance;
        GameObject m_ItemDrop;
        public ListDropItems itemsVariables;
        
        float m_StartTime;
        bool m_StartExplosion;
        bool m_IsDestroy;
        PlayerDetectApproach m_PlayerDetectApproach;
        Transform m_Target;
        AudioSource m_AudioSource;
        Animator m_Animator;

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
            if (!GamePlayManager.instance.shouldBePlayingGame)
                return;
            switch (GamePlayManager.instance.readyToFinish)
            {
                case true when gameObject.activeSelf:
                    DestroyMe();
                    return;
                case true:
                    return;
            }
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
            playerApproachRadius = SetPlayerApproachRadius();
        }
        

        internal void Initialization(Transform pool)
        {
            myPool = pool;
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
            
            Tools.ToggleChildren(transform, false);
            var transform1 = transform;
            Instantiate(explosionParticlePrefab,transform1.position, transform1.rotation);
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
        
        void DestroyMeExplosion()
        {
            m_IsDestroy = true;
            var transform1 = transform;
            enemyExplodeAudio.Play(m_AudioSource);
            if (deadParticlePrefab)
            {
                var explosion = Instantiate(deadParticlePrefab,transform1.position, transform1.rotation);
                Destroy(explosion, mineTimeOutExplosion);
            }
            Tools.ToggleChildren(transform, false);
            DropItem();
            Invoke(nameof(DestroyMe), deadTimeOutExplosion);
        }

        void DropItem()
        {
            //Debug.Log($"Start Drop: {dropChance.y}, {itemsVariables}");
            if (dropChance.y <= 0 || itemsVariables == null) return;
            float checkChance = (dropChance.x != 0) ? Random.Range(dropChance.x, dropChance.y) : dropChance.y;
            float sortRange = Random.value;
            //.Log("Sorteio 1 - Chance: "+ checkChance + " Sorteio: " + sortRange);
            if (!(sortRange <= checkChance)) return;
            //Debug.Log("Vai Dropar um item");
            sortRange = Random.value;
            var dropItem = itemsVariables.TakeRandomItem(sortRange);
            if (dropItem.item == null) return;
            //Debug.Log("Dropou o item: " + dropItem.item.name);
            var position = transform.position;
            var dropPosition = new Vector3(position.x, dropHeight, position.z);
            m_ItemDrop = Instantiate(dropItem.item, dropPosition, Quaternion.identity);
            m_ItemDrop.SetActive(true);
            if (timeToAutoDestroy > 0)
                Invoke(nameof(DestroyDrop), timeToAutoDestroy);
        }
        void DestroyDrop()
        {
            Destroy(m_ItemDrop);
        }
        #region Gizmos
        void OnDrawGizmosSelected()
        {
        #if UNITY_EDITOR
            
            if (playerApproachRadius <= 0 && playerApproachRadiusRandom.y <= 0) return;
            float realApproachRadius  = playerApproachRadiusRandom != Vector2.zero ? Random.Range(playerApproachRadiusRandom.x, playerApproachRadiusRandom.y) : playerApproachRadius;
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
