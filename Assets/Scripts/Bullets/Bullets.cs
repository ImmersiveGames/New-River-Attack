using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class Bullets : MonoBehaviour, IPoolable
    {
        [SerializeField]
        protected AudioEventSample audioShoot;
        protected float bulletSpeed;
        bool m_HasBulletLifeTime;
        protected float bulletLifeTime;
        protected internal ObjectMaster ownerShoot;
        Transform m_MyPool;

        public void Init(float speed,float lifetime = 0)
        {
            SetBulletSpeed(speed);
            SetBulletLifeTime(lifetime);
        }
        
        void SetBulletSpeed(float speed)
        {
            bulletSpeed = speed;
        }

        void SetBulletLifeTime(float lifetime)
        {
            m_HasBulletLifeTime = lifetime > 0;
            bulletLifeTime = lifetime + Time.time;
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
        protected void DestroyMe()
        {
            gameObject.SetActive(false);
            if (!m_MyPool)
                return;
            gameObject.transform.SetParent(m_MyPool);
            gameObject.transform.SetAsLastSibling();
        }
    }
}
