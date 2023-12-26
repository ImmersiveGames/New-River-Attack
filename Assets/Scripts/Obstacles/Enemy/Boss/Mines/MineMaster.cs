using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;
namespace RiverAttack
{
    public class MineMaster: ObstacleMaster, IPoolable
    {
        [SerializeField] GameObject detonationParticles;
        [SerializeField] protected float bulletLifeTime;
        bool m_HasBulletLifeTime;
        Transform m_MyPool;
        #region Delagetes
        public event GeneralEventHandler EventMineAlert;
        #endregion
/*
        #region UNITYMETHODS
        internal override void OnEnable()
        {
            base.OnEnable();
            myColliders ??= GetComponentsInChildren<Collider>();
            MineInvulnerability(true);
        }
        internal override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (other.GetComponent<BulletPlayer>())
            {
                Invoke(nameof(DestroyMe), timeoutDestroyExplosion);
                return;
            }
            ComponentToKill(other.GetComponentInParent<PlayerMaster>(), CollisionType.Collider);
            GamePlayManager.instance.OnEventOtherEnemiesKillPlayer();
            Invoke(nameof(DestroyMe), timeoutDestroyExplosion);
        }
        
        #endregion
        
        public void Initialization(Transform pool)
        {
            m_MyPool = pool;
            Invoke(nameof(MineOffInvulnerability),2f);
        }

        public Transform getPool
        {
            get
            {
                return m_MyPool;
            }
        }

        void MineOffInvulnerability()
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
        protected void AutoDestroyMe(float timer)
        {
            if (m_HasBulletLifeTime && Time.time >= timer)
            {
                DestroyMe();
            }
        }
        void DestroyMe()
        {
            Tools.ToggleChildren(transform);
            gameObject.SetActive(false);
            //StartObstacle();
            if (!m_MyPool)
                return;
            gameObject.transform.SetParent(m_MyPool);
            gameObject.transform.SetAsLastSibling();
        }
        protected virtual void OnEventMineAlert()
        {
            EventMineAlert?.Invoke();
        }*/
    }
}
