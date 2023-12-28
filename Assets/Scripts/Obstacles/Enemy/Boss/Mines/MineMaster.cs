using UnityEngine;
using Utils;
using Random = UnityEngine.Random;
namespace RiverAttack
{
    public class MineMaster: ObstacleMaster, IPoolable
    {
        Transform m_MyPool;
        Coroutine m_MyCoroutine;
        #region Delagetes
        public event GeneralEventHandler EventMineAlert;
        #endregion

        #region UNITYMETHODS
        internal override void OnEnable()
        {
            base.OnEnable();
            myColliders ??= GetComponentsInChildren<Collider>();
            MineInvulnerability(true);
            Invoke(nameof(MineOffInvulnerability), 1f);
        }
        internal override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (other.GetComponent<BulletPlayer>())
            {
                isDestroyed = true;
                Invoke(nameof(DestroyMe), timeoutDestroyExplosion);
                return;
            }
            ComponentToKill(other.GetComponentInParent<PlayerMaster>(), CollisionType.Collider);
            isDestroyed = true;
            GamePlayManager.instance.OnEventOtherEnemiesKillPlayer();
            Invoke(nameof(DestroyMe), timeoutDestroyExplosion);
        }
        
        #endregion
        
        public void Initialization(Transform pool)
        {
            m_MyPool = pool;
        }

        public Transform getPool
        {
            get
            {
                return m_MyPool;
            }
        }

        internal void MineOffInvulnerability()
        {
            MineInvulnerability(false);
        }
        void MineInvulnerability(bool active)
        {
            foreach (var collider1 in myColliders)
            {
                collider1.enabled = !active;
            }
        }

        internal void SetMyCoroutine(Coroutine coroutine)
        {
            m_MyCoroutine = coroutine;
        }
        internal void DestroyMe()
        {
            
            if (m_MyCoroutine != null)
            {
                StopCoroutine(m_MyCoroutine);
            }
            Tools.ToggleChildren(transform);
            gameObject.SetActive(false);
            //StartObstacle();
            if (!m_MyPool)
                return;
            gameObject.transform.SetParent(m_MyPool);
            gameObject.transform.SetAsLastSibling();
        }
        public virtual void OnEventMineAlert()
        {
            EventMineAlert?.Invoke();
        }
    }
}
