using UnityEngine;
namespace RiverAttack
{
    public abstract class ObstacleColliders : MonoBehaviour
    {
        protected ObstacleMaster obstacleMaster;
        protected GamePlayManager gamePlayManager;

        #region UNITY METHODS
        protected virtual void OnEnable()
        {
            SetInitialReferences();
        }
        protected virtual void OnTriggerEnter(Collider collision) { }
  #endregion

        protected virtual void SetInitialReferences()
        {
            gamePlayManager = GamePlayManager.instance;
            obstacleMaster = GetComponent<ObstacleMaster>();
        }
      
        public virtual void HitThis(Collider collision) { }

        //public virtual void HitThis(Collider collision, PlayerMaster playerM = null) { }

        public virtual void CollectThis(Collider collision) { }
        
        protected PlayerMaster WhoHit(Collider collision)
        {
            if (collision.GetComponentInParent<PlayerMaster>())
            {
                collision.GetComponentInParent<PlayerMaster>().AddEnemiesHitList(obstacleMaster.enemy);
                return collision.GetComponentInParent<PlayerMaster>();
            }
            var ownerShoot = collision.GetComponent<Bullets>().ownerShoot as PlayerMaster;
            if (ownerShoot == null) return null;
            ownerShoot.AddEnemiesHitList(obstacleMaster.enemy);
            return ownerShoot;
        }
        
        protected void ShouldSavePoint()
        {
            if (obstacleMaster.enemy.isCheckInPoint)
                gamePlayManager.CallEventCheckPoint(transform.position);
        }
    }
}
