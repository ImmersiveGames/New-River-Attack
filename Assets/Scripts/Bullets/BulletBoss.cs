using UnityEngine;
namespace RiverAttack
{
    public class BulletBoss : Bullets
    {
        private Vector3 direcaoMovimento;
        private float velocidade;
        private float suavizacao = 5.0f;
        float m_StartTime;

        #region UNITYMETHODS
        void OnEnable()
        {
            GamePlayManager.instance.EventEnemiesMasterKillPlayer += DestroyMe;
            if (GamePlayManager.instance.playerDead) return;
            var audioSource = GetComponent<AudioSource>();
            audioShoot.Play(audioSource);
            m_StartTime = Time.time + bulletLifeTime;

        }
        void FixedUpdate()
        {
            Vector3 novaPosicao = transform.position + direcaoMovimento * velocidade * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, novaPosicao, suavizacao * Time.deltaTime);
            AutoDestroyMe(m_StartTime);
        }
        void OnTriggerEnter(Collider collision)
        {
            if ((collision.GetComponentInParent<EnemiesMaster>() && !collision.GetComponentInParent<CollectiblesMaster>()) ||
                collision.GetComponentInParent<BulletEnemy>()) return;
            
            if (collision.GetComponentInParent<WallsMaster>() ||
                collision.GetComponentInParent<EffectAreaMaster>()) return;
            
            DestroyMe();
        }
        void OnBecameInvisible()
        {
            Invoke(nameof(DestroyMe), .01f);
        }
        #endregion
        public void MoveShoot(Vector3 direcao, float velocidadeDisparo)
        {
            direcaoMovimento = direcao.normalized;
            velocidade = velocidadeDisparo;
            
        }
    }
}
