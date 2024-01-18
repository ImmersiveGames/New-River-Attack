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
        protected bool hasBulletLifeTime;
        protected float bulletLifeTime;
        protected internal ObjectMaster ownerShoot;
        internal Transform myPool;

        public void Init(float speed, float lifetime = 0)
        {
            bulletSpeed = speed;
            hasBulletLifeTime = lifetime > 0;
            bulletLifeTime = lifetime;
        }

        public bool haveAPool
        {
            get { return myPool; }
        }
        public void SetMyPool(Transform pool)
        {
            myPool = pool;
        }
        protected void AutoDestroyMe(float timer)
        {
            if (hasBulletLifeTime && Time.time >= timer)
            {
                DestroyMe();
            }
        }
        protected virtual void DestroyMe()
        {
            gameObject.SetActive(false);
            if (!myPool)
                return;
            gameObject.transform.SetParent(myPool);
            gameObject.transform.SetAsLastSibling();
        }
    }
}
