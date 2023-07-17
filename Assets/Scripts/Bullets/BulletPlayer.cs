using UnityEngine;

namespace RiverAttack
{
    public class BulletPlayer : MonoBehaviour
    {
         #region Variable Private Inspector
        [SerializeField] AudioEventSample audioShoot;
        [SerializeField] float shootVelocity = 10f;
        [SerializeField] bool bulletLifeTime = false;
        [SerializeField] float lifeTime = 2f;
        float m_StartTime;
        public PlayerMaster ownerShoot;
        Transform m_MyPool;

        public void SetMyPool(Transform myPool)
        {
            m_MyPool = myPool;
        }
    #endregion

        private void OnEnable()
        {
            var audioSource = GetComponent<AudioSource>();
            audioShoot.Play(audioSource);
        }
        /// <summary>
        /// Executa quando o objeto esta ativo
        /// </summary>
        /// 
        void Start()
        {
            m_StartTime = Time.time + lifeTime;
        }

        public void SetSpeedShoot(double speedy)
        {
            shootVelocity = (float)speedy;
        }
        /// <summary>
        /// Executa a cada atualização de frame da fisica
        /// </summary>
        /// 
        void FixedUpdate()
        {
            MoveShoot();
            AutoDestroy();
        }
        /// <summary>
        /// Proper this object forward
        /// </summary>
        /// 
        void MoveShoot()
        {
            if (GamePlayManager.instance.shouldBePlayingGame)
            {
                float speedy = shootVelocity * Time.deltaTime;
                transform.Translate(Vector3.forward * speedy);
            }
            else
            {
                DestroyMe();
            }

        }
        void OnTriggerEnter(Collider collision)
        {
            if (collision.GetComponentInParent<PlayerMaster>()) return;
            var hitCollectable = (CollectibleScriptable)collision.transform.root.GetComponent<EnemiesMaster>().enemy;
            if (hitCollectable) return;
            DestroyMe();
        }
        /// <summary>
        /// If OnLifeTime set, auto destroyer this object
        /// </summary>
        /// 
        void AutoDestroy()
        {
            if (bulletLifeTime && Time.time >= m_StartTime)
            {
                DestroyMe();
            }
        }
        /// <summary>
        /// Atalho para destruir este objeto
        /// </summary>
        void DestroyMe()
        {
            //Destroy(this.gameObject);

            gameObject.SetActive(false);
            gameObject.transform.SetParent(m_MyPool);
            gameObject.transform.SetAsLastSibling();
        }
        /// <summary>
        /// Se o objeto sai da tela ele é destruido
        /// </summary>
        void OnBecameInvisible()
        {
            Invoke(nameof(DestroyMe), .1f);
        }
    }
}
