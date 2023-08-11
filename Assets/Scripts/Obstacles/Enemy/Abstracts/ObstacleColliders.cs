using UnityEngine;
namespace RiverAttack
{
    public abstract class ObstacleColliders : MonoBehaviour
    {
        /*Collider[] m_MyCollider;
        protected ObstacleMaster obstacleMaster;
        protected GamePlayManager gamePlayManager;

        #region UNITY METHODS
        protected virtual void OnEnable()
        {
            SetInitialReferences();
            if (obstacleMaster.enemy.canRespawn)
                gamePlayManager.EventResetEnemies += ColliderOn;
        }
        protected abstract void OnTriggerEnter(Collider collision);
        
        void OnDisable()
        {
            if (obstacleMaster.enemy.canRespawn)
                gamePlayManager.EventResetEnemies -= ColliderOn;
        }
        /*void OnDestroy()
        {
            if (obstacleMaster.enemy.canRespawn)
                gamePlayManager.EventResetEnemies -= ColliderOn;
        }#1#
        #endregion

        protected virtual void SetInitialReferences()
        {
            gamePlayManager = GamePlayManager.instance;
            obstacleMaster = GetComponent<ObstacleMaster>();
            m_MyCollider = GetComponentsInChildren<Collider>();
        }
        protected abstract void HitThis(Collider collision);

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
        protected void ColliderOff()
        {
            int length = m_MyCollider.Length;
            for (int i = 0; i < length; i++)
            {
                m_MyCollider[i].enabled = false;
            }
        }
        void ColliderOn()
        {
            Debug.Log($"Collider: {m_MyCollider}");
            if (m_MyCollider == null)  return;
            int length = m_MyCollider.Length;
            for (int i = 0; i < length; i++)
            {
                m_MyCollider[i].enabled = true;
            }
        }*/
    }
}
