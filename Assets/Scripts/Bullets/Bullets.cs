using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class Bullets : MonoBehaviour, IPoolable
    {
        [SerializeField]
        internal int powerFire = 1;
        [SerializeField]
        protected AudioEventSample audioShoot;
        protected float bulletSpeed;
        bool m_HasBulletLifeTime;
        protected float bulletLifeTime;
        protected internal ObjectMaster ownerShoot;
        internal Transform m_MyPool;

        public void Init(float speed, float lifetime = 0)
        {
            bulletSpeed = speed;
            m_HasBulletLifeTime = lifetime > 0;
            bulletLifeTime = lifetime;
        }

        public bool haveAPool
        {
            get { return m_MyPool; }
        }
        public void SetMyPool(Transform pool)
        {
            m_MyPool = pool;
        }
        protected void AutoDestroyMe(float timer)
        {
            if (m_HasBulletLifeTime && Time.time >= timer)
            {
                DestroyMe();
            }
        }
        protected virtual void DestroyMe()
        {
            gameObject.SetActive(false);
            if (!m_MyPool)
                return;
            gameObject.transform.SetParent(m_MyPool);
            gameObject.transform.SetAsLastSibling();
        }
    }
}
