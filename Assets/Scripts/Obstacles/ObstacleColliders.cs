using UnityEngine;
namespace RiverAttack
{
    [RequireComponent(typeof(EnemiesMaster))]
    public abstract class ObstacleColliders : MonoBehaviour
    {
        protected EnemiesMaster enemiesMaster;
        protected GamePlayManager gamePlay;
        GameSettings m_GameSettings;
        GameManager m_GameManager;

        #region UNITY METHODS
        protected virtual void OnEnable()
        {
            SetInitialReferences();
        }
        protected virtual void OnTriggerEnter(Collider collision) { }
  #endregion

        protected virtual void SetInitialReferences()
        {
            enemiesMaster = GetComponent<EnemiesMaster>();
            m_GameSettings = GameSettings.instance;
            gamePlay = GamePlayManager.instance;
            m_GameManager = GameManager.instance;
        }
      
        public virtual void HitThis(Collider collision) { }

        //public virtual void HitThis(Collider collision, PlayerMaster playerM = null) { }

        public virtual void CollectThis(Collider collision) { }
        
        protected PlayerMaster WhoHit(Collider collision)
        {
            if (collision.GetComponentInParent<PlayerMaster>())
            {
                collision.GetComponentInParent<PlayerMaster>().AddEnemiesHitList(enemiesMaster.enemy);
                return collision.GetComponentInParent<PlayerMaster>();
            }
            var ownerShoot = collision.GetComponent<Bullets>().GetOwner();
            if (ownerShoot == null) return new PlayerMaster();
            ownerShoot.AddEnemiesHitList(enemiesMaster.enemy);
            return ownerShoot;
        }
        
        protected void ShouldSavePoint()
        {
            if (enemiesMaster.enemy.isCheckInPoint)
                gamePlay.CallEventCheckPoint(transform.position);
        }
    }
}
