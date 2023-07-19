using UnityEngine;
namespace RiverAttack
{
    public class Bullets : MonoBehaviour
    {
        [SerializeField] protected AudioEventSample audioShoot;
        protected internal float shootVelocity;
        protected internal PlayerMaster ownerShoot;
        [Header("Life Time")]
        [SerializeField] protected bool bulletLifeTime = false;
        [SerializeField] protected float lifeTime = 2f;
        protected float startTime;
        public void SetOwner(PlayerMaster owner)
        {
            ownerShoot = owner;
        }
        public PlayerMaster GetOwner()
        {
            return ownerShoot;
        }
    }
}
